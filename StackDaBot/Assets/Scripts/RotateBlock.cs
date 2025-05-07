using UnityEngine;

public class RotateBlock : InstructionBlock
{
    public string type = "Rotate";
    public bool value = true;
    public string valueType = "Direction";
    public GameObject prefab;

    public bool isInst() => true;
    public string getType() => type;
    public bool needValue() => value;
    public string checkValue() => valueType;
    public GameObject getObject() => prefab;

    public bool setObject(GameObject reference)
    {
        prefab = reference;
        if(prefab)
            return true;
        else
            return false;
    }
}