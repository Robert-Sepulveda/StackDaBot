using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Probably the ugliest UI management - I have no clue what Im doing
    // sorry :P
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
    Button ContorQuit;
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
    GameObject StartHint;

    [Header("TextBoxes")]
    [SerializeField]
    TMP_Text WinPanelText;
    [SerializeField]
    TMP_Text HintText;
    [SerializeField]
    TMP_Text Quit;

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
        restart.onClick.AddListener(restartLevel);
    }

    public void GameOver(string msg)
    {
        if (msg == "NoMoves")
            if(pih.getWin())
                win = true;
            else
                hint = "Stack : Hey I need more Instructions!";
        else if (msg == "Death")
            hint = "Stack : YOU KILLED ME!";
        else if (msg == "invalidMove")
            hint = "Stack : Hey I can't read these instructions!";

        if (win)
        {
            WinPanelText.text = "You Win!";
            hint = "Stack : W's in da chat";
        }
        else
        {
            WinPanelText.text = "You Lose!";
            Quit.text = "Quit";
            ContorQuit.onClick.AddListener(quitGame);
        }
        HintText.text = hint;
        TogglePanelOn(WinMenu);
    }

    public void startGame()
    {
        if(!ih.isEmpty())
        {
            TogglePanelOff(PreGame);
            pih.startGame = true;
        }
        else
            TogglePanelOn(StartHint);
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

    public void restartLevel()
    {
        SceneManager.LoadScene("Level0");
    }

    public void quitGame()
    {
        // safe and cleaner to use preprocessor directives
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
