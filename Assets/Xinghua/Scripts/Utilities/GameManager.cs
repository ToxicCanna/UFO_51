using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

    public TMP_Text battleBonusText;
    public int battleBonus =0;
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

        battleBonusText.text = "Battle Bonus:" + battleBonus.ToString()+ "/10";
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
        coinText.text = "Coin:" +""+ coinCount.ToString();   
    }
    public void DecreaseCoinCount( int value)
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
        if(currentTurn== PlayerTurn.PlayerRedSide)
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
            return HeroPocketManager.Instance.redSideHeroes.Count;
        }
        else
        {
            return HeroPocketManager.Instance.blueSideHeroes.Count;
        }

    }

    public void HeroOwned()
    { 
        owenedHeroText.text = "Own Hero:" + GetHeroCount() ;
    }
    public void AddbattleBonus(int value)
    {
        Debug.Log("update the battle bonus text");
        battleBonus += value;
        battleBonusText.text = "Battle Bonus:" + battleBonus.ToString();
    }
}



