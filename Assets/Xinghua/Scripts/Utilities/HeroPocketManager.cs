using System.Collections.Generic;
using UnityEngine;

public class HeroPocketManager : MonoBehaviour
{
    public static HeroPocketManager Instance;
    //this is hero in the scene
    private List<GameObject> redSideHeroes;
    private List<GameObject> blueSideHeroes;
    private Dictionary<string, List<GameObject>> heroTeams = new Dictionary<string, List<GameObject>>()
    {
        { "red", new List<GameObject>() },
        { "blue", new List<GameObject>() }
    };


    private List<GameObject> RedSideheros;//get from child gameobject
    private List<GameObject> BlueSideheros;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RedSideheros = GetComponent<TwoSidesHero>().GetHerosRed();
        foreach (var hero in RedSideheros)
        {
            var heroData = hero.GetComponent<HeroData>();
            var heroPos = new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);

            GridManager.Instance.occupiedGridTeams.Add(heroPos, "red");

            RegisterHero("red", hero);
            GridManager.Instance.AddHeroWithTeamInfo(heroPos);
        }

        //GetHeroData("hero01");
        BlueSideheros = GetComponent<TwoSidesHero>().GetHerosBlue();
        foreach (var hero in BlueSideheros)
        {
            var heroData = hero.GetComponent<HeroData>();
            var key = hero.name;
            var heroPos = new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);
            GridManager.Instance.occupiedGridTeams.Add(heroPos, "blue");

            RegisterHero("blue", hero);
            GridManager.Instance.AddHeroWithTeamInfo(heroPos);

        }
        GetAllBlueSideHeroes();
        GetAllRedSideHeroes();
        Debug.Log("redSideHeroes count" + redSideHeroes.Count);
        Debug.Log("blueSideHeroes count" + blueSideHeroes.Count);
        GetAllHeroes();

    }
    public string GetTeamByHeroObj(GameObject obj)
    {
        foreach (var team in heroTeams)
        {
            foreach (var heroData in team.Value)
            {
                if (heroData.gameObject == obj)
                {
                    return team.Key; 
                }
            }
        }

        return "none"; 
    }

    public List<GameObject> GetAllRedSideHeroes()
    {
        redSideHeroes = heroTeams["red"];
        return redSideHeroes;
    }
    public List<GameObject> GetAllBlueSideHeroes()
    {
        blueSideHeroes = heroTeams["blue"];
        return blueSideHeroes;
      
    }

    public List<GameObject> GetAllHeroes()
    {
        List<GameObject> allHeroes = new List<GameObject>();

        foreach (var hero in GetAllBlueSideHeroes())
        {
            allHeroes.Add(hero);
        }
        foreach (var hero in GetAllRedSideHeroes())
        {
            allHeroes.Add(hero);
        }
        //Debug.Log("allHero num" + allHeroes.Count);
        return allHeroes;

    }

    public GameObject GetHeroByPosition(Vector2Int position)
    {
        foreach (var hero in GetAllHeroes())
        {
            Vector2Int heroPos = new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);

            if (heroPos == position)
            {
                return hero;
            }
        }
        return null;
    }

    public List<GameObject> GetOppositeHeros()
    {
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            return GetAllBlueSideHeroes();
        }
        else
        {
            return GetAllRedSideHeroes();
        }
    }

    public GameObject GetOppositeHerosByPosition(Vector2Int position)
    {

        foreach (var hero in GetOppositeHeros())
        {
            Vector2Int heroPos = new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);

            if (heroPos == position)
            {
                return hero;
            }
        }
        return null;
    }



    // if player buy hero in the shop should also register and update this data




    public void RegisterHero(string key, GameObject hero)
    {
        if (heroTeams.ContainsKey(key))
        {
            heroTeams[key].Add(hero);
        }
    }

    public void RemoveHero(GameObject hero)
    {
        string heroName = hero.name.Replace("(Clone)", "").Trim();

        /*if (redSideHeroes.ContainsKey(heroName))
        {
            redSideHeroes.Remove(heroName);
            Debug.Log($"Removed {heroName} from RedSideHeroes.");
        }
        else if (blueSideHeroes.ContainsKey(heroName))
        {
            blueSideHeroes.Remove(heroName);
            Debug.Log($"Removed {heroName} from BlueSideHeroes.");
        }
        else
        {
            Debug.LogWarning($"Hero {heroName} not found in any list.");
        }
*/

        Vector2 heroPos = new Vector2(hero.transform.position.x, hero.transform.position.y);
        if (GridManager.Instance.occupiedGridTeams.ContainsKey(heroPos))
        {
            GridManager.Instance.occupiedGridTeams.Remove(heroPos);
            Debug.Log($"Removed {heroName} from occupied grid at {heroPos}");
        }
    }

}
