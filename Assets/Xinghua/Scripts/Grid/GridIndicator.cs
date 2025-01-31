using System;
using System.Collections.Generic;
using TMPro;
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
    public event Action onHeroPositon;
    public event Action heroUnselected;
    public event Action targetSelecting;
    //public event Action rollingDice;//this is for the dic roll function
    public event Action activeShop;



    private Vector3 heroPosition;
    //grid Occupied
    private Vector3 newIndicatorLocation;
    private Vector3 oldIndicatorLocation;

    private Vector2Int currentGridPosition;
    private List<GameObject> herosInRedSide;
    private List<GameObject> herosInBlueSide;
    private List<GameObject> currentTurnHeros;
    public HeroData submitHeroData;
    private bool isOnHeroPosition = true;
    private bool isHeroSubmited = false;
    private HashSet<Vector2Int> allowedPositions;
    private bool isHaveTargets = false;
    private bool isCanChooseTarget = false;
    public HeroData submitedTargetHero;

    [SerializeField] private TMP_Text playerText;

    //(Killian)
    //Reference to BattleManager
    [SerializeField] private BattleManager battleManager;

    void Start()
    {
        currentGridPosition = WorldToGridPosition(transform.position);
        transform.position = GridToWorldPosition(currentGridPosition);

        currentTurn = PlayerTurn.PlayerRedSide;
        minI = 0; maxI = 9;
        minJ = 0; maxJ = 7;

        Debug.Log("Start current turn :" + currentTurn);
       
    }


    private void Update()
    {
        /*    var occupiedGrids = GridManager.Instance.GetOccupiedGrids();
            Debug.Log("occupiedGrids" + occupiedGrids.Count);
            foreach (var grid in occupiedGrids)
            {
                Debug.Log("occupied" + grid);
                if (grid.x == transform.position.x && grid.y == transform.position.y)
                {
                    isOnHeroPosition = true;
                }
                else
                {
                    isOnHeroPosition = false;
                }
            }*/
        /* Debug.Log("isOnHeroPosition" + isOnHeroPosition);
         Debug.Log("isHeroSubmited" + isHeroSubmited);*/

    }
    public bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= minI && position.x <= maxI &&
               position.y >= minJ && position.y <= maxJ;
    }
    public bool IsWithinMoveDirection(Vector2Int centerPosition, Vector2Int newPosition)
    {
        //old 
        var oldX = centerPosition.x;
        var oldY = centerPosition.y;

        var newX = newPosition.x;
        var newY = newPosition.y;
        if (oldX - 1 <= newX && newX <= oldX + 1 && oldY - 1 <= newY && newY <= oldY + 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //private void UpdateIndicatorToSelectedHero()
    //{
    //    var currentHeroPosition = heroSelect.GetSelectedHeroPosition();
    //}

    public void HandleIndicatorMoveNew(Vector2 direction)
    {
        Debug.Log("Handle move isOnHeroPosition" + isOnHeroPosition);
        Debug.Log("Handle move isHeroSubmited" + isHeroSubmited);
        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));


        if (isHeroSubmited && isOnHeroPosition)
        {
            Debug.Log("move with range");
            //move the indicator with limit
            MoveIndicatorWithRange(intDirection);
        }
        else if (isCanChooseTarget)
        {
            //switch to attack state
        }
        //else
        //{

        //    Debug.Log("move without range");
        //    // only move indicator 
        //    HandleIndicatorMove(intDirection);
        //}
    }
    private int currentIndex = 0;
    public void ChooseTargets(Vector2 direction)
    {
        Debug.Log("move in opponent side hero list");
        GetOppositeHeroAsTarget();
        Debug.Log("opposite heros count:" + GetOppositeHeroAsTarget().Count);
        foreach (var pos in GetOppositeHeroAsTarget())
        {
            Debug.Log("opposite heros location:" + pos.transform.position);
        }
        // Determine new index based on direction
        int moveDirection = 0;

        if (direction.y > 0) moveDirection = 1;  // Move forward (W)
        if (direction.y < 0) moveDirection = -1; // Move backward (S)

        if (moveDirection != 0)
        {
            currentIndex = 0;
            // Update current index
            currentIndex += moveDirection;

            // Loop back when reaching the last/first item
            if (currentIndex >= GetOppositeHeroAsTarget().Count) currentIndex = 0;
            if (currentIndex < 0) currentIndex = GetOppositeHeroAsTarget().Count - 1;

            // Move the indicator to the new selected hero position
            transform.position = GetOppositeHeroAsTarget()[currentIndex].transform.position;

            Debug.Log("Indicator moved to: " + transform.position);
        }

    }

    private List<GameObject> GetOppositeHeroAsTarget()
    {
        List<GameObject> opponiteHeros = new List<GameObject>();
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            opponiteHeros = HeroPocketManager.Instance.GetAllBlueSideHeroes();

        }
        else
        {
            opponiteHeros = HeroPocketManager.Instance.GetAllRedSideHeroes();
        }
        return opponiteHeros;

    }

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
                //Debug.Log("hero postion:" + hero.transform.position.x + ", " + hero.transform.position.y);
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
                Debug.Log("heroMoveIndex with name:" + heroPath.gameObject.name);
                // activeShop?.Invoke();
            }
            else
            {
                heroUnselected?.Invoke();
            }
            newIndicatorLocation = transform.position;
        }
    }



    public void MoveIndicatorWithRange(Vector2 direction)
    {

        Debug.Log("move with range direction:" + direction);
        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
        Vector2Int targetPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y) + intDirection;
        Debug.Log("move with range oldPosition:" + currentGridPosition);
        Debug.Log("move with range targetPosition:" + targetPosition);
        var currentIndicatorPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Debug.Log(" IsWithinBounds(targetPosition):" + IsWithinBounds(targetPosition));
        Debug.Log(" IsWithinMoveDirection(targetPosition):" + IsWithinMoveDirection(currentGridPosition, targetPosition));
        if (IsWithinBounds(targetPosition) && IsWithinMoveDirection(currentGridPosition, targetPosition))
        {

            Debug.Log("move indicator!!!!!!!!");
            transform.position = new Vector3(targetPosition.x, targetPosition.y);
        }

    }
    private bool isWithinMoveRange()
    {
        // check the target (I,J)value is in range
        var isWithInMoveRange = false;

        return isWithInMoveRange;
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
        GetCurrentPlayerTurn();
        currentGridPosition = WorldToGridPosition(transform.position);
        Debug.Log("MoveToTargetIndicator");
        if (!isOnHeroPosition && !isHeroSubmited) return;

        finishSelection?.Invoke();
        //store the location that was occupied

        oldIndicatorLocation = submitHeroData.gameObject.transform.position;
        GridManager.Instance.RemoveOccupiedGrid(oldIndicatorLocation);

        submitHeroData.gameObject.transform.position = transform.position;
        GridManager.Instance.AddOccupiedGrid(transform.position);
        CheckAttackTargets();

    }
    private void CheckAttackTargets()
    {

        if (!isHaveTargets)
        {
            UpdatePlayerTurn();
            UpdateIndicatorWhenTurnChange();
        }
        else
        {
            Debug.Log("attack directionly");
        }
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
        playerText.text = currentTurn.ToString();
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
        // onHeroPositon?.Invoke();//this if for path highlight to listen
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
        onHeroPositon?.Invoke();
        Debug.Log("current hero submit");
        isOnHeroPosition = true;
        var position = GetSubmitHeroPositon();

        GetSubmitHero(position);
        Debug.Log("submitHeroData" + submitHeroData.gameObject.name);

    }
    public Vector2 GetSubmitHeroPositon()
    {
        Debug.Log("hero submit position:" + transform.position);
        //if is occupied
        var position = new Vector2(transform.position.x, transform.position.y);
        currentGridPosition = new Vector2Int((int)position.x, (int)position.y);
        return position;

    }
    public HeroData GetSubmitHero(Vector2 position)
    {
        var allHerosInScene = HeroPocketManager.Instance.GetAllHeroes();

        foreach (var hero in allHerosInScene)
        {
            if (position.x == hero.transform.position.x && position.y == hero.transform.position.y)
            {
                isHeroSubmited = true;
                var heroData = hero.GetComponent<HeroData>();
                submitHeroData = heroData;
            }

        }
        return submitHeroData;
    }

    public List<GameObject> GetOppositHerosInTheScene()
    {
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            var heros = HeroPocketManager.Instance.GetAllBlueSideHeroes();
            return heros;
        }
        else
        {
            var heros = HeroPocketManager.Instance.GetAllRedSideHeroes();
            return heros;
        }
    }

    public void SbumitedTarget()
    {
        Debug.Log("Submit target to attack");
        //attack each other

        var heros = GetOppositHerosInTheScene();
        for (int i = 0; i < heros.Count; i++)
        {
            //var currentHeroIndex = 0;
            var hero = heros[i];
            if (transform.position.x == hero.transform.position.x || transform.position.y == hero.transform.position.y)
            {
                var HeroData = hero.GetComponent<HeroData>();

                //(Killian)
                //set Target in BattleManager
                battleManager.targetHero = HeroData;
                //Set Current Hero in BattleManager using "GetSubmitHero()"
                battleManager.currentHero = submitHeroData;

                Debug.Log("Target hero selected: " + battleManager.targetHero.name);
                Debug.Log("Attacking with hero: " + battleManager.currentHero.name);
                //end of Killians code
            }
        }
        Debug.Log("kill him!!!!!!!"+ submitedTargetHero.name);
    }
}
