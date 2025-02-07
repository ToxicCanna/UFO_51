using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class SpawnHero : MonoBehaviour
{

    [SerializeField] public Transform redSpawn, redCastle, blueSpawn, blueCastle;
    [SerializeField] GridIndicator gridIndicator;
    [SerializeField] GameObject[] redHeroPrefab;
    [SerializeField] GameObject[] blueHeroPrefab;
    [SerializeField] GameObject redBasicHeroPrefab;
    [SerializeField] GameObject redAHeroPrefab;
    private GameObject[] shopList;

    private Transform spawnLoc;



    private void Update()
    {
        ShopSetup();
    }


    public void ShopSetup()
    {
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            spawnLoc = redSpawn;
            shopList = redHeroPrefab;
        }

        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide)
        {
            spawnLoc = blueSpawn;
            shopList = blueHeroPrefab;
        }
    }

    /*    public void Spawn(int i)
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
        }*/
    public void SpawnNew(int i)
    {
        
        GameObject spawnedHero = null;
        if (i == 0)//basic
        {
            spawnedHero = Instantiate(redBasicHeroPrefab, spawnLoc.position, Quaternion.identity);
            Debug.Log("Spawn");
            HeroPocketManager.Instance.RegisterHero(spawnedHero.name, spawnedHero, "red");
            GameManager.Instance.DecreaseCoinCount(2);


        }
        if (i == 3)//knight
        {
            spawnedHero = Instantiate(redAHeroPrefab, spawnLoc.position, Quaternion.identity);

            HeroPocketManager.Instance.RegisterHero(spawnedHero.name, spawnedHero, "red");
        }
        //Debug.Log("Spawn + " + i);
    }
}

