using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;
    public HeroPath heroPath;
    public bool IsHeroSubmitted { get; private set; }

    public TMP_Text coinText;
    private int coinCountRed;
    private int coinCountBlue = 3;

    public TMP_Text battleBonusTextR;
    public TMP_Text battleBonusTextB;
    private int pointCountRed;
    private int pointCountBlue;
    [SerializeField] private int winPoint;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private TMP_Text WinnerText;

    [SerializeField] private PauseMenu pauseMenu;
    private bool isGamePaused;
    public TMP_Text currentTurnText;
    public TMP_Text owenedHeroText;

    public TMP_Text errorText;
    public TMP_Text inputText;

    // public string controlTextValue { get; set; }
    public int battleBonus { get; private set; }

    public string diceResult;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

  
    public void ReplayGame()
    {


        // Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }
    public void HandlePauseGame()
    {
        Debug.Log("Pause in gameManager");
        //pause game
        if (pauseMenu != null && isGamePaused ==false)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
            isGamePaused = true;
        }else if(isGamePaused == true)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
            isGamePaused = false;
        }


    }
    

    private void Start()
    {
        coinCountRed = 3;
        coinCountBlue = 3;
        coinText.text ="Coin:" +coinCountRed.ToString();

        HeroOwned();
        battleBonusTextB.text = "";
        inputText.text = "WASD_move;Enter_submit ;Q_cancle";
        GameInit();

    }
    public void GameInit()
    {
        isGamePaused = false;
        winMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
     
    }
    private void Update()
    {
        UpdatePlayerTurn();
        HeroOwned();

        battleBonusTextR.text = "Point1:" + battleBonusRed.ToString() + "/10";
        battleBonusTextB.text = "Point2:" + battleBonusBlue.ToString() + "/10";

    }
    public void DisplayErrorText(string value)
    {
        inputText.text = "";
        errorText.text = value;
        StartCoroutine(ClearTextAfterDelay(5f));
    }

    public void DisplayInputText(string value)
    {
        inputText.text = value;
    }

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        errorText.text = "";
        Debug.Log("Text cleared after " + delay + " seconds");
    }

    public void UpdateHeroSubmissionState(bool state)
    {
        IsHeroSubmitted = state;
    }

    public void AddPointCount()
    {
        var pointCount = coinCountRed;

        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            pointCount = pointCountRed;
            pointCountRed++;
        }
        else
        {
            pointCount = pointCountBlue;
            pointCountBlue++;
        }
        coinText.text = "Coin:" + "" + pointCount.ToString();
    }

    public void AddCoinCount()
    {
        var coinCount = coinCountRed;

        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            coinCount = coinCountRed;
            coinCountRed++;
        }
        else
        {
            coinCount = coinCountBlue;
            coinCountBlue++;
        }
        coinText.text = "Coin:" + "" + coinCount.ToString();
        UINavManager.Instance.UpdateShopButtons();
    }
    public void DecreaseCoinCount(int value)
    {
        Debug.Log("cost is " + value);

        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            coinCountRed -= value;
            Debug.Log("red coin " + coinCountRed);
            coinText.text = "Coin:" + "" + coinCountRed.ToString();
        }
        else
        {
            coinCountBlue -= value;
            Debug.Log("red coin " + coinCountRed);
            coinText.text = "Coin:" + "" + coinCountBlue.ToString();
        }

    }
    public int GetCurrentTurnCoin()
    {
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            return coinCountRed;

        }
        else
        {
            return coinCountBlue;
        }


    }



    public void UpdatePlayerTurn()
    {
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            currentTurnText.text = "Red Player";
        }
        else
        {
            currentTurnText.text = "Blue Player";
        }

    }
    public int GetHeroCount()
    {

        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            return HeroPocketManager.Instance.GetAllRedSideHeroes().Count;
        }
        else
        {
            return HeroPocketManager.Instance.GetAllBlueSideHeroes().Count;
        }

    }

    public void HeroOwned()
    {
        owenedHeroText.text = "Own Hero:" + GetHeroCount();
    }

    int battleBonusRed = 0;
    int battleBonusBlue = 0;
    internal bool isBattling = true;

    public void AddbattleBonus(string side, int value)
    {
        if (side == null) return;

        Debug.Log("(destroy.side" + side);
        Debug.Log("(obj.bonus" + value);
        if (side == "Red")
        {
            Debug.Log("add bonus to blue" + value);
            battleBonusBlue += value;
            battleBonusTextB.text = "Point2:" + battleBonusBlue.ToString();

        }
        else if (side == "Blue")
        {
            Debug.Log("add bonus to red" + value);
            battleBonusRed += value;
            battleBonusTextR.text = "Point1:" + battleBonusRed.ToString();
        }

        CheckWin(side);

    }
    private void CheckWin(string side)
    {
        Debug.Log("CheckWin");
        if (battleBonusRed >= winPoint || battleBonusBlue >= winPoint)
        {
            Debug.Log(currentTurn + "some one win");
            ShowWinner( side);
        }

    }

    public void ShowWinner(string side)
    {
        Debug.Log("side"+side);
        //yield return new WaitForSeconds(1f);
        Debug.Log(side+"Player " + "win");
        winMenu.gameObject.SetActive(true);
        WinnerText.text = currentTurn.ToString() +"Win";
    }


}



