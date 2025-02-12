using System;
using System.Collections;
using UnityEngine;

public class VisualRollGen : MonoBehaviour
{
    [SerializeField] private Sprite[] sides;
    public SpriteRenderer spriteRen;

    private Sprite sprite;
    private int randomSide;
    private IEnumerator coroutine;
    public void ShowRoll(int result)
    {
        for (int i = 0; i <= 6; i++)
        {
            if (i < 6)
            {
                coroutine = RollDelay((i + 1) * .25f);
                StartCoroutine(coroutine);
                Debug.Log($"{i} i is");
            }
            else if (i == 6)
            {
                coroutine = RollDelaySetsResult((i + 1) * 0.25f, result);
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator RollDelay(float tick)
    {
        yield return new WaitForSeconds(tick);
                randomSide = UnityEngine.Random.Range(0, sides.Length);
        sprite = sides[randomSide];
        spriteRen.sprite = sprite;
        Debug.Log($"{randomSide + 1} rolled visual");
    }

    private IEnumerator RollDelaySetsResult(float tick, int result)
    {
        yield return new WaitForSeconds(tick);
        sprite = sides[result-1];
        spriteRen.sprite = sprite;
        Debug.Log($"{result} rolled visual");
    }
}
