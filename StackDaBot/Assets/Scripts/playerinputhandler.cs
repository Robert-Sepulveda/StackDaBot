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
    [SerializeField]
    GameManager gm;

    InstructionBlock currentInst;
    InstructionBlock currentValue;

    public bool startGame = false;
    private bool noMoreMoves = false;
    private bool invalidMove = false;
    private bool playerDied = false;
    private bool playerWin = false;
    private bool takingAction = false;
    public string gameNotif = "";

    // Update is called once per frame
    void Update()
    {
        if(!startGame)
            return;
        // check win
        CheckWin();
        // even if player wins, we have to check its valid
        CheckDeath();
        if (noMoreMoves||invalidMove||playerDied)
        {
            // send a notification that the game is over if there are no more moves left
            // TODO: send notification to game manager
            MsgGame();
            takingAction=true;
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
            if(!ReadValue())
                return;
        }

        // 4. execute action based off instruction
        if (currentInst.getType() == "Move")
        {
            MovePlayer();
        }
        else if(currentInst.getType() == "Rotate")
        {
            RotatePlayer();
        }
    }

    // handle moving player
    private void MovePlayer()
    {
        takingAction = true;
        NumBlock N = (NumBlock)currentValue;
        playerCharacter.Move(new Vector3(N.number,0f,N.number));
        takingAction = false;
        return;
    }

    // handle player rotation
    private void RotatePlayer()
    {
        takingAction = true;
        Vector3 rotate = Vector3.zero;
        DirectionBlock D = (DirectionBlock)currentValue;
        rotate.y = D.direction;
        playerCharacter.Rotate(rotate);
        takingAction = false;
        return;
    }

    // get the next instruction from instructionhandler
    bool ReadInstruction()
    {
        currentInst = instructionHandler.Pop();
        if(CheckForMove(currentInst))
            return false;
        else if(!currentInst.isInst())
        {
            invalidMove=true;
            gameNotif="invalidMove";
            return false;
        }
        return true;
    }

    // read a value box from instructionhandler
    bool ReadValue()
    {
        currentValue = instructionHandler.Pop();
        if(CheckForMove(currentValue))
            return false;
        if (!CompareValues())
            return false;
        else if(currentValue.isInst())
        {
            invalidMove=true;
            gameNotif="invalidMove";
            return false;
        }
        return true;
    }

    // get an instruction and check for value, return value is determined by the queued instruction
    bool CompareValues()
    {
        if (currentValue.getType() == currentInst.checkValue())
            return true;
        gameNotif="invalidMove";
        invalidMove = true;
        return false;
    }

    // check for moves
    private bool CheckForMove(InstructionBlock block)
    {
        if(block == null)
        {
            noMoreMoves=true;
            gameNotif = "NoMoves";
            return true;
        }
        return false;
    }

    // check for death
    public void CheckDeath()
    {
        if(playerCharacter.CheckKillBoxCollision() || playerCharacter.checkDeath())
        {
            playerDied = true;
            gameNotif = "Death";
            playerCharacter.tanksfx.Pause();
        }
    }

    // check for win
    private void CheckWin()
    {
        if(playerCharacter.CheckExitCollision())
            playerWin = true;
    }

    public bool getWin() => playerWin;

    public void MsgGame()
    {
        gm.GameOver(gameNotif);
    }
}
