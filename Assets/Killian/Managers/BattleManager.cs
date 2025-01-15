using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private DiceRoller diceroller;
    public HeroSO Player1Hero;
    public HeroSO Player2Hero;
    int damage;
    private void Update()
    {
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
        }

        Debug.Log($"Rolled {atkValue}atk against {defValue}def for {damage} Damage");
    }
}
