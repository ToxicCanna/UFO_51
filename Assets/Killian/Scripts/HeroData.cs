using System.Xml.Serialization;
using UnityEditor.Playables;
using UnityEngine;

public class HeroData : MonoBehaviour
{
    public HeroSO heroData;

    public Sprite mySprite;

    public int maxHealth, currentHealth, cost, atk, atkSize, def, defSize, heal, healSize, moveSpeed, range, ability;
    private string abilityScore;

    private TwoSidesHero twoSidesHero;


    void Start()
    {
        twoSidesHero = FindFirstObjectByType<TwoSidesHero>();
        SetStats();
    }

    void SetStats()
    {
        mySprite = heroData.sprite;
        maxHealth = heroData.health;
        currentHealth = maxHealth;
        cost = heroData.cost;
        atk = heroData.atk;
        atkSize = heroData.atkSize;
        def = heroData.def;
        defSize = heroData.defSize;           
        heal = heroData.heal;
        healSize = heroData.healSize;
        moveSpeed = heroData.moveSpeed;            
        range = heroData.range;
        ability = heroData.ability;

        abilityScore = heroData.abilityScore;
    }

    public void UpdateHealth(int amount)
    {
        currentHealth -= Mathf.Clamp(amount, 0, maxHealth);
        if (currentHealth <= 0)
        {
            //die
            RemoveFromHeroList();
            Destroy(gameObject);
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

    /*public void UpgradeUnit()
    {
        if (heroData.Upgrade != null)
        {
            heroData = heroData.Upgrade;
            SetStats();
        }
    }*/
    public void PowerUp()
    {
        switch (abilityScore.Trim().ToLower())
        {
            case "atk":
                atk += ability;
                break;
            case "def":
                def += ability;

                break;
            case "heal":
                heal += ability;

                break;
            case "move":
                moveSpeed += ability;

                break;
            case "range":
                range += ability;

                break;
        }
    }
    public void PowerDown()
    {
        switch (abilityScore.Trim().ToLower())
        {
            case "atk":
                atk -= ability;
                break;
            case "def":
                def -= ability;

                break;
            case "heal":
                heal -= ability;

                break;
            case "move":
                moveSpeed -= ability;

                break;
            case "range":
                range -= ability;

                break;
        }
    }
}
