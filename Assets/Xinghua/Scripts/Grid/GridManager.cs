using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }



    private List<Vector2> occupiedGrids = new List<Vector2>();
    private List<GameObject> heros = new List<GameObject>();//this is for store the heros in the scene
    public Dictionary<Vector2, string> occupiedGridTeams = new Dictionary<Vector2, string>();

    private Vector2 indicatorPos;

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

    public void AddHeroWithTeamInfo(GameObject hero)
    {
        heros.Add(hero);
        Vector2 heroPosition = hero.transform.position;
        string heroTeam = HeroPocketManager.Instance.GetHeroTeamByName(hero.name); 
        occupiedGrids.Add(heroPosition);
        occupiedGridTeams[heroPosition] = heroTeam; 
        Debug.Log($"Hero {hero.name} added at {heroPosition}, Team: {heroTeam}");
    }



    public void RemoveOccupiedGrid(Vector2 gridPosition)
    {
        occupiedGrids.Remove(gridPosition);
        occupiedGridTeams.Remove(gridPosition); 
        Debug.Log($"Removed occupation at {gridPosition}");
    }

    public void AddOccupiedGrid(Vector2 gridPosition)
    {
        // Debug.Log("add :"+gridPosition);
        occupiedGrids.Add(gridPosition);
    }
    public string GetHeroTeamAtPosition(Vector2 position)
    {
        if (occupiedGridTeams.TryGetValue(position, out string team))
        {
           
            return team;
        }
        return "none"; 
    }



    public void CheckOccupiedHero(Vector2 position)
    {

        indicatorPos = position;
        foreach (var indicatorPos in occupiedGrids)
        {
            if (!indicatorPos.Equals(position))
            {
                Debug.Log("hero here");
            }
        }

    }

    public void AddHero(GameObject hero)
    {
        heros.Add(hero);
    }

    public List<GameObject> GetHeros()
    {
        return heros;
    }

    public List<Vector2> GetOccupiedGrids()
    {
        // print all the position been occupied by heros
        Debug.Log("occupiedGrids " + occupiedGrids.Count);
        return occupiedGrids;
    }



    public bool IsGridOccupied(Vector2 gridPosition)
    {
        return occupiedGrids.Contains(gridPosition);
    }
}
