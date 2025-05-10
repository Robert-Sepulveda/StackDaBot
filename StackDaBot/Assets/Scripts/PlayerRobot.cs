using System;
using UnityEngine;
using System.Collections;

public class PlayerRobot : MonoBehaviour
{   
    [Header("Movement")]
    [SerializeField]
    float moveSpeed = 0.25f;
    [SerializeField]
    float snapDistance = 0.05f;
    [SerializeField]
    float rotateSpeed = 0.5f;

    [Header("Raycasting")]
    [SerializeField]
    LayerMask wallMask;
    [SerializeField]
    LayerMask killBoxMask;
    [SerializeField]
    LayerMask exitMask;
    [SerializeField]
    LayerMask colorChanger;
    [SerializeField]
    LayerMask energyGate;

    [Header("Material")]
    [SerializeField]
    Material tankMetal;

    [Header("Audio")]
    [SerializeField]
    AudioClip tankMovingSFX;
    [SerializeField]
    AudioSource sfx;
    [SerializeField]
    public AudioSource tanksfx;
    [SerializeField]
    AudioClip energyDeath;
    [SerializeField]
    AudioClip energyTraverse;
    [SerializeField]
    AudioClip changeColors;



    RaycastHit hit;
    float rayLength = 0.5f;
    float floorRayLength = 0.18f;
    Vector3 targetPosition;
    Vector3 targetRotate;
    bool isPlayerMoving = false;
    bool isPlayerRotating = false;
    bool isFalling = false;
    bool isPlayerDead = false;
    bool playedTraverseSound = false;
    bool playedChangeSound = false;
    string color = "none";
    Renderer ren;
    Color baseColor = new Color (127/255f,167/255f,183/255f);

    void Start()
    {
        tankMetal.color = baseColor;
    }

    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit,rayLength,colorChanger))
        {
            ColorChanger changer = hit.transform.GetComponent<ColorChanger>();
            if(changer != null)
            {
                changeSound();
                color = changer.getColor();
                changeColor();
            }
        }
        else if(Physics.Raycast(transform.position,transform.forward,out hit,rayLength,energyGate))
        {
            Gate gate = hit.transform.GetComponent<Gate>();
            if(gate != null)
            {
                if(!gate.checkColor(color))
                {
                    energyDeathSound();
                    isPlayerDead = true;
                }
                else
                {
                    traverseSound();
                }
                
            }
        }
        ApplyGravity();
    }

    private void changeSound()
    {
        if(playedChangeSound)
            return;
        playedChangeSound = true;
        sfx.resource = changeColors;
        sfx.time=10.0f;
        sfx.Play();
    }

    private void traverseSound()
    {
        if(playedTraverseSound)
            return;
        playedTraverseSound = true;
        sfx.resource = energyTraverse;
        sfx.Play();
    }

    private void energyDeathSound()
    {
        if(playedTraverseSound)
            return;
        playedTraverseSound = true;
        sfx.resource = energyDeath;
        sfx.Play();
    }

    private void changeColor()
    {
        if(color == "yellow")
        {
            tankMetal.color = Color.yellow;
        }
        if(color=="blue")
        {
            tankMetal.color = Color.blue;
        }
    }

    public bool checkDeath()
    {
        return isPlayerDead;
    }

    public void Rotate(Vector3 rotateVector)
    {
        if(rotateVector == Vector3.zero || !CanMove())
            return;
        targetRotate = rotateVector;
        isPlayerRotating = true;
        StartCoroutine(RotatePlayer(rotateSpeed));
    }

    public void Move(Vector3 distance)
    {
        // check for value and if already moving
        if(distance == Vector3.zero|| !CanMove())
            return;
        
        // set target variables so update() can move the player over time
        Vector3 forward = transform.forward;
        forward.x *= distance.x;
        forward.z *= distance.z;
        targetPosition = transform.position + forward;
        isPlayerMoving = true;
        StartCoroutine(MovePlayer());
        return;
    }

    public bool CanMove()
    {
        if (isPlayerMoving || isPlayerRotating)
            return false;
        return true;
    }

    // takes a mask and returns true if there is a raycast collision detected
    private bool CheckCollision(Vector3 pos, Vector3 ray, float len, LayerMask mask)
    {
        Debug.DrawRay(pos,ray * len,Color.green);
        return Physics.Raycast(pos, ray, len, mask);
    }

    // checks and applies gravity to player
    private void ApplyGravity()
    {
        // check floor collision
        isFalling = !CheckCollision(transform.position, Vector3.down, floorRayLength, wallMask);

        // apply if no floor detected
        if(isFalling)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            targetPosition.y = transform.position.y;
        }
        return;
    }

    public bool CheckKillBoxCollision()
    {
        return CheckCollision(transform.position, Vector3.down, floorRayLength, killBoxMask);
    }

    public bool CheckExitCollision()
    {
        return CheckCollision(transform.position,transform.forward,rayLength,exitMask);
    }

    private IEnumerator MovePlayer()
    {
        tanksfx.resource = tankMovingSFX;
        tanksfx.time=30f;
        tanksfx.Play();
        while(transform.position != targetPosition)
        {
            // check for collisions
            // return if player hits an obstacle
            if(CheckCollision(transform.position,transform.forward,rayLength,wallMask))
            {
                targetPosition = transform.position;
            }
            else if (Vector3.Distance(transform.position, targetPosition) > snapDistance)
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            else
            {
                // snap player to exact targetPosition
                transform.position = targetPosition;
            }
            yield return null;
        }
        tanksfx.Stop();
        yield return new WaitForSeconds(1f);
        isPlayerMoving = false;
    }

    private IEnumerator RotatePlayer(float duration)
    {
        tanksfx.resource = tankMovingSFX;
        tanksfx.time=30f;
        tanksfx.Play();
        float yRotation;
        float startAngle = transform.eulerAngles.y;
        targetRotate.y += startAngle;
        float time = 0f;
        // rotate player
        // while (Vector3.Angle(targetRotate, transform.forward) > rotateSnapDistance)
        while(time < duration)
        {
            time += Time.deltaTime;
            yRotation = Mathf.Lerp(startAngle,targetRotate.y,time/duration) %360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation,transform.eulerAngles.z);
            yield return null;
        }
        tanksfx.Stop();
        yield return new WaitForSeconds(1f);
        isPlayerRotating = false;
    }
}
