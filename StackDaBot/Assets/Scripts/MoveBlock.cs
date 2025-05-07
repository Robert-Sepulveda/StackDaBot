using UnityEngine;

public class MoveBlock : InstructionBlock
{
    public string type = "Move";
    public bool value = true;
    public string valueType = "Num";
    public GameObject prefab;

    public GameObject getObject() => prefab;
    public bool isInst() => true;
    public string getType() => type;
    public bool needValue() => value;
    public string checkValue() => valueType;

    public bool setObject(GameObject reference)
    {
        prefab = reference;
        if(prefab)
            return true;
        else
            return false;
    }
}