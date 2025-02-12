using System.Collections.Generic;
using UnityEngine;

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
    public void AddHeroWithTeamInfo(Vector2Int position, GameObject hero, string colorSide)
    {
        if (!occupiedGridTeams.ContainsKey(position))
        {
            occupiedGridTeams[position] = new List<HeroInfo>();//init
        }
        occupiedGridTeams[position].Add(new HeroInfo(hero, colorSide));

    }


    //use this to remove
    public void RemoveOccupiedGrid(Vector2Int position, GameObject hero, string colorSide)
    {
        occupiedGridTeams[position].Remove(new HeroInfo(hero, colorSide));

    }





    public string GetGridOccupiedHeroType(Vector2Int position)
    {
    

        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            if (occupiedGridTeams.ContainsKey(position))
            {
                foreach (HeroInfo hero in occupiedGridTeams[position])
                {
                    if (hero.side == "Red")
                    {
                        return "Red";
                    }
                }
            }
        }
        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide)
        {
            if (occupiedGridTeams.ContainsKey(position))
            {
                foreach (HeroInfo hero in occupiedGridTeams[position])
                {
                    if (hero.side == "Blue")
                    {
                        return "Blue";
                    }
                }
            }
        }

        return "";
    }

}
