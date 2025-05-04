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
    [SerializeField]
    float rotateSnapDistance = 1f;

    [Header("Raycasting")]
    [SerializeField]
    LayerMask Ignore;
    [SerializeField]
    public LayerMask floorLayerMask;

    float rayLength = 0.5f;
    float floorRayLength = 0.55f;
    Vector3 targetPosition;
    Vector3 targetRotate;
    bool isPlayerMoving = false;
    bool isPlayerRotating = false;
    bool isFalling = false;

    // Update is called once per frame
    void Update()
    {
        // floor collision
        isFalling = !Physics.Raycast(transform.position, Vector3.down, floorRayLength, ~Ignore);
        Debug.DrawRay(transform.position,Vector3.down * floorRayLength,Color.green);

        if(isFalling)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            targetPosition.y = transform.position.y;
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

    private IEnumerator MovePlayer()
    {
        while(transform.position != targetPosition)
        {
            // check for collisions
            // return if player hits an obstacle
            Debug.DrawRay(transform.position,transform.forward,Color.red);
            if(Physics.Raycast(transform.position,transform.forward,rayLength,~Ignore))
            {
                targetPosition = transform.position;
            }

            if (Vector3.Distance(transform.position, targetPosition) > snapDistance)
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
