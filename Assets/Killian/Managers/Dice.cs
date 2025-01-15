using UnityEngine;

public class Dice
{
    public virtual int Roll(int sides)
    {
        return Random.Range(1, sides + 1);
    }
}
