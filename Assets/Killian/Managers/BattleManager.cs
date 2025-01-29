using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceRoller diceroller;
    public HeroData currentHero;
    private HeroData targetHero;
    int damage;
    int clashDamage;
    bool poweredUp;
    bool ability = false;

    public void Heal(HeroData target)
    {
        int healValue = diceroller.RollTotal(currentHero.heal, currentHero.healSize);

        target.currentHealth += healValue;
    }

    public void Attack(HeroData target)
    {
        targetHero = target;
        if (ability == true)
        {
            currentHero.PowerUp();
            Debug.Log($"{currentHero.atk}");
            poweredUp = true;
        }

        int atkValue = diceroller.RollTotal(currentHero.atk, currentHero.atkSize);
        int defValue = diceroller.RollTotal(targetHero.def, targetHero.defSize);

        if (atkValue > defValue)
        {
            damage = atkValue - defValue;
            target.currentHealth -= damage;
        }
        else if (atkValue < defValue || (atkValue == defValue && currentHero.range > 1))
        {
            damage = 0;
            //block attack
        }
        else
        {
            damage = 0;
            //(atkValue = defValue)! Clash!
            Clash(target);
        }

        Debug.Log($"Rolled {atkValue}atk against {defValue}def for {damage} Damage");

        if (poweredUp == true)
        {
            currentHero.PowerDown();
            Debug.Log($"{currentHero.atk}");
            poweredUp = false;
        }
    }
    void Clash(HeroData target)
    {
        int clashAtkValue = diceroller.RollTotal(1, 6);
        int clashDefValue = diceroller.RollTotal(1, 6);

        if (clashAtkValue > clashDefValue)
        {
            damage = clashAtkValue;
            //pure damage!!
            target.currentHealth += damage;
        }
        else if (clashAtkValue < clashDefValue)
        {
            clashDamage = clashDefValue - clashAtkValue;
            //counter attack
            currentHero.currentHealth -= clashDamage;
        }
        else
        {
            damage = 0;
            //(clashAtkValue = clashDefValue)! Clash!
            Clash(target);
        }
    }
}
