using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceRoller diceroller;
    [SerializeField] private GameObject[] redDice;
    [SerializeField] private GameObject[] blueDice;
    public HeroData currentHero;
    public HeroData targetHero;
    int damage;
    int clashDamage;
    bool poweredUp;
    bool ability = false;
    private GameObject[] atkDice;
    private GameObject[] defDice;
    private void Update()
    {
        atkDice = (currentHero.side == "Red") ? redDice : blueDice;
        defDice = (currentHero.side == "Red") ? blueDice : redDice;
    }
    //xinghua code start
    private void OnEnable()
    {
        Debug.Log("BattleManager Enabled");

        var healManager = FindAnyObjectByType<GridIndicator>();
        if (healManager != null)
        {
            healManager.healHero -= Heal;
            healManager.healHero += Heal;
        }
        else
        {
            Debug.LogWarning("CheckHealRange not found in scene!");
        }
    }

    private void OnDisable()
    {
        var healManager = FindAnyObjectByType<GridIndicator>();
        if (healManager != null)
        {
            healManager.healHero -= Heal;
        }
    }


    public void Heal(HeroData target, HeroData current)
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
        // target.currentHealth += healValue;
        if (target.currentHealth + healValue > target.maxHealth)
        {
            target.currentHealth = target.maxHealth;
        }
        else
        {
            target.currentHealth = target.currentHealth + healValue;
        }

       // target.currentHealth = Mathf.Min(target.currentHealth + healValue, target.maxHealth);
        targetHero.UpdateHealthWithHealValue(target.currentHealth);//xinghua add this;
    }

    public void Attack()
    {
        if (currentHero == null || targetHero == null)
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
        RevealDice(atkDice, currentHero.atk);

        int defValue = diceroller.RollTotal(targetHero.def, targetHero.defSize);
        RevealDice(defDice, targetHero.def);

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
    }
    private void RevealDice(GameObject[] dice, int statValue)
    {
        StartCoroutine(HideDice(dice));
        for (int i = 0; i < dice.Length; i++)
        {
            if (i < statValue)
            {
                dice[i].SetActive(true);
                int rollResult = diceroller.rolls[i];
                dice[i].GetComponent<VisualRollGen>().ShowRoll(rollResult);
            }
            else
            {
                dice[i].SetActive(false);
            }
        }
    }
    private IEnumerator HideDice(GameObject[] dice)
    {
        yield return new WaitForSeconds(5f);
        foreach (var item in dice)
        {
            item.SetActive(false);
        }
    }
    void Clash(HeroData target)
    {
        int clashAtkValue = diceroller.RollTotal(1, 6);
        int clashDefValue = diceroller.RollTotal(1, 6);
        RevealDice(atkDice, currentHero.atk);
        RevealDice(defDice, targetHero.def);

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
