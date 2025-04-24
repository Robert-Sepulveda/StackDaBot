using UnityEngine;
using System;
using System.Collections.Generic;

public class instructionhandler : MonoBehaviour
{
    Stack<InstructionBlock> instructionReader = new Stack<InstructionBlock>();

    // todo: implement push and pop
    void Push(InstructionBlock block)
    {
        instructionReader.Push(block);
    }

    // return the next instruction from the end of the array
    public InstructionBlock Pop()
    {
        return instructionReader.Pop();
    }
}
