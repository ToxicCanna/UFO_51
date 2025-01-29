using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroPocketManager : MonoBehaviour
{
    public static HeroPocketManager Instance;
    //this is hero in the scene
    private Dictionary<string, GameObject> redSideHeroes = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> blueSideHeroes = new Dictionary<string, GameObject>();

    //this two list for swith hero ;and prepare for action 
    //private List<GameObject> redSideHeroes = new List<GameObject>();
    //private List<GameObject> blueSideHeroes = new List<GameObject>();

    private List<GameObject> RedSideheros;
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
            RegisterHero(key, hero,"red");
        }
        GetAllRedSideHeroes();

        //GetHeroData("hero01");
        BlueSideheros = GetComponent<TwoSidesHero>().GetHerosBlue();
        foreach (var hero in BlueSideheros)
        {
            var heroData = hero.GetComponent<HeroData>();
            var key = hero.name;
            //GridManager.Instance.AddOccupiedGrid(hero.transform.position);
            GridManager.Instance.AddHero(hero);
            RegisterHero(key, hero, "blue");

        }
        GetAllBlueSideHeroes();

    }

    public List<GameObject> GetAllRedSideHeroes()
    {
        List<GameObject> heroListRedSide = new List<GameObject>(); // Create a new list

        foreach (var hero in redSideHeroes)
        {
            string heroName = hero.Key;
            GameObject heroObject = hero.Value;

            Debug.Log($"Hero Name: {heroName}, Hero Object: {heroObject.name}");

            heroListRedSide.Add(heroObject); // Add to list
        }


        return heroListRedSide; // Return the list
    }
    public List<GameObject> GetAllBlueSideHeroes()
    {
        List<GameObject> heroListBlueSide = new List<GameObject>(); // Create a new list
        Debug.Log("blueSideHeroes in GetAllBlueSideHeroes" + blueSideHeroes);

        foreach (var hero in blueSideHeroes)
        {
            string heroName = hero.Key;
            GameObject heroObject = hero.Value;

            Debug.Log($"Hero Name: {heroName}, Hero Object: {heroObject.name}");

            heroListBlueSide.Add(heroObject); // Add to list
        }


        return heroListBlueSide; // Return the list
    }

    public List<GameObject> GetAllHeroes()
    {
        List<GameObject> allHeroes = new List<GameObject>();
        allHeroes.AddRange(GetAllRedSideHeroes()); 
        allHeroes.AddRange(GetAllBlueSideHeroes()); 
        return allHeroes;
    }
    public void RegisterHero(string heroId, GameObject hero,string heroColor)
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
        
        Debug.Log("hero have already" + redSideHeroes.Count);
    }


    public void GetHeroData(string heroId)
    {
        var heroComponent = redSideHeroes[heroId].GetComponent<HeroData>();
        if (heroComponent != null)
        {
            int heroHealth = heroComponent.heroData.cost;
            // Debug.Log("health" + heroHealth);
        }
        // Debug.Log("heroData" + heroComponent);
    }

    /*   public void RemoveHighlight(string heroId)
       {
           if (redSideHeroes.TryGetValue(heroId, out GameObject hero))
           {
               Debug.Log("hide highlight path");
           }
       }*/
}
