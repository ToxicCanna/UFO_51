using UnityEngine;

public class HeroPath : MonoBehaviour
{
    public int heroPathID { get; private set; }

    private void Start()
    {

        SetHeroPathID();

        Debug.Log("move id " + heroPathID);
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
        else if (this.name.Contains("Ra"))
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

}
