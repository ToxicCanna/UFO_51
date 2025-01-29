using UnityEngine;

public class CastleAttack : MonoBehaviour
{
    [SerializeField] private DiceRoller DiceRoller;
    private HeroData heroData;

    private void Awake()
    {
        heroData = GetComponent<HeroData>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        var castle = other.GetComponent<Castle>();
        if (castle != null)
        {
            castle.TakeDamage(DiceRoller.RollTotal(heroData.atk, heroData.atkSize));
        }
        Debug.Log("attacked castle");
    }
}
