using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroPocketManager : MonoBehaviour
{
    public static HeroPocketManager Instance;
    //this is hero in the scene
    public Dictionary<string, GameObject> redSideHeroes = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> blueSideHeroes = new Dictionary<string, GameObject>();

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
            var key = hero.name;
            
            var heroPos = new Vector2(hero.transform.position.x, hero.transform.position.y);
            GridManager.Instance.occupiedGridTeams.Add(heroPos, "red");
            RegisterHero(key, hero, "red");
            GridManager.Instance.AddOccupiedGrid(hero.transform.position);
        }

        //GetHeroData("hero01");
        BlueSideheros = GetComponent<TwoSidesHero>().GetHerosBlue();
        foreach (var hero in BlueSideheros)
        {
            var heroData = hero.GetComponent<HeroData>();
            var key = hero.name;
            //GridManager.Instance.AddOccupiedGrid(hero.transform.position);
            var heroPos = new Vector2(hero.transform.position.x, hero.transform.position.y);
            GridManager.Instance.occupiedGridTeams.Add(heroPos, "blue");
           
            RegisterHero(key, hero, "blue");
            GridManager.Instance.AddOccupiedGrid(hero.transform.position);
        }

        GetAllHeroes();

    }

 
    public List<GameObject> GetAllRedSideHeroes()
    {
        List<GameObject> heroListRedSide = new List<GameObject>(); // Create a new list

        foreach (var hero in redSideHeroes)
        {
            string heroName = hero.Key;
            GameObject heroObject = hero.Value;
            heroListRedSide.Add(heroObject); // Add to list
        }

        return heroListRedSide; // Return the list
    }
    public List<GameObject> GetAllBlueSideHeroes()
    {
        List<GameObject> heroListBlueSide = new List<GameObject>(); // Create a new list
        foreach (var hero in blueSideHeroes)
        {
            string heroName = hero.Key;
            GameObject heroObject = hero.Value;

            // Debug.Log($"Hero Name: {heroName}, Hero Object: {heroObject.name}");

            heroListBlueSide.Add(heroObject); // Add to list
        }


        return heroListBlueSide; // Return the list
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
            Vector2Int heroPos =new Vector2Int((int)hero.transform.position.x, (int)hero.transform.position.y);

            if (heroPos == position)
            {
                return hero;
            }
        }
        return null;
    }

/*    public string GetHeroTeamByName(string heroName)
    {
        if (redSideHeroes.ContainsKey(heroName))
        {
            return "red";
        }
        else if (blueSideHeroes.ContainsKey(heroName))
        {
            return "blue";
        }
        return "none"; 
    }*/

    // if player buy hero in the shop should also register and update this data
    public void RegisterHero(string heroId, GameObject hero, string heroColor)
    {
        if (heroColor == "red")
        {
            if (!redSideHeroes.ContainsKey(heroId))
            {
                redSideHeroes.Add(heroId, hero);
            }
           
        }
        else
        {
            if (!blueSideHeroes.ContainsKey(heroId))
            {
                blueSideHeroes.Add(heroId, hero);

            }
        }
    }

/*    public void GetHeroData(string heroId)
    {
        var heroComponent = redSideHeroes[heroId].GetComponent<HeroData>();
        if (heroComponent != null)
        {
            int heroHealth = heroComponent.heroData.cost;
        }
    }*/

    public void RemoveHero(GameObject hero)
    {
        string heroName = hero.name.Replace("(Clone)", "").Trim(); 

        if (redSideHeroes.ContainsKey(heroName))
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

     
        Vector2 heroPos = new Vector2(hero.transform.position.x, hero.transform.position.y);
        if (GridManager.Instance.occupiedGridTeams.ContainsKey(heroPos))
        {
            GridManager.Instance.occupiedGridTeams.Remove(heroPos);
            Debug.Log($"Removed {heroName} from occupied grid at {heroPos}");
        }
    }

}
