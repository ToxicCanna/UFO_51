using System.Collections.Generic;
using UnityEngine;

public class HeroPocketManager : MonoBehaviour
{
    public static HeroPocketManager Instance;
    //this is hero in the scene
    private Dictionary<string, GameObject> redSideHeroes = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> blueSideHeroes = new Dictionary<string, GameObject>();

    //this two list for swith hero ;and prepare for action 
    //private List<GameObject> redSideHeroes = new List<GameObject>();
    //private List<GameObject> blueSideHeroes = new List<GameObject>();

    private List<GameObject> heros;
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
        heros = GetComponent<TwoSidesHero>().GetHeros();
        foreach (var hero in heros)
        {
            var heroData = hero.GetComponent<HeroData>();
            var key = hero.name;
            RegisterHero(key, hero);
        }


        //GetHeroData("hero01");
        foreach (var hero in heros)
        {
            //GridManager.Instance.AddOccupiedGrid(hero.transform.position);
            GridManager.Instance.AddHero(hero);

        }
        GetAllRedSideHeroes();
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

    public void RegisterHero(string heroId, GameObject hero)
    {
        if (!redSideHeroes.ContainsKey(heroId))
        {
            redSideHeroes.Add(heroId, hero);
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
