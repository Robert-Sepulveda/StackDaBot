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

    float rayLength = 0.5f;
    public float floorRayLength = 0.55f;
    Vector3 targetPosition;
    Vector3 targetRotate;
    bool isPlayerMoving = false;
    bool isPlayerRotating = false;
    bool isFalling = false;
    string color = "green";

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="ColorChanger")
        {
            // change color
            Debug.Log("Triggered by color changer");
        }
        else if(other.tag=="ColorGate")
        {
            // check color
            Debug.Log("Triggered by color gate");
        }
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
        yield return new WaitForSeconds(1f);
        isPlayerMoving = false;
    }

    private IEnumerator RotatePlayer(float duration)
    {
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
        yield return new WaitForSeconds(1f);
        isPlayerRotating = false;
    }
}
