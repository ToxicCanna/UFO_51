using UnityEngine;

public class CastleAttack : MonoBehaviour
{
    private DiceRoller DiceRoller;
    private HeroData heroData;
    private TwoSidesHero twoSidesHero;

    private void Awake()
    {
        heroData = GetComponent<HeroData>();
        DiceRoller = FindFirstObjectByType<DiceRoller>();
        twoSidesHero = FindFirstObjectByType<TwoSidesHero>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        var castle = other.GetComponent<Castle>();
        if (castle != null)
        {
            int damage = DiceRoller.RollTotal(heroData.atk, heroData.atkSize);
            castle.TakeDamage(damage);
        Debug.Log($"attacked castle for {damage}");
            Destroy(gameObject);
            RemoveFromHeroList();
        }
    }
    private void RemoveFromHeroList()
    {
        // Determine which side the hero belongs to (assuming red or blue side)
        if (twoSidesHero != null)
        {
            // Check the hero's affiliation, red or blue
            if (this.CompareTag("RedHero"))
            {
                twoSidesHero.GetHerosRed().Remove(gameObject);
            }
            else if (this.CompareTag("BlueHero"))
            {
                twoSidesHero.GetHerosBlue().Remove(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("TwoSidesHero reference not found.");
        }
    }
}
