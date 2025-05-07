using UnityEngine;

public interface InstructionBlock
{
    public bool isInst();
    public string getType();
    public bool setObject(GameObject reference);
    public GameObject getObject();

    // return true if value block needed
    public bool needValue();

    // return value type needed for block
    public string checkValue();
}