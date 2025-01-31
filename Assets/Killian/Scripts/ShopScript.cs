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

    private TwoSidesHero twoSidesHero;

    private void Start()
    {
        twoSidesHero = FindFirstObjectByType<TwoSidesHero>();
    }

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
        Debug.Log("Spawn");

        GameObject spawnedHero = null;

        if (gridIndicator.currentTurn == GridIndicator.PlayerTurn.PlayerRedSide)
        {
            spawnedHero = Instantiate(shopList[i], spawnLoc.position, Quaternion.identity);
            twoSidesHero.GetHerosRed().Add(spawnedHero);
        }
        else if (gridIndicator.currentTurn == GridIndicator.PlayerTurn.PlayerBlueSide)
        {
            spawnedHero = Instantiate(shopList[i], spawnLoc.position, Quaternion.identity);
            twoSidesHero.GetHerosBlue().Add(spawnedHero);
        }
    }
}
