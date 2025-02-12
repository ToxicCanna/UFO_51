using System.Collections;
using UnityEngine;

public class HeroPath : MonoBehaviour
{
    public int heroPathID { get; private set; }
    HeroData heroData;
    public string heroType { get; private set; }
    private void Awake()
    {
        heroData = GetComponent<HeroData>();
       
    }
    private void Start()
    {

        SetHeroAttributes();
        heroData = GetComponent<HeroData>();
        StartCoroutine(WaitForSide());  
    }

    private IEnumerator WaitForSide()
    {
        while (string.IsNullOrEmpty(heroData.side))
        {
            yield return null; 
        }
    }
   
    private void SetHeroAttributes()
    {
        switch (name)
        {
            case string n when n.Contains("Basic"):
                heroPathID = 0;
                heroType = "Basic";
                break;
            case string n when n.Contains("Knight"):
                heroPathID = 1;
                heroType = "Knight";
                break;
            case string n when n.Contains("Thief"):
                heroPathID = 2;
                heroType = "Thief";
                break;
            case string n when n.Contains("Range"):
                heroPathID = 3;
                heroType = "Range";
                break;
            case string n when n.Contains("Healer"):
                heroPathID = 4;
                heroType = "Healer";
                break;
            default:
                heroPathID = -1;
                heroType = "";
                break;
        }
    }

}
