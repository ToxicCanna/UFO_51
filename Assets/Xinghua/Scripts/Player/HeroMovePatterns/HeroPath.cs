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
        Debug.Log(this.name + " side after wait: " + heroData.side);
    }
    private void SetHeroPathID()
    {

        string firstTwoLetters = this.name.Substring(0, 2);
        
        if (this.name.Contains("Basic")) 
        {
            heroPathID = 0;
        }
        else if (this.name.Contains("Knight"))
        {
            heroPathID = 1;
        }
        else if (this.name.Contains("Thief"))
        {
            heroPathID = 2;
        }
        else if (this.name.Contains("Range"))
        {
            heroPathID = 3;
        }
        else if (this.name.Contains("Healer"))
        {
            heroPathID = 4;
        }
        else
        {
            heroPathID = -1; 
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
            case string n when n.Contains("Ra"):
                heroPathID = 3;
                heroType = "Ra";
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
