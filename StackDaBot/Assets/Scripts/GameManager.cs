using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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
    Button valuesButton;
    [SerializeField]
    Button actionsButton;
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
    [SerializeField]
    Button restart;
    [SerializeField]
    Button NLorQuit;
    [SerializeField]
    Button levels;

    [Header("SubMenus")]
    [SerializeField]
    GameObject PreGame;
    [SerializeField]
    GameObject IntMenu;
    [SerializeField]
    GameObject DirMenu;
    [SerializeField]
    GameObject InstMenu;
    [SerializeField]
    GameObject WinMenu;
    [SerializeField]
    GameObject LoseMenu;
    [SerializeField]
    TMP_Text WinPanelText;

    string hint;
    private bool win = false;

    void Start()
    {
        startButton.onClick.AddListener(startGame);
        valuesButton.onClick.AddListener(openValues);
        actionsButton.onClick.AddListener(openActions);
        moveButton.onClick.AddListener(createMoveBlock);
        rotateButton.onClick.AddListener(createRotateBlock);
        num1.onClick.AddListener(()=>createNumBlock(1));
        num2.onClick.AddListener(()=>createNumBlock(2));
        num3.onClick.AddListener(()=>createNumBlock(3));
        num4.onClick.AddListener(()=>createNumBlock(4));
        num5.onClick.AddListener(()=>createNumBlock(5));
        left.onClick.AddListener(()=>createDirBlock(-90));
        right.onClick.AddListener(()=>createDirBlock(90));
        back.onClick.AddListener(()=>createDirBlock(180));
    }

    public void GameOver(string msg)
    {
        if (msg == "NoMoves")
            if(pih.getWin())
                win = true;
            else
                hint = "Hey I need more Instructions!";
        else if (msg == "Death")
            hint = "YOU DIED";
        else if (msg == "Invalid")
            hint = "Hey I can't read these instructions!";

        if (win)
        {
            WinPanelText.text = "You Win!";
        }
        else
        {
            WinPanelText.text = "You Lose!";
        }
        TogglePanelOn(WinMenu);
    }

    public void startGame()
    {
        TogglePanelOff(PreGame);
        pih.startGame = true;
    }

    public void openValues()
    {
        TogglePanelOn(DirMenu);
        TogglePanelOn(IntMenu);
        TogglePanelOff(InstMenu);
    }

    public void openActions()
    {
        TogglePanelOff(DirMenu);
        TogglePanelOff(IntMenu);
        TogglePanelOn(InstMenu);
    }

    public void createMoveBlock()
    {
        TogglePanelOff(InstMenu);
        ih.SpawnBlock(new MoveBlock());
        return;
    }

    public void createRotateBlock()
    {
        TogglePanelOff(InstMenu);
        ih.SpawnBlock(new RotateBlock());
        return;
    }

    public void createNumBlock(float value)
    {
        TogglePanelOff(IntMenu);
        TogglePanelOff(DirMenu);
        ih.SpawnBlock(new NumBlock(value));
    }

    public void createDirBlock(int value)
    {
        TogglePanelOff(IntMenu);
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
