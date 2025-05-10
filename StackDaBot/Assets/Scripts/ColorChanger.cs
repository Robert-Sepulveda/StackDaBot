using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Header("color")]
    [SerializeField]
    string color = "yellow";

    public string getColor()
    {
        return color;
    }
}
