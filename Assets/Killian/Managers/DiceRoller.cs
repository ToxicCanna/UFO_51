using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    private Dice dice;
    private int dTotal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        dice = new Dice();
    }
    public int RollTotal(int amount, int size)
    {
        //clear Data
        dTotal = 0;

        //roll each dice
        for (int i = amount; i > 0; i--)
        {
            dTotal += dice.Roll(size);
        }
        return dTotal;
    }
}
