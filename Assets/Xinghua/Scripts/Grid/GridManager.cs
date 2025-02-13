using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    //public List<Vector2Int> occupiedGrids = new List<Vector2Int>();
    public Dictionary<Vector2Int, List<HeroInfo>> occupiedGridTeams = new Dictionary<Vector2Int, List<HeroInfo>>();

    private List<GameObject> herosAtRedSpawnPosition = new List<GameObject>();
    private List<GameObject> herosAtBlueSpawnPosition = new List<GameObject>();
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
    public void AddSpawnHero(GameObject hero, string color)
    {
        if (color == "Red")
        {
            herosAtRedSpawnPosition.Add(hero);
        }
        else if (color == "Blue")
        {
            herosAtBlueSpawnPosition.Add(hero);
        }
        Debug.Log("herosAtRedSpawnPosition" + herosAtRedSpawnPosition.Count);
        Debug.Log("herosAtBlueSpawnPosition" + herosAtBlueSpawnPosition.Count);

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

    //use this to add 
    public void AddHeroWithTeamInfo(Vector2Int position, GameObject hero, string colorSide)
    {
        if (!occupiedGridTeams.ContainsKey(position))
        {
            occupiedGridTeams[position] = new List<HeroInfo>();//init
        }
        occupiedGridTeams[position].Add(new HeroInfo(hero, colorSide));
        // Debug.Log("occupiedGridTeams count" + occupiedGridTeams.Count);
    }


    //use this to remove
    public void RemoveOccupiedGrid(Vector2Int position, GameObject hero, string colorSide)
    {
        if (occupiedGridTeams.ContainsKey(position))
        {
            int beforeCount = occupiedGridTeams[position].Count;


            occupiedGridTeams[position].RemoveAll(h => h.heroObj == hero && h.side == colorSide);


            if (occupiedGridTeams[position].Count == 0)
            {
                occupiedGridTeams.Remove(position);
                Debug.Log($"Position {position} is now empty and removed from occupiedGridTeams.");
            }

            int afterCount = occupiedGridTeams.Count;
            Debug.Log($"Before: {beforeCount}, After: {afterCount}, Dictionary Count: {occupiedGridTeams.Count}");
        }

    }




    public bool IsSpawnOccupied()
    {

        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            foreach (var hero in herosAtRedSpawnPosition)
            {
                if (hero.transform.position.x == 0 && hero.transform.position.y == 4)
                {
                    return true;
                }


            }
        }
        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide)
        {
            foreach (var hero in herosAtBlueSpawnPosition)
            {
                if (hero.transform.position.x == 9 && hero.transform.position.y == 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /*    public List<Vector2Int> GetAllOccupiedPositions()
        {

            occupiedGrids.Clear(); 
            Debug.Log("Printing all occupied positions:");

            foreach (var entry in occupiedGridTeams)
            {
                Vector2Int position = entry.Key;
                List<HeroInfo> heroes = entry.Value;

                if (heroes.Count > 0)
                {
                    occupiedGrids.Add(position);
                    Debug.Log($"Position: {position} | Hero Count: {heroes.Count}");
                }
            }
            return occupiedGrids;
        }*/


}
