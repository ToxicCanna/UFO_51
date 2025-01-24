using Unity.VisualScripting;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    [SerializeField] public Transform redSpawn, redCastle, blueSpawn, blueCastle;
    [SerializeField] GridIndicator gridIndicator;
    [SerializeField] GameObject[] redHeroPrefab;
    [SerializeField] GameObject[] blueHeroPrefab;
    private GameObject[] shopList;

    private Transform spawnLoc;

    private void Update()
    {
        ShopSetup(gridIndicator.currentTurn);
    }


    public void ShopSetup(GridIndicator.PlayerTurn gridIndicator)
    {
        if(gridIndicator == GridIndicator.PlayerTurn.PlayerRedSide)
        {
            spawnLoc = redSpawn;
            shopList = redHeroPrefab;
        }

        else if(gridIndicator == GridIndicator.PlayerTurn.PlayerBlueSide)
        {
            spawnLoc = blueSpawn;
            shopList = blueHeroPrefab;
        }
    }

    public void Spawn(int i)
    {
        if (gridIndicator.currentTurn == GridIndicator.PlayerTurn.PlayerRedSide)
        {
            Instantiate(shopList[i], spawnLoc);
        }
        if (gridIndicator.currentTurn == GridIndicator.PlayerTurn.PlayerBlueSide)
        {
            Instantiate(shopList[i], spawnLoc);
        }
    }
}
