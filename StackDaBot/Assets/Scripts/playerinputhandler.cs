using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        Vector3 rotate = Vector3.zero;

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
        if (!playerCharacter.CanMove())
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
            NumBlock N = (NumBlock)currentValue;
            playerCharacter.Move(new Vector3(N.number,0f,5f));
        }
        else if(currentInst.getType() == "Rotate")
        {
            DirectionBlock D = (DirectionBlock)currentValue;
            rotate.y = D.direction;
        }

        playerCharacter.Rotate(rotate);
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
        Debug.Log(currentInst.getType());
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
        Debug.Log(currentValue.getType());
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
