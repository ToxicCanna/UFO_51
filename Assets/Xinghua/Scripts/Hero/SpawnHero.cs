using System.Collections;
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
            spawnHeroColor = "Red";
        }
        else
        {
            heroArray = blueHeroPrefabs;
            spawnHeroColor = "Blue";
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

        GameObject spawnedHero = Instantiate(SpawnPrefab, spawnLoc.position, Quaternion.identity);
    
        HeroPocketManager.Instance.RegisterHero(spawnHeroColor, spawnedHero);
        var pos=new Vector2Int((int)spawnedHero.transform.position.x, (int)spawnedHero.transform.position.y);
        GridManager.Instance.AddHeroWithTeamInfo(pos, spawnedHero,spawnHeroColor);
        StartCoroutine(WaitForHeroData(spawnedHero));
    }

    private IEnumerator WaitForHeroData(GameObject hero)
    {
        yield return new WaitForEndOfFrame();

        var heroData = hero.GetComponent<HeroData>();
        GameManager.Instance.DecreaseCoinCount(heroData.cost);

    }
}

