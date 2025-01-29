using System;
using UnityEngine;

public class VisualRollGen : MonoBehaviour
{
    [SerializeField] private Sprite[] sides;

    private Sprite sprite;
    private int side;
    public void ShowRoll(int result)
    {
        if (result <= sides.Length)
        {
            side = UnityEngine.Random.Range(0, sides.Length);

            sprite = sides[side];
        }

    }
}
