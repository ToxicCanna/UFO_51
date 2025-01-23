using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceRoller diceroller;
    public HeroSO Player1Hero;
    public HeroSO Player2Hero;
    int damage;
    int clashDamage;
    bool poweredUp;
    bool ability = false;
    private void Update()
    {
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
        }
        else if (atkValue < defValue)
        {
            damage = 0;
            //block attack
        }
        else
        {
            damage = 0;
            //(atkValue = defValue)! Clash!
            Clash();
        }

        Debug.Log($"Rolled {atkValue}atk against {defValue}def for {damage} Damage");

        if (poweredUp == true)
        {
            Player1Hero.PowerDown();
            Debug.Log($"{Player1Hero.atk}");
            poweredUp = false;
        }
    }
    void Clash()
    {
        int clashAtkValue = diceroller.RollTotal(1, 6);
        int clashDefValue = diceroller.RollTotal(1, 6);

        if (clashAtkValue > clashDefValue)
        {
            damage = clashAtkValue;
            //pure damage!!
        }
        else if (clashAtkValue < clashDefValue)
        {
            clashDamage = clashDefValue - clashAtkValue;
            //counter attack
        }
        else
        {
            damage = 0;
            //(clashAtkValue = clashDefValue)! Clash!
            Clash();
        }
    }
}
