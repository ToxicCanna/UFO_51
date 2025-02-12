using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public List<Vector2Int> occupiedGrids = new List<Vector2Int>();
    public Dictionary<Vector2Int, List<HeroInfo>> occupiedGridTeams = new Dictionary<Vector2Int, List<HeroInfo>>();

    public class HeroInfo
    {
        public GameObject heroObj;
        public string side; 

        public HeroInfo(GameObject hero, string side)
        {
            this.heroObj = hero;
            this.side = side;
        }
    }

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
    public void AddHeroWithTeamInfo(Vector2Int position,GameObject hero,string colorSide)
    {
        if (!occupiedGridTeams.ContainsKey(position))
        {
            occupiedGridTeams[position] = new List<HeroInfo>();//init
        }
        occupiedGridTeams[position].Add(new HeroInfo(hero, colorSide));
        Debug.Log($"Grid Added {hero.name} to {position} from side {colorSide}");
    }


    //use this to remove
    public void RemoveOccupiedGrid(Vector2Int position, GameObject hero, string colorSide)
    {
        occupiedGridTeams[position].Remove(new HeroInfo(hero, colorSide));
        Debug.Log($"Grid Remove {hero.name} to {position} from Side {colorSide}");
    }



/*    public void CheckOccupiedHero(Vector2 position)
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


*//*    public List<Vector2Int> GetOccupiedGrids()
    {
        // print all the position been occupied by heros
        Debug.Log("occupiedGrids " + occupiedGrids.Count);
        return occupiedGrids;
    }

*//*

    public bool IsGridOccupied(Vector2Int gridPosition)
    {
        return occupiedGrids.Contains(gridPosition);
    }*/
}
