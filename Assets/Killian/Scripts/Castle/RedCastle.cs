using UnityEngine;

public class RedCastle : Castle
{
    public override void LoseGame()
    {
        //Blue Wins
        tMPro.text = "Blue Wins!";
        tMPro.gameObject.SetActive(true);
    }
}
