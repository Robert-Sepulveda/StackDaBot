using UnityEngine;

public class MoveBlock : InstructionBlock
{
    public string type = "Move";
    public bool value = true;
    public string valueType = "Num";

    public bool isInst() => true;
    public string getType() => type;
    public bool needValue() => value;
    public string checkValue() => valueType;
}