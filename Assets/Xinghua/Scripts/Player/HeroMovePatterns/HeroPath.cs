using UnityEngine;

public class HeroPath : MonoBehaviour
{
   // public HeroPath heroPath;
   // [SerializeField] HeroMovementRule heroMoveRule;
    public int heroPathID;
  
    public int GetHeroMoveIndex()
    {
        return this.heroPathID;
    }
}
