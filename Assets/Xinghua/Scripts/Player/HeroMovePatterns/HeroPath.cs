using UnityEngine;

public class HeroPath : MonoBehaviour
{
    [SerializeField] HeroMovementRule heroMoveRule;
    [SerializeField] public int heroPathID;
    public int GetHeroMoveIndex()
    {
        return heroPathID;
    }
  
}
