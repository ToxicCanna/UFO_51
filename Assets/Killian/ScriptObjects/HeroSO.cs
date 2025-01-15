using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "ScriptableObjects/HeroSO", order = 1)]
public class HeroSO : ScriptableObject
{
    public int health;
    public int atk;
    public int atkSize;
    public int def;
    public int defSize;
    public int heal;
    public int healSize;
}
