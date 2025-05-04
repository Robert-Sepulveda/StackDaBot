using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Collections;

public class playerinputhandler : MonoBehaviour
{
    [SerializeField]
    PlayerRobot playerCharacter;
    [SerializeField]
    instructionhandler instructionHandler;

    InstructionBlock currentInst;
    InstructionBlock currentValue;

    private bool noMoreMoves = false;
    private bool invalidMove = false;
    private bool takingAction = false;

    // Update is called once per frame
    void Update()
    {

        // before taking the next action, we check for any game ending scenarios
        if (noMoreMoves)
        {
            // send a notification that the game is over if there are no more moves left
            // TODO: send notification to game manager
            return;
        }
        else if(invalidMove)
        {
            // TODO: send a notification to a game manager
            return;
        }
        
        // 1. check if player can move
        if (!playerCharacter.CanMove() || takingAction)
            return;
        // 2. get the next instruction from the instruction stack
        if(!ReadInstruction())
        {
            return;
        }
        // 3. get a value if needed
        if(currentInst.needValue())
        {
            if(!ReadValue() || !CompareValues())
                return;
        }

        // 4. execute action based off instruction
        if (currentInst.getType() == "Move")
        {
            MovePlayer();
        }
        else if(currentInst.getType() == "Rotate")
        {
            StartCoroutine(RotatePlayer());
        }
    }

    private void MovePlayer()
    {
        takingAction = true;
        NumBlock N = (NumBlock)currentValue;
        playerCharacter.Move(new Vector3(N.number,0f,5f));
        takingAction = false;
        return;
    }

    private IEnumerator RotatePlayer()
    {
        takingAction = true;
        Vector3 rotate = Vector3.zero;
        DirectionBlock D = (DirectionBlock)currentValue;
        rotate.y = D.direction;
        playerCharacter.Rotate(rotate);
        yield return new WaitForSeconds(1f);
        takingAction = false;
    }

    // get the next instruction from the instruction handler and queue it for movement, return true upon success
    bool ReadInstruction()
    {
        currentInst = instructionHandler.Pop();
        if(currentInst == null)
        {
            noMoreMoves=true;
            return false;
        }
        else if(!currentInst.isInst())
        {
            invalidMove=true;
            return false;
        }
        return true;
    }

    bool ReadValue()
    {
        currentValue = instructionHandler.Pop();
        if(currentValue == null)
        {
            noMoreMoves=true;
            return false;
        }
        return true;
    }

    // get an instruction and check for value, return value is determined by the queued instruction
    bool CompareValues()
    {
        if (currentValue.getType() == currentInst.checkValue())
            return true;
        invalidMove = true;
        return false;
    }
}
