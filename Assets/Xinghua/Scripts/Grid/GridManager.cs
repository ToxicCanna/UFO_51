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
        Debug.Log($"Grid Added {hero.name} to {position} from side {colorSide}");
    }


    //use this to remove
    public void RemoveOccupiedGrid(Vector2Int position, GameObject hero, string colorSide)
    {
        occupiedGridTeams[position].Remove(new HeroInfo(hero, colorSide));
        Debug.Log($"Grid Remove {hero.name} to {position} from Side {colorSide}");
    }




    public bool IsGridOccupied()
    {
        Vector2Int redTargetPosition = new Vector2Int(0, 4);
        Vector2Int blueTargetPosition = new Vector2Int(8, 3);

        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            if (occupiedGridTeams.ContainsKey(redTargetPosition))
            {
                foreach (HeroInfo hero in occupiedGridTeams[redTargetPosition])
                {
                    if (hero.side == "Red")
                    {
                        return true;
                    }
                }
            }
        }
        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide)
        {
            if (occupiedGridTeams.ContainsKey(blueTargetPosition))
            {
                foreach (HeroInfo hero in occupiedGridTeams[blueTargetPosition])
                {
                    if (hero.side == "Blue")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
