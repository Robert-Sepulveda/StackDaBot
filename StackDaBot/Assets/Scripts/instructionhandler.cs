using UnityEngine;
using System;
using System.Collections.Generic;

public class instructionhandler : MonoBehaviour
{
    [Header("block types")]
    [SerializeField]
    GameObject moveBlock;
    [SerializeField]
    GameObject rotateBlock;
    [SerializeField]
    GameObject numBlock;
    [SerializeField]
    GameObject dirBlock;
    Stack<InstructionBlock> instructionReader = new Stack<InstructionBlock>();
    void Start()
    {
        // Push(new NumBlock(3f));
        // Push(new MoveBlock());
        // Push(new DirectionBlock(-90));
        // Push(new RotateBlock());
        // Push(new NumBlock(5f));
        // Push(new MoveBlock());
        // Push(new DirectionBlock(180));
        // Push(new RotateBlock());
        // Push(new NumBlock(5f));
        // Push(new MoveBlock());
        // Push(new DirectionBlock(-90));
        // Push(new RotateBlock());
        // Push(new NumBlock(5f));
        // Push(new MoveBlock());
    }

    // pushes an instruction onto the stack
    private void Push(InstructionBlock block)
    {
        instructionReader.Push(block);
    }

    // return the next instruction from the end of the array
    public InstructionBlock Pop()
    {
        if (instructionReader.Count == 0)
            return null;
        InstructionBlock block = instructionReader.Pop();
        Destroy(block.getObject());
        return block;
    }

    // handles the creation of a given instruction block
    public void SpawnBlock(InstructionBlock block)
    {
        string type = block.getType();
        // spawn block and assign it to code instance
        if(type == "Move")
            block.setObject(Instantiate(moveBlock,transform.position,transform.rotation));
        else if(type == "Rotate")
            block.setObject(Instantiate(rotateBlock,transform.position,transform.rotation));
        else if(type == "Direction")
            block.setObject(Instantiate(dirBlock,transform.position,transform.rotation));
        else if(type == "Num")
            block.setObject(Instantiate(numBlock,transform.position,transform.rotation));
            // block.setObject(Spawner.SpawnBlock(numBlock));
        Push(block);
    }

    public bool isEmpty()
    {
        if (instructionReader.Count > 0)
            return false;
        return true;
    }
}
