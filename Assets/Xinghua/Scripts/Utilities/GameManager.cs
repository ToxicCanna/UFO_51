using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;
    public HeroPath heroPath;
    public bool IsHeroSubmitted { get; private set; }
    private int coinCountRed;
    private int coinCountBlue = 3;

    private int pointCountRed;
    private int pointCountBlue;
    public TMP_Text coinText;
    public TMP_Text currentTurnText;
    public TMP_Text owenedHeroText;

    public TMP_Text battleBonusTextR;
    public TMP_Text battleBonusTextB;
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



    private void Start()
    {
        coinCountRed = 3;
        coinCountBlue = 3;
        coinText.text = coinCountRed.ToString();
       
        HeroOwned();
        battleBonusTextB.text = "";
        inputText.text = "WASD_move;Enter_submit ;Q_cancle";
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
        Debug.Log("8888" + value);
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
        // Debug.Log("cost is " + value);

        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            coinCountRed -= value;
            coinText.text = "Coin:" + "" + coinCountRed.ToString();
        }
        else
        {
            coinCountBlue -= value;
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
            currentTurnText.text = "Player1";
        }
        else
        {
            currentTurnText.text = "Player2";
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



    }

    public void CheckGameWin()
    {
        //if one of the player point reach 10,the player can win
    }

}



