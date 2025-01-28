using System.Collections.Generic;
using UnityEngine;

public class HeroPocketManager : MonoBehaviour
{
    //this is hero in the scene
    private Dictionary<string, GameObject> redSideHeroes = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> blueSideHeroes = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject[] heros;
    private void Start()
    {
        RegisterHero("hero01", heros[0]);

        GetHeroData("hero01");
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
            int heroHealth =heroComponent.heroData.cost;
           // Debug.Log("health" + heroHealth);
        }
       // Debug.Log("heroData" + heroComponent);
    }

    public void RemoveHighlight(string heroId)
    {
        if (redSideHeroes.TryGetValue(heroId, out GameObject hero))
        {
            Debug.Log("hide highlight path");
        }
    }
}
