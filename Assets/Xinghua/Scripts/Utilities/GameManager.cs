using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;

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

    private void Update()
    {
        UpdatePlayerTurn();
        HeroOwned();

        battleBonusTextR.text = "Point:" + battleBonusRed.ToString() + "/10";
        battleBonusTextB.text = "Point:" + battleBonusBlue.ToString() + "/10";

    }

    private void Start()
    {
        coinCountRed = 4;
        coinCountBlue = 4;
        coinText.text = coinCountRed.ToString();
        AddCoinCount();
        HeroOwned();
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
    public void AddbattleBonus(GameObject destroyedObj, int value)
    {
        if (destroyedObj != null)
        {
          /*  Debug.Log("(obj.name" + destroyedObj.name);
            Debug.Log("(obj.bonus" + value);*/
            var color = HeroPocketManager.Instance.GetTeamByHeroObj(destroyedObj);
           // Debug.Log("(color%%%" + color);
            if (color == null) return;
            if (color == "red")
            {
                Debug.Log("add bonus to blue" + value);
                battleBonusRed += value;
                battleBonusTextB.text = "Point2:" + battleBonusBlue.ToString();
               
            }
            else if (color == "blue")
            {
                Debug.Log("add bonus to red" + value);
                battleBonusBlue += value;
                battleBonusTextR.text = "Point1:" + battleBonusRed.ToString();
            }
            

        }
    }

    public void CheckGameWin()
    {
        //if one of the player point reach 10,the player can win
    }

}



