using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    private Dice dice;
    private int dTotal;
    public int tempRoll;
    public int[] rolls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        dice = new Dice();
    }
    public int RollTotal(int amount, int size)
    {
        //clear Data
        rolls = new int[amount];
        dTotal = 0;

        //roll each dice
        for (int i = 0; i < amount; i++)
        {
            rolls[i] = dice.Roll(size);  // Store each roll result in the array
            dTotal += rolls[i];

            AudioManager.Instance.Play("RollDice");
        }
        return dTotal;
    }
}
