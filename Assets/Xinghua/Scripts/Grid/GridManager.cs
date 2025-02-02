using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    //public HashSet<Vector2> occupiedGrids = new HashSet<Vector2>();

    private List<Vector2> occupiedGrids = new List<Vector2>();
    private List<GameObject> heros = new List<GameObject>();//this is for store the heros in the scene

    //[SerializeField]private List<GameObject> redSideHerosScene = new List<GameObject>();
    //[SerializeField] private List<GameObject> blueSideHerosScne = new List<GameObject>();
    private Vector2 indicatorPos;
    private void Start()
    {
        foreach (var hero in heros)
        {
            AddOccupiedGrid(hero.transform.position);
        }

    }
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
        // Debug.Log("add :"+gridPosition);
        occupiedGrids.Add(gridPosition);
    }
    public void RemoveOccupiedGrid(Vector2 gridPosition)
    {
        occupiedGrids.Remove(gridPosition);
        // Debug.Log("remove :" + gridPosition);
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
    /*  public HashSet<Vector2> GetOccupiedGrids()
      {
          // print all the position been occupied by heros
          Debug.Log("occupiedGrids " + occupiedGrids);
          return occupiedGrids;
      }*/


    public bool IsGridOccupied(Vector2 gridPosition)
    {
        return occupiedGrids.Contains(gridPosition);
    }
}
