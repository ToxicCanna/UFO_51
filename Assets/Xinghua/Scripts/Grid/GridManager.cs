using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }




    public List<Vector2Int> occupiedGrids = new List<Vector2Int>();

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
        occupiedGrids.Add(position);

    }


    //use this to remove
    public void RemoveOccupiedGrid(Vector2Int gridPosition)
    {
        occupiedGrids.Remove(gridPosition);
        Debug.Log($"Removed occupation at {gridPosition}");
    }

    public void AddOccupiedGrid(Vector2Int gridPosition)
    {
        // Debug.Log("add :"+gridPosition);
        occupiedGrids.Add(gridPosition);
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


    public List<Vector2Int> GetOccupiedGrids()
    {
        // print all the position been occupied by heros
        Debug.Log("occupiedGrids " + occupiedGrids.Count);
        return occupiedGrids;
    }



    public bool IsGridOccupied(Vector2Int gridPosition)
    {
        return occupiedGrids.Contains(gridPosition);
    }
}
