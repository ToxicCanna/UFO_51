using System.Xml.Serialization;
using UnityEngine;

public class HeroData : MonoBehaviour
{
    public HeroSO heroData;

    int maxHealth, currentHealth;

    void Start()
    {
        SetHealth();
    }

    void SetHealth()
    {
        maxHealth = heroData.health;
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            //die
            Destroy(gameObject);
        }
    }

    public void UpgradeUnit()
    {
        if (heroData.Upgrade != null)
        {
            heroData = heroData.Upgrade;
            SetHealth();
        }
    }
}
