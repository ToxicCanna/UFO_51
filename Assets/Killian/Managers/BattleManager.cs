using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceRoller diceroller;
    [SerializeField] private VisualRollGen gen;
    public HeroData currentHero;
    public HeroData targetHero;
    int damage;
    int clashDamage;
    bool poweredUp;
    bool ability = false;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    Attack();
        //}
    }
    //xinghua code start
    private void OnEnable()
    {
        Debug.Log("BattleManager Enabled");

        var healManager =FindAnyObjectByType<GridIndicator>();
        if (healManager != null)
        {
            healManager.healHero += Heal;
        }
        else
        {
            Debug.LogWarning("CheckHealRange not found in scene!");
        }
    }

    private void OnDisable()
    {
        Debug.Log("BattleManager Disabled");

        var healManager = FindAnyObjectByType<GridIndicator>();
        if (healManager != null)
        {
            healManager.healHero -= Heal;
        }
    }


    public void Heal(HeroData target,HeroData current)
    {
       
        if (target == null)
        {
            Debug.LogError("Heal called with null target!");
            return;
        }
        Debug.Log("Heal currentHero" + current);
        Debug.Log("Heal target" + target);
        currentHero = current;
        //xinghua code end
        int healValue = diceroller.RollTotal(currentHero.heal, currentHero.healSize);
        Debug.Log("healValue" + healValue);
        target.currentHealth += healValue;
        targetHero.UpdateHealth(target.currentHealth);//xinghua add this
    }

    public void Attack()
    {
        if(currentHero == null || targetHero == null)
        {
            if (currentHero == null)
            {
                Debug.Log("currenthero is null");
            }
            if (targetHero == null)
            {
                Debug.Log("targetHero is null");
            }
            return;
        }

        //targetHero = target;
        if (ability == true)
        {
            currentHero.PowerUp();
            Debug.Log($"{currentHero.atk}");
            poweredUp = true;
        }
        Debug.Log($"current {currentHero.name}");
        Debug.Log($"target {targetHero.name}");


        int atkValue = diceroller.RollTotal(currentHero.atk, currentHero.atkSize);
        int defValue = diceroller.RollTotal(targetHero.def, targetHero.defSize);

        //gen.ShowRoll(atkValue);

        if (atkValue > defValue)
        {
            damage = atkValue - defValue;
            targetHero.UpdateHealth(damage);
            Debug.Log($"Rolled {atkValue}atk against {defValue}def for {damage} Damage");
        }
        else if (atkValue < defValue || (atkValue == defValue && currentHero.range > 1))
        {
            damage = 0;
            //block attack
            Debug.Log($"Rolled {atkValue}atk against {defValue}def, they blocked!");
        }
        else
        {
            damage = 0;
            //(atkValue = defValue)! Clash!
            Clash(targetHero);
            Debug.Log($"Rolled {atkValue}atk against {defValue}def to Clash!!!");

        }

        if (poweredUp == true)
        {
            currentHero.PowerDown();
            Debug.Log($"{currentHero.atk}");
            poweredUp = false;
        }
        //start next turn
    }
    void Clash(HeroData target)
    {
        int clashAtkValue = diceroller.RollTotal(1, 6);
        int clashDefValue = diceroller.RollTotal(1, 6);

        if (clashAtkValue > clashDefValue)
        {
            damage = clashAtkValue;
            //pure damage!!
            target.UpdateHealth(damage);
            Debug.Log($"Rolled {clashAtkValue} Clash atk against {clashDefValue} Clash def for {damage} pure damage");
        }
        else if (clashAtkValue < clashDefValue)
        {
            clashDamage = clashDefValue - clashAtkValue;
            //counter attack
            currentHero.UpdateHealth(clashDamage);
            Debug.Log($"Rolled {clashAtkValue} Clash atk against {clashDefValue} Clash def for {clashDamage} counter damage");
        }
        else
        {
            damage = 0;
            //(clashAtkValue = clashDefValue)! Clash!
            Clash(target);
            Debug.Log($"Rolled {clashAtkValue} Clash atk against {clashDefValue} Clash def to clash again!!!");

        }
    }
}
