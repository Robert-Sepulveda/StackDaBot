using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("color")]
    [SerializeField]
    string color = "yellow";

    public bool checkColor(string playerColor)
    {
        if(color == playerColor)
            return true;
        return false;
    }
}
