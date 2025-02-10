using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class HeroData : MonoBehaviour
{
    public HeroSO heroData;

    public Sprite mySprite;

    public int maxHealth, currentHealth, cost, atk, atkSize, def, defSize, heal, healSize, moveSpeed, range, ability;
    private string abilityScore;
    //xinghua add
    public string side;
    //add end
    private TwoSidesHero twoSidesHero;


    void Start()
    {
        twoSidesHero = FindFirstObjectByType<TwoSidesHero>();
        SetStats();
    }

    void SetStats()
    {
        mySprite = heroData.sprite;
        maxHealth = heroData.health;
        currentHealth = maxHealth;
        cost = heroData.cost;
        atk = heroData.atk;
        atkSize = heroData.atkSize;
        def = heroData.def;
        defSize = heroData.defSize;           
        heal = heroData.heal;
        healSize = heroData.healSize;
        moveSpeed = heroData.moveSpeed;            
        range = heroData.range;
        ability = heroData.ability;

        abilityScore = heroData.abilityScore;

        //xinghua add
        side = heroData.side;
        //end
    }

    public void UpdateHealth(int amount)
    {
        currentHealth -= Mathf.Clamp(amount, 0, maxHealth);
        if (currentHealth <= 0)
        {
            //die
           // RemoveFromHeroList();
            //Xinghua add here befor you destroy need update data
            HeroPocketManager.Instance.RemoveHero(heroData.side, gameObject);
            GameManager.Instance.AddbattleBonus(this.gameObject,heroData.cost);//this for the battle kill point add
            var intPostiont = new Vector2Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);
            GridManager.Instance.RemoveOccupiedGrid(intPostiont, gameObject,heroData.side);
            Debug.Log("destroy:"+gameObject.name);
            //xinghua code end
            Destroy(gameObject);
        }
    }
/*    private void RemoveFromHeroList()
    {
        // Determine which side the hero belongs to (assuming red or blue side)
        if (twoSidesHero != null)
        {
            // Check the hero's affiliation, red or blue
            if (this.CompareTag("RedHero"))
            {
                twoSidesHero.GetHerosRed().Remove(gameObject);
            }
            else if (this.CompareTag("BlueHero"))
            {
                twoSidesHero.GetHerosBlue().Remove(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("TwoSidesHero reference not found.");
        }
    }*/

    /*public void UpgradeUnit()
    {
        if (heroData.Upgrade != null)
        {
            heroData = heroData.Upgrade;
            SetStats();
        }
    }*/
    public void PowerUp()
    {
        switch (abilityScore.Trim().ToLower())
        {
            case "atk":
                atk += ability;
                break;
            case "def":
                def += ability;

                break;
            case "heal":
                heal += ability;

                break;
            case "move":
                moveSpeed += ability;

                break;
            case "range":
                range += ability;

                break;
        }
    }
    public void PowerDown()
    {
        switch (abilityScore.Trim().ToLower())
        {
            case "atk":
                atk -= ability;
                break;
            case "def":
                def -= ability;

                break;
            case "heal":
                heal -= ability;

                break;
            case "move":
                moveSpeed -= ability;

                break;
            case "range":
                range -= ability;

                break;
        }
    }
}
