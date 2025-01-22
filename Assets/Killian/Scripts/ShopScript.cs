using Unity.VisualScripting;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField] public Transform redSpawn, redCastle, blueSpawn, blueCastle;
    [SerializeField] GridIndicator gridIndicator;
    [SerializeField] GameObject redheroprefab;
    [SerializeField] GameObject blueheroprefab;

    private Transform spawnLoc;
    private Transform shopLoc;

    private void Update()
    {
        ShopSetup(gridIndicator.currentTurn);
    }


    public void ShopSetup(GridIndicator.PlayerTurn gridIndicator)
    {
        if(gridIndicator == GridIndicator.PlayerTurn.PlayerRedSide)
        {
            spawnLoc = redSpawn;
            shopLoc = redCastle;
        }

        else if(gridIndicator == GridIndicator.PlayerTurn.PlayerBlueSide)
        {
            spawnLoc = blueSpawn;
            shopLoc = blueCastle;
        }
    }

    public void Spawn()
    {
        if (gridIndicator.currentTurn == GridIndicator.PlayerTurn.PlayerRedSide)
        {
            Instantiate(redheroprefab, spawnLoc);
        }
        if (gridIndicator.currentTurn == GridIndicator.PlayerTurn.PlayerBlueSide)
        {
            Instantiate(blueheroprefab, spawnLoc);
        }
    }
}
