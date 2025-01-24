using System.Xml.Serialization;
using UnityEditor.Playables;
using UnityEngine;

public class HeroData : MonoBehaviour
{
    public HeroSO heroData;

    public int maxHealth, currentHealth, cost, atk, atkSize, def, defSize, heal, healSize, moveSpeed, range, ability;
    private string abilityScore;


    void Start()
    {
        SetStats();
    }

    void SetStats()
    {
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
            Destroy(gameObject);
        }
    }

    /*public void UpgradeUnit()
    {
        if (heroData.Upgrade != null)
        {
            heroData = heroData.Upgrade;
            SetHealth();
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
