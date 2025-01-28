using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    private HashSet<Vector2> occupiedGrids = new HashSet<Vector2>();

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

    public void AddOccupiedGrid(Vector2 gridPosition)
    {
        occupiedGrids.Add(gridPosition);
       // Debug.Log("occupiedGrids" +occupiedGrids.Count);
        foreach (Vector2 grid in occupiedGrids)
        {
            Debug.Log("Occupied Grid: " + grid);
            
        }
    }

    public  HashSet<Vector2> GetOccupiedGrid(Vector2 gridPositionWithHero)
    {
        // print all the position been occupied by heros
      
       return occupiedGrids;
       Debug.Log("occupiedGrids "+ occupiedGrids);
       
    }

    public void RemoveOccupiedGrid(Vector2 gridPosition)
    {
        occupiedGrids.Remove(gridPosition);
    }

    public bool IsGridOccupied(Vector2 gridPosition)
    {
        return occupiedGrids.Contains(gridPosition);
    }
}
