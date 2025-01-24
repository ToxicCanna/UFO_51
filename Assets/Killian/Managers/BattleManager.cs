using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceRoller diceroller;
    public HeroData Player1Hero;
    private HeroData Player2Hero;
    int damage;
    int clashDamage;
    bool poweredUp;
    bool ability = false;

    public void Heal(HeroData target)
    {
        int healValue = diceroller.RollTotal(Player1Hero.heal, Player1Hero.healSize);

        target.currentHealth += healValue;
    }

    public void Attack(HeroData target)
    {
        Player2Hero = target;
        if (ability == true)
        {
            Player1Hero.PowerUp();
            Debug.Log($"{Player1Hero.atk}");
            poweredUp = true;
        }

        int atkValue = diceroller.RollTotal(Player1Hero.atk, Player1Hero.atkSize);
        int defValue = diceroller.RollTotal(Player2Hero.def, Player2Hero.defSize);

        if (atkValue > defValue)
        {
            damage = atkValue - defValue;
            target.currentHealth -= damage;
        }
        else if (atkValue < defValue || (atkValue == defValue && Player1Hero.range > 1))
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
            Player1Hero.PowerDown();
            Debug.Log($"{Player1Hero.atk}");
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
            Player1Hero.currentHealth -= clashDamage;
        }
        else
        {
            damage = 0;
            //(clashAtkValue = clashDefValue)! Clash!
            Clash(target);
        }
    }
}
