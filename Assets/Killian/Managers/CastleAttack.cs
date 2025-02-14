using System;
using System.Collections;
using UnityEngine;

public class CastleAttack : MonoBehaviour
{
    private DiceRoller DiceRoller;
    private HeroData heroData;
    private TwoSidesHero twoSidesHero;

    //Xinghua
    private Animator anim;
    //end
    private void Awake()
    {
        heroData = GetComponent<HeroData>();
        DiceRoller = FindFirstObjectByType<DiceRoller>();
        twoSidesHero = FindFirstObjectByType<TwoSidesHero>();
    }
    //xinghua code：before check castle and destroy,  need check if hero from opposite there, if yes , attact hero first

    public void OnTriggerEnter2D(Collider2D other)
    {
        var castle = other.GetComponent<Castle>();
        if (castle != null)
        {
            int damage = DiceRoller.RollTotal(heroData.atk, heroData.atkSize);
            StartCoroutine(DestroyAfterDelay(0.5f, damage, castle));
            Debug.Log($"attacked castle for {damage}");
            anim = gameObject.GetComponent<Animator>();
        }
    }
    private IEnumerator DestroyAfterDelay(float delay,int damage, Castle castle)
    {
        yield return new WaitForSeconds(0.5f);
        if (anim != null)
        {
            anim.SetBool("IsRun", false);
            yield return new WaitForSeconds(0.1f);
            anim.SetBool("IsAtk", true);

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length+1f);
        }
        if (castle!= null)
        {
            castle.TakeDamage(damage);
            AudioManager.Instance.Play("AttackCastle");
            HeroPocketManager.Instance.RemoveHero(heroData.side,gameObject); //xinghua note :before destroy any game object remove first or will error
            Destroy(gameObject);
            GameManager.Instance.UpdatePlayerTurn();
        }
       
      
    }
    //code end

    //twoSidesHero is just manage data when start ,after that use HeroPocketManager
    /*  private void RemoveFromHeroList()
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
}
