using System.Collections.Generic;
using UnityEngine;

public class HeroPocketManager : MonoBehaviour
{
    //this is hero in the scene
    private Dictionary<string, GameObject> heroes = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject hero;
    private void Start()
    {
        RegisterHero("hero01", hero);
        GetHeroData("hero01");
    }
    public void RegisterHero(string heroId, GameObject hero)
    {
        if (!heroes.ContainsKey(heroId))
        {
            heroes.Add(heroId, hero);
        }

        Debug.Log("hero have already" + heroes.Count);
    }
    public void GetHeroData(string heroId)
    {
        var heroComponent = heroes[heroId].GetComponent<HeroData>();
        if (heroComponent != null)
        { 
            int heroHealth =heroComponent.heroData.cost;
           // Debug.Log("health" + heroHealth);
        }
       // Debug.Log("heroData" + heroComponent);
    }

    public void RemoveHighlight(string heroId)
    {
        if (heroes.TryGetValue(heroId, out GameObject hero))
        {
            Debug.Log("hide highlight path");
        }
    }
}
