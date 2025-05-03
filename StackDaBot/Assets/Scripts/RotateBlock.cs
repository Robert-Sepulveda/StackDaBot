using UnityEngine;

public class RotateBlock : InstructionBlock
{
    public string type = "Rotate";
    public bool value = true;
    public string valueType = "Direction";

    public bool isInst() => true;
    public string getType() => type;
    public bool needValue() => value;
    public string checkValue() => valueType;
}