using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjects/HeroSO", order = 1)]
public class HeroSO : ScriptableObject
{
    public Sprite sprite;
    public int health, cost, atk, atkSize, def, defSize, heal, healSize, moveSpeed, range, ability;
    public string abilityScore;
    //public HeroSO Upgrade;
}
