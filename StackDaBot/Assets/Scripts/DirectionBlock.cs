using UnityEngine;

public class DirectionBlock : InstructionBlock
{
    public string type = "Direction";
    public bool value = false;
    public string valueType = "None";
    public float direction;

    public DirectionBlock(float dir)
    {
        direction = dir;
    }

    public bool isInst() => false;
    public string getType() => type;
    public bool needValue() => value;
    public string checkValue() => valueType;
}