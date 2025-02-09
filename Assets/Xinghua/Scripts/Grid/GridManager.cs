using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }



    public List<Vector2> occupiedGrids = new List<Vector2>();
    public List<Vector2Int> occupiedGrid = new List<Vector2Int>();
   // private List<GameObject> heros = new List<GameObject>();//this is for store the heros in the scene
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

    //use this to add 
    public void AddHeroWithTeamInfo(Vector2Int position)
    {
        occupiedGrid.Add(position);

    }


    //use this to remove
    public void RemoveOccupiedGrid(Vector2Int gridPosition)
    {
        occupiedGrid.Remove(gridPosition);
        Debug.Log($"Removed occupation at {gridPosition}");
    }

    public void AddOccupiedGrid(Vector2 gridPosition)
    {
        // Debug.Log("add :"+gridPosition);
        occupiedGrids.Add(gridPosition);
    }


/*    public string GetHeroTeamAtPosition(Vector2 position)
    {
        
        if (occupiedGridTeams.TryGetValue(position, out string team))
        {
           
            return team;
        }
        return "none"; 
    }
*/


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

 /*   public void AddHero(GameObject hero)
    {
        heros.Add(hero);
    }
*/
/*    public List<GameObject> GetHeros()
    {
        return heros;
    }
*/
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
