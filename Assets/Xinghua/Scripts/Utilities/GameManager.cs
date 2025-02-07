using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public bool IsHeroSubmitted { get; private set; }
    public int coinCount;
    public TMP_Text coinText;

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
        // coinCount =3; 
        UpdateCoinCount(3);
    }
    public void UpdateHeroSubmissionState(bool state)
    {
        IsHeroSubmitted = state;
    }
    public void UpdateCoinCount(int value)
    {
        coinCount += value;
        coinText.text = coinCount.ToString();   
    }

}



