using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjects/HeroSO", order = 1)]
public class HeroSO : ScriptableObject
{
    public int health, cost, atk, atkSize, def, defSize, heal, healSize, moveSpeed, range, ability;
    public string abilityScore;
    public HeroSO Upgrade;

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
