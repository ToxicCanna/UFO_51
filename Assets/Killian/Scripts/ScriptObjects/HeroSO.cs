using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjects/HeroSO", order = 1)]
public class HeroSO : ScriptableObject
{
    public Sprite sprite;
    public int health, cost, atk, atkSize, flatAtk, def, defSize, flatDef, heal, healSize, flatHeal, moveSpeed, range, ability;
    public string abilityScore;
    //public HeroSO Upgrade;
    public string side;//xinghua add
    public string heroType; // dustin add
}
