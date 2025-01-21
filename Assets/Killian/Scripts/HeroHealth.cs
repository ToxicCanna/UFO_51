using System.Xml.Serialization;
using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    public HeroSO heroData;

    int maxHealth, currentHealth;

    void Start()
    {
        maxHealth = heroData.health;
        currentHealth = maxHealth;
    }
}
