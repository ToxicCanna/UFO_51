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
    private int coinCountRed =3;
    private int coinCountBlue = 3;
    public TMP_Text coinText;
    public TMP_Text currentTurnText;
    public TMP_Text BasicHeroText;
    public int BasicHeroCount = 0;
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
    }

    private void Start()
    {
        coinCountRed = 4;
        coinCountBlue = 3;
        coinText.text = coinCountRed.ToString();
        UpdateCoinCount();
        BasicHeroOwned();
    }

    public void UpdateHeroSubmissionState(bool state)
    {
        IsHeroSubmitted = state;
    }

    public void UpdateCoinCount()
    {
        var coinCount = coinCountRed;
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
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
        var coinCount = coinCountRed;
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            coinCount -= value;
        }
        else
        {
            coinCount = coinCountBlue;
            coinCountBlue++;
        }
        coinText.text = "Coin:" + "" + coinCount.ToString();
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

    public void BasicHeroOwned()
    {
        BasicHeroText.text = "Basic (" + BasicHeroCount.ToString() +"/4)";
    }
}



