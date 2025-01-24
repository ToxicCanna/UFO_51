using UnityEditor;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHP;
    public int castleHP;

    private void Start()
    {
        castleHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        castleHP -= damage;
        if (castleHP <= 0)
        {
            LoseGame();
        }
    }
    public virtual void LoseGame()
    {

    }
}
