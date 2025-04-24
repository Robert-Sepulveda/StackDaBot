using UnityEngine;

public class playerinputhandler : MonoBehaviour
{
    [SerializeField]
    PlayerRobot playerCharacter;
    [SerializeField]
    instructionhandler instructionHandler;

    InstructionBlock currentInst;
    InstructionBlock value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotate = Vector3.zero;

        // 1. check if player can move
        if (playerCharacter.CanMove())
            // 2. get the next instruction from the instruction stack
            ReadInstruction();

        // 3. get a value if needed
        if(currentInst.needValue)
            GetValue(currentInst.type);

        // todo: execute movement based off instruction
        // player movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerCharacter.Move(new Vector3(5f,0f,5f));
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            rotate.y = 180;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            rotate.y = -90;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            rotate.y += 90;
        }

        playerCharacter.Rotate(rotate);
    }

    // get the next instruction from the instruction handler and queue it for movement
    void ReadInstruction()
    {
        currentInst = instructionHandler.Pop(); 
    }

    // get an instruction and check for value, return value is determined by the queued instruction
    bool GetValue(string type)
    {
        value = instructionHandler.Pop();
        if (value.type != "val")
            return false;

        return currentInst.checkValue();
    }
}
