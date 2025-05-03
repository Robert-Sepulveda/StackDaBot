using UnityEngine;

public class NumBlock : InstructionBlock
{
    public string type = "Num";
    public bool value = false;
    public string valueType = "None";
    public float number;

    public NumBlock(float num)
    {
        number = num;
    }
    
    public bool isInst() => false;
    public string getType() => type;
    public bool needValue() => value;
    public string checkValue() => valueType;
}