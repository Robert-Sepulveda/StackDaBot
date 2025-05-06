using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("playerinput")]
    [SerializeField]
    playerinputhandler pih;

    [Header("instruction handler")]
    [SerializeField]
    instructionhandler ih;

    [Header("Buttons")]
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button moveButton;
    [SerializeField]
    Button rotateButton;
    [SerializeField]
    Button num1;
    [SerializeField]
    Button num2;
    [SerializeField]
    Button num3;
    [SerializeField]
    Button num4;
    [SerializeField]
    Button num5;
    [SerializeField]
    Button left;
    [SerializeField]
    Button right;
    [SerializeField]
    Button back;

    [Header("SubMenus")]
    [SerializeField]
    GameObject PreGame;
    [SerializeField]
    GameObject IntMenu;
    [SerializeField]
    GameObject DirMenu;

    private int NUMOFSPACES=5;

    void Start()
    {
        startButton.onClick.AddListener(startGame);
        moveButton.onClick.AddListener(createMoveBlock);
        rotateButton.onClick.AddListener(createRotateBlock);
        num1.onClick.AddListener(()=>createNumBlock(1));
        num2.onClick.AddListener(()=>createNumBlock(2));
        num3.onClick.AddListener(()=>createNumBlock(3));
        num4.onClick.AddListener(()=>createNumBlock(4));
        num5.onClick.AddListener(()=>createNumBlock(5));
        left.onClick.AddListener(()=>createNumBlock(-90));
        right.onClick.AddListener(()=>createNumBlock(90));
        back.onClick.AddListener(()=>createNumBlock(180));
    }

    public void startGame()
    {
        TogglePanelOff(PreGame);
        pih.startGame = true;
    }

    public void createMoveBlock()
    {
        TogglePanelOn(IntMenu);
        TogglePanelOff(DirMenu);
        ih.SpawnBlock(new MoveBlock());
        return;
    }

    public void createRotateBlock()
    {
        TogglePanelOn(DirMenu);
        TogglePanelOff(IntMenu);
        ih.SpawnBlock(new RotateBlock());
        return;
    }

    public void createNumBlock(float value)
    {
        TogglePanelOff(IntMenu);
        ih.SpawnBlock(new NumBlock(value));
    }

    public void createDirBlock(int value)
    {
        TogglePanelOff(DirMenu);
        ih.SpawnBlock(new DirectionBlock(value));
    }

    public void TogglePanelOn(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void TogglePanelOff(GameObject panel)
    {
        panel.SetActive(false);
    }
}
