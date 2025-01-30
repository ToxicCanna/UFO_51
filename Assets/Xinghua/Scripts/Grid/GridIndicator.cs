using System;
using System.Collections.Generic;
using UnityEngine;


public class GridIndicator : MonoBehaviour
{
    // [SerializeField] private List<ScriptableObject> items; 
    public int minI = 0, maxI, minJ, maxJ = 7;
    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;

    [SerializeField] private GameObject playerRedHero;
    [SerializeField] private GameObject playerBlueHero;
    GameObject selectedHero;
    private int heroPathID;
    [SerializeField] private GameObject heroPrefab;
    // private Vector2 currentGridPosition;
    //[SerializeField] private GameObject startPosition;//hero spawn front of the gate
    private HeroSelect heroSelect;
    public event Action finishSelection;
    public event Action heroSelecting;
    public event Action heroUnselected;
    public event Action rollingDice;//this is for the dic roll function
    public event Action activeShop;

    private Vector3 heroPosition;
    private Vector3 newIndicatorLocation;
    private Vector2Int currentGridPosition;
    private List<GameObject> herosInRedSide;
    private List<GameObject> herosInBlueSide;
    private List<GameObject> currentTurnHeros;
    public HeroData submitHeroData;
    private bool isHeroSelected =false;
    void Start()
    {
        currentGridPosition = WorldToGridPosition(transform.position);
        transform.position = GridToWorldPosition(currentGridPosition);

        currentTurn = PlayerTurn.PlayerRedSide;
        minI = 0; maxI = 9;
        minJ = 0; maxJ = 7;
    }

    public bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= minI && position.x <= maxI &&
               position.y >= minJ && position.y <= maxJ;
    }
    //private void UpdateIndicatorToSelectedHero()
    //{
    //    var currentHeroPosition = heroSelect.GetSelectedHeroPosition();
    //}


    public void HandleIndicatorMove(Vector2 direction)
    {
        //Debug.Log("currentPosition x:" + currentGridPosition.x + ",y:" + currentGridPosition.y);
        //Debug.Log("direction.x:" + direction.x+ ",direction.y:"+ direction.y);

        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
        Vector2Int targetPosition = currentGridPosition + intDirection;
        if (IsWithinBounds(targetPosition))
        {
            //Debug.Log("move indicator");
            currentGridPosition = targetPosition;

            transform.position += new Vector3((int)direction.x, (int)direction.y, 0);
            Debug.Log("currentGridPosition after move" + currentGridPosition);
            //judge if this position have hero already
            //var heros = GridManager.Instance.GetHeros();




            var heros = HeroPocketManager.Instance.GetAllHeroes();
            var isHeroOccupied = false;

            foreach (var hero in heros)
            {
                Debug.Log("hero postion:" + hero.transform.position.x + ", " + hero.transform.position.y);
                if (hero.transform.position.x == currentGridPosition.x && hero.transform.position.y == currentGridPosition.y)
                {
                    isHeroOccupied = true;
                    Debug.Log("current position have hero");
                    selectedHero = hero;
                }
            }
            if (isHeroOccupied)
            {
                var heroPath = selectedHero.GetComponent<HeroPath>();
                Debug.Log("heroData:" + heroPath);
                heroPathID = heroPath.GetHeroMoveIndex();
                Debug.Log("heroMoveIndex:" + heroPathID);
                // activeShop?.Invoke();
                heroSelecting?.Invoke();//this if for path highlight to listen
            }
            else
            {
                heroUnselected?.Invoke();
            }



            newIndicatorLocation = transform.position;

        }
    }


    public int GetHeroMoveIndex()
    {
        return heroPathID;
    }

    //private void UpdateHeroPosition()
    //{
    //    heroPosition = heroSelect.GetSelectedHeroPosition();
    //}
    public void MoveToTargetIndicator()
    {
        Debug.Log("MoveToTargetIndicator");

        finishSelection?.Invoke();
        //store the location that was occupied
        GridManager.Instance.AddOccupiedGrid(newIndicatorLocation);

        if (GetCurrentPlayerTurn() == PlayerTurn.PlayerBlueSide)
        {
            playerBlueHero.transform.position = transform.position;//PlayerBlue Hero need Dynamic from array
        }
        else
        {
            playerRedHero.transform.position = transform.position;//PlayerRedHero need Dynamic
        }

        UpdatePlayerTurn();
        UpdateIndicatorWhenTurnChange();
    }
    private void UpdateIndicatorWhenTurnChange()
    {
        Debug.Log("current turn:" + currentTurn);
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            //transform.position = herosInRedSide[0].transform.position;
            transform.position = playerRedHero.transform.position;
        }
        else
        {
            //transform.position = herosInBlueSide[0].transform.position;
            transform.position = playerBlueHero.transform.position;
        }
    }
    private PlayerTurn GetCurrentPlayerTurn()
    {
        return currentTurn;
    }

    private PlayerTurn GetNextPlayerTurn()
    {
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            currentTurn = PlayerTurn.PlayerBlueSide;
        }
        else
        {
            currentTurn = PlayerTurn.PlayerRedSide;
        }
        return currentTurn;
    }
    private void UpdatePlayerTurn()
    {
        if (currentTurn == PlayerTurn.PlayerBlueSide)
        {
            currentTurn = PlayerTurn.PlayerRedSide;
        }
        else
        {
            currentTurn = PlayerTurn.PlayerBlueSide;
        }
    }

    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int i = Mathf.RoundToInt(worldPosition.x);
        int j = Mathf.RoundToInt(worldPosition.y);
        return new Vector2Int(i, j);
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        float x = gridPosition.x;
        float y = gridPosition.y;
        return new Vector3(x, y, 0);
    }
    public void HandleSelectHero()
    {
        herosInRedSide = HeroPocketManager.Instance.GetAllRedSideHeroes();
        Debug.Log("start hero in red side :" + herosInRedSide.Count);
        herosInBlueSide = HeroPocketManager.Instance.GetAllBlueSideHeroes();
        Debug.Log("start hero in blue side :" + herosInBlueSide.Count);
        Debug.Log("use X key to switch");
        //Debug.Log("current indicator position" + transform.position);
        Debug.Log("current turn" + GetCurrentPlayerTurn());
        Debug.Log("red hero coun when select :" + herosInRedSide.Count);
        Debug.Log("blue hero coun when select :" + herosInBlueSide.Count);
        if (GetCurrentPlayerTurn() == PlayerTurn.PlayerRedSide)
        {

            //Debug.Log("count:"+herosInRedSide.Count);
            var currentHeroIndex = 0;
            for (int i = 0; i < herosInRedSide.Count; i++)
            {
                Debug.Log($"Index: {i}, Value: {herosInRedSide[i]}");
                var hero = herosInRedSide[i];
                Debug.Log("hero position x:" + hero.transform.position.x + " y:" + hero.transform.position.y);

                if (hero.transform.position.x == transform.position.x && hero.transform.position.y == transform.position.y)
                {
                    Debug.Log("current i :" + i);
                    currentHeroIndex = i;
                    break;
                }
            }
            var nextHeroIndex = (currentHeroIndex + 1) % herosInRedSide.Count;
            var nextHero = herosInRedSide[nextHeroIndex];
            transform.position = nextHero.transform.position;
        }
        else if (GetCurrentPlayerTurn() == PlayerTurn.PlayerBlueSide)
        {

            //Debug.Log("count:"+herosInRedSide.Count);
            var currentHeroIndex = 0;
            for (int i = 0; i < herosInBlueSide.Count; i++)
            {
                Debug.Log($"Index: {i}, Value: {herosInBlueSide[i]}");
                var hero = herosInBlueSide[i];
                Debug.Log("hero position x:" + hero.transform.position.x + " y:" + hero.transform.position.y);

                if (hero.transform.position.x == transform.position.x && hero.transform.position.y == transform.position.y)
                {
                    Debug.Log("current i :" + i);
                    currentHeroIndex = i;
                    break;
                }
            }
            var nextHeroIndex = (currentHeroIndex + 1) % herosInBlueSide.Count;
            var nextHero = herosInBlueSide[nextHeroIndex];
            transform.position = nextHero.transform.position;
        }
    }


    //Submit current selected hero
    public void HandleSubmitHeroSelected()
    {
        Debug.Log("current hero submit");
        var position = GetSubmitHeroPositon();
        GetSubmitHero(position);
        Debug.Log("submitHeroData"+ submitHeroData);
    }
    public Vector2 GetSubmitHeroPositon()
    {
        Debug.Log("hero submit position:" + transform.position);
        //if is occupied
        var positon = new Vector2(transform.position.x, transform.position.y);
        return transform.position;

    }
    public HeroData GetSubmitHero(Vector2 position)
    {
        var allHerosInScene = HeroPocketManager.Instance.GetAllHeroes();

        foreach (var hero in allHerosInScene)
        {
            if (position.x == hero.transform.position.x && position.y ==hero.transform.position.y)
            {
                isHeroSelected = true;
                var heroData =hero.GetComponent<HeroData>();
                submitHeroData=heroData;
            }
            
        }
        return submitHeroData;
    }
}
