using System.Collections.Generic;
using UnityEngine;

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
           // Debug.Log("Occupied Grid: " + grid);
        }
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
