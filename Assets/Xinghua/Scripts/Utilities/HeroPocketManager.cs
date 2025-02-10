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
        { "Red", new List<GameObject>() },
        { "Blue", new List<GameObject>() }
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
        HeroData heroData;
        foreach (var hero in RedSideheros)
        {
            heroData = hero.GetComponent<HeroData>();
            var heroPos = new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);
            GridManager.Instance.AddHeroWithTeamInfo(heroPos, hero, heroData.side);


            RegisterHero(heroData.side, hero);
           
        }
        BlueSideheros = GetComponent<TwoSidesHero>().GetHerosBlue();
        foreach (var hero in BlueSideheros)
        {
            heroData = hero.GetComponent<HeroData>();
            var heroPos = new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);
            GridManager.Instance.AddHeroWithTeamInfo(heroPos, hero, heroData.side);

            RegisterHero(heroData.side, hero);


        }
        GetAllBlueSideHeroes();
        GetAllRedSideHeroes();
        Debug.Log("redSideHeroes count" + redSideHeroes.Count);
        Debug.Log("blueSideHeroes count" + blueSideHeroes.Count);
        
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
        redSideHeroes = heroTeams["Red"];
        return redSideHeroes;
      //  return heroTeams["red"];
    }
    public List<GameObject> GetAllBlueSideHeroes()
    {
        blueSideHeroes = heroTeams["Blue"];
        return blueSideHeroes;
       //return heroTeams["Blue"];

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
    public void RegisterHero(string side, GameObject hero)
    {
        if (heroTeams.ContainsKey(side))
        {
            heroTeams[side].Add(hero);
        }
    }
    //use this to remove
    public void RemoveHero(string side, GameObject hero)
    {
        if (heroTeams.ContainsKey(side))
        {
            if (heroTeams[side].Contains(hero)) 
            {
                heroTeams[side].Remove(hero);
                Debug.Log($"Pocket Removed {hero.name} from {side} team.");
            }
            else
            {
                Debug.LogWarning($"Pocket Hero {hero.name} not found in {side} team.");
            }
        }
        else
        {
            Debug.LogWarning($"Team {side} does not exist.");
        }
    }

}
