using UnityEngine;

public class BlueCastle : Castle
{
    public override void LoseGame()
    {
        //Red Wins
        tMPro.text = "Red Wins!";
        tMPro.gameObject.SetActive(true);
    }
}
