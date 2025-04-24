using System;
using UnityEngine;

public class PlayerRobot : MonoBehaviour
{   
    [Header("Movement")]
    [SerializeField]
    float moveSpeed = 0.25f;
    [SerializeField]
    float snapDistance = 0.05f;
    [SerializeField]
    float rotateSnapDistance = 1;

    [Header("Raycasting")]
    [SerializeField]
    LayerMask Ignore;

    float rayLength = 1f;
    Vector3 targetPosition;
    Vector3 targetRotate;
    bool isPlayerMoving = false;
    bool isPlayerRotating = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move player
        if(isPlayerMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) > snapDistance)
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            else
            {
                // snap player to exact targetPosition
                transform.position = targetPosition;
                isPlayerMoving = false;
            }
        }

        // rotate player
        // if(isPlayerRotating)
        // {
        //     Debug.Log(targetRotate);
        //     Debug.Log(transform.forward);
        //     Debug.Log(Vector3.Angle(targetRotate, transform.forward));
        //     if (Vector3.Angle(targetRotate, transform.forward) > rotateSnapDistance)
        //         transform.Rotate(targetRotate * moveSpeed * Time.deltaTime,Space.World);
        //     else
        //     {
        //         transform.Rotate(targetRotate,Space.World);
        //         isPlayerRotating = false;
        //     }
        // }
    }

    public void Rotate(Vector3 rotateVector)
    {
        if(rotateVector == Vector3.zero || isPlayerMoving)
            return;
        transform.Rotate(rotateVector,Space.World);
        targetRotate = rotateVector;
        isPlayerRotating = true;
    }

    public void Move(Vector3 distance)
    {
        // check for value and if already moving
        if(distance == Vector3.zero|| isPlayerMoving)
            return;

        // return if player hits an obstacle
        if(Physics.Raycast(transform.position,transform.forward,rayLength,~Ignore))
            return;
        
        // set target variables so update() can move the player over time
        Debug.Log(transform.forward);
        Vector3 forward = transform.forward;
        forward.x *= distance.x;
        forward.z *= distance.z;
        targetPosition = transform.position + forward;
        isPlayerMoving = true;
    }

    public bool CanMove()
    {
        return isPlayerMoving;
    }
}
