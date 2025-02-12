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
        string colorPrefix = GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide ? "Red" : "Blue";

        Debug.Log("SetSpawnPrefab button name " + buttonName);
        GameObject[] heroArray;
        //spawnHeroColor = "";
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

        string expectedHeroName = colorPrefix + buttonName;
        foreach (GameObject hero in heroArray)
        {
         
           
            Debug.Log("hero name " + hero.name);
            Debug.Log("expectedHeroName " + expectedHeroName);
            if (hero.name==expectedHeroName)
             {

                 SpawnPrefab = hero;
                Debug.Log("SpawnNew SpawnPrefab " + SpawnPrefab.name);
            }
        }

    }

    public void SpawnNew(string buttonName,int cost)
    {
      
      
        SetSpawnPrefab(buttonName);
        GameObject spawnedHero = Instantiate(SpawnPrefab, spawnLoc.position, Quaternion.identity);




        HeroPocketManager.Instance.RegisterHero(spawnHeroColor, spawnedHero);
        var pos = new Vector2Int((int)spawnedHero.transform.position.x, (int)spawnedHero.transform.position.y);
        GridManager.Instance.AddHeroWithTeamInfo(pos, spawnedHero, spawnHeroColor);
        StartCoroutine(WaitForHeroData(spawnedHero,cost));
    }

    private IEnumerator WaitForHeroData(GameObject hero,int cost)
    {
        yield return new WaitForEndOfFrame();

        var heroData = hero.GetComponent<HeroData>();
        GameManager.Instance.DecreaseCoinCount(cost);
        //here need update the shop UI display
       // UINavManager.Instance.UpdateShopButtons();
      

    }
}

