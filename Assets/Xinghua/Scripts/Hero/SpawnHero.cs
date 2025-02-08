using System.Linq;
using UnityEngine;

public class SpawnHero : MonoBehaviour
{

    [SerializeField] public Transform redSpawn, redCastle, blueSpawn, blueCastle;
    [SerializeField] GridIndicator gridIndicator;
    [SerializeField] GameObject[] redHeroPrefabs;
    [SerializeField] GameObject[] blueHeroPrefabs;

    private GameObject SpawnPrefab;
    private Transform spawnLoc;


    private void Update()
    {
        SpawnLocationSetup();
    }


    public void SpawnLocationSetup()
    {
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            spawnLoc = redSpawn;
        }

        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide)
        {
            spawnLoc = blueSpawn;
        }
    }

    private void SetSpawnPrefab(string buttonName)
    {
        GameObject[] heroArray;
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            heroArray = redHeroPrefabs;
        }
        else
        {
            heroArray = blueHeroPrefabs;
        }
        Debug.Log("hero length" + heroArray.Length);
        foreach (GameObject hero in heroArray)
        {
            if (hero.name.Contains(buttonName))
            {

                SpawnPrefab = hero;
            }
        }

    }

    public void SpawnNew(string buttonName)
    {
        SetSpawnPrefab(buttonName);
      
        GameObject spawnedHero = Instantiate(SpawnPrefab,spawnLoc.position, Quaternion.identity);
     
        HeroPocketManager.Instance.RegisterHero(spawnedHero.name, spawnedHero, "red");
        GameManager.Instance.DecreaseCoinCount(2);
    }
}

