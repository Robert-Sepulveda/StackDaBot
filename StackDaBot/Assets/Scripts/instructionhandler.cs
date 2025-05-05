using UnityEngine;
using System;
using System.Collections.Generic;

public class instructionhandler : MonoBehaviour
{
    Stack<InstructionBlock> instructionReader = new Stack<InstructionBlock>();
    void Start()
    {
        Push(new NumBlock(3f));
        Push(new MoveBlock());
        Push(new DirectionBlock(-90));
        Push(new RotateBlock());
        Push(new NumBlock(5f));
        Push(new MoveBlock());
        Push(new DirectionBlock(180));
        Push(new RotateBlock());
        Push(new NumBlock(5f));
        Push(new MoveBlock());
        Push(new DirectionBlock(-90));
        Push(new RotateBlock());
        Push(new NumBlock(5f));
        Push(new MoveBlock());
    }

    // todo: implement push and pop
    void Push(InstructionBlock block)
    {
        instructionReader.Push(block);
    }

    // return the next instruction from the end of the array
    public InstructionBlock Pop()
    {
        if (instructionReader.Count == 0)
            return null;
        return instructionReader.Pop();
    }
}
