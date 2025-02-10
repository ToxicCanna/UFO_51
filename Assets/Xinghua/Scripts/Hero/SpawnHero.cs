using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnHero : MonoBehaviour
{

    [SerializeField] public Transform redSpawn, redCastle, blueSpawn, blueCastle;
    [SerializeField] GridIndicator gridIndicator;
    [SerializeField] GameObject[] redHeroPrefabs;
    [SerializeField] GameObject[] blueHeroPrefabs;
    private string spawnHeroColor;

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
         spawnHeroColor = "";
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            heroArray = redHeroPrefabs;
            spawnHeroColor = "red";
        }
        else
        {
            heroArray = blueHeroPrefabs;
            spawnHeroColor = "blue";
        }

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
        HeroPocketManager.Instance.RegisterHero(spawnHeroColor,spawnedHero);
        StartCoroutine(WaitForHeroData(spawnedHero));
    }

    private IEnumerator WaitForHeroData(GameObject hero)
    {
        yield return new WaitForEndOfFrame();

        var heroData = hero.GetComponent<HeroData>();
        GameManager.Instance.DecreaseCoinCount(heroData.cost);
       
    }
}

