using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridIndicator : MonoBehaviour
{
    // [SerializeField] private List<ScriptableObject> items; 
    private int minI, maxI, minJ, maxJ;

    [SerializeField] private GameObject playerRedHero;
    [SerializeField] private GameObject playerBlueHero;
    GameObject selectedHero;
    private int heroPathID;
    [SerializeField] private GameObject heroPrefab;

    //[SerializeField] private GameObject startPosition;//hero spawn front of the gate
    [SerializeField] GameStateMachine gameStateMachine;
    private HighLight highLight;

    private HeroSelect heroSelect;
    public event Action finishSelection;
    public event Action onHeroPositon;
    public event Action heroUnselected;
    public event Action targetSelecting;
    public event Action AttackHappenOneSpot;
    public event Action activeShop;
    public event Action moveFinish;

    //public event Action rollingDice;//this is for the dic roll function



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
    public Vector2Int selectedHeroPosition;
    private Vector3 submitHeroPosition;
    private int currentSelectedHeroId;
    private bool isAtOppositeHeroPos = false;
    private bool isCancleSelected = false;
    private bool isAttackEnd =false;
    public TMP_Text controlHintText;


    //(Killian)
    //Reference to BattleManager
    [SerializeField] private BattleManager battleManager;

    void Start()
    {
        // Debug.Log("trans" + transform.position);
        currentGridPosition = WorldToGridPosition(transform.position);
        //Debug.Log("currentGridPosition" + currentGridPosition);

        transform.position = GridToWorldPosition(currentGridPosition);
        //Debug.Log("transform.position" + transform.position);
        // currentTurn = PlayerTurn.PlayerRedSide;
        minI = 0; maxI = 9;
        minJ = 0; maxJ = 7;

        //Debug.Log("Start current turn :" + currentTurn);


        herosInRedSide = HeroPocketManager.Instance.GetAllRedSideHeroes();
        herosInBlueSide = HeroPocketManager.Instance.GetAllBlueSideHeroes();
        highLight = FindAnyObjectByType<HighLight>();
        controlHintText.text = "WASD to move";
    }

 


    public bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= minI && position.x <= maxI &&
               position.y >= minJ && position.y <= maxJ;
    }


    public bool IsWithinMoveDirectionNew(Vector2Int centerPosition, Vector2Int newPosition)
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

    public void HandleIndicatorMoveNew(Vector2 direction)
    {
        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
        HandleIndicatorMove(intDirection);
    }

    private int currentIndex = 0;
  

    public void ChooseTargets(Vector2 direction)
    {
        if (direction.x != 0) return;
        // Debug.Log("move in opponent side hero list");
        var oppositHeros = GetOppositeHeros();
        Debug.Log("opposite heros count:" + GetOppositeHeros().Count);
        var oppositeHeroCount = GetOppositeHeros().Count;

        foreach (var pos in GetOppositeHeros())
        {
            Debug.Log("opposite heros location:" + pos.transform.position);
        }
        // Determine new index based on direction
        var x = transform.position.x;
        var y = transform.position.y;

        var nextIndex = 0;
        for (int i = 0; i < oppositeHeroCount; i++)
        {
            var hero = oppositHeros[i];
            if (x == hero.transform.position.x && y == hero.transform.position.y)
            {
                currentIndex = i;
                break;
            }

        }
        if (direction.y > 0)
        {
            //W 
            nextIndex = (currentIndex + 1) % oppositeHeroCount;

        }
        else if (direction.y < 0)
        {
            //S
            nextIndex = (currentIndex - 1 + oppositeHeroCount) % oppositeHeroCount;
        }
        else
        {
            return;
        }

        transform.position = GetOppositeHeros()[nextIndex].transform.position;
        Debug.Log("Indicator moved to: " + transform.position);

    }

    private List<GameObject> GetCurrentTurnPlayerHeros()
    {
        List<GameObject> opponiteHeros = new List<GameObject>();
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            opponiteHeros = HeroPocketManager.Instance.GetAllRedSideHeroes();

        }
        else
        {
            opponiteHeros = HeroPocketManager.Instance.GetAllBlueSideHeroes();
        }
        return opponiteHeros;

    }
    private List<GameObject> GetOppositeHeros()
    {
        List<GameObject> opponiteHeros = new List<GameObject>();
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
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

        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt((int)direction.x), Mathf.RoundToInt((int)direction.y));
        Vector2Int targetPosition = currentGridPosition + intDirection;
        if (IsWithinBounds(targetPosition))
        {
            //Debug.Log("move indicator");
            currentGridPosition = targetPosition;
            transform.position = GridToWorldPosition(currentGridPosition);
            // transform.position += new Vector3((int)direction.x, (int)direction.y, 0);
            // Debug.Log("currentGridPosition will move" + currentGridPosition);
            //judge if this position have hero already
            //var heros = GridManager.Instance.GetHeros();




            /*  var heros = HeroPocketManager.Instance.GetAllHeroes();
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
              newIndicatorLocation = transform.position;*/
        }
    }


    /*
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

        }*/
    private bool isWithinMoveRange()
    {
        // check the target (I,J)value is in range
        var isWithInMoveRange = false;

        return isWithInMoveRange;
    }


    public void MoveToTargetIndicator()
    {
        if (!isOnHeroPosition || isCancleSelected) return;
        currentGridPosition = WorldToGridPosition(transform.position);

        //check the target is valid or not ,if valid move the hero to indicator current position
        var validPos = highLight.GetNeighbors(GetSubmitHeroPositon(), currentSelectedHeroId);
        var canMove = false;
        foreach (var pos in validPos)
        {
            if (pos.x == currentGridPosition.x && pos.y == currentGridPosition.y)
            {
                canMove = true;
            }
        }

        if (!canMove)
        {
            Debug.Log("target is invalid");
        }
        else
        {
            //update the gride status ;if not occupied empty it
            oldIndicatorLocation = submitHeroData.gameObject.transform.position;


            GridManager.Instance.RemoveOccupiedGrid(oldIndicatorLocation);


            GridManager.Instance.RemoveOccupiedGrid(oldIndicatorLocation);
            submitHeroData.gameObject.transform.position = transform.position;//move the hero
            currentGridPosition = WorldToGridPosition(transform.position);
            // GridManager.Instance.AddOccupiedGrid(transform.position);
            GridManager.Instance.AddHeroWithTeamInfo(submitHeroData.gameObject);

            CheckAttackTargets();

            finishSelection?.Invoke();//if finish move hide the highlight
            isHeroSubmited = false;
            GameManager.Instance.UpdateHeroSubmissionState(isHeroSubmited);
            UpdatePlayerTurn();
            if (isAttackEnd = true)
            {
                SetIndicatorInCurrentHeroPos();
            }
            

            isCancleSelected = false;
        }

    }



    private void CheckAttackTargets()
    {
        Debug.Log("CheckAttackTargets");
        battleManager.currentHero = submitHeroData.GetComponent<HeroData>();
        //this just when two player in one spot attack each other
        var herosOpposite = GetOppositeHeros();
        Debug.Log("herosOpposite " + herosOpposite.Count);

        foreach (var hero in herosOpposite)
        {
            Debug.Log("hero oppo" + hero.name);
            Debug.Log("target positon:" + currentGridPosition);
            Debug.Log("hero.transform.position" + hero.transform.position);


            //if (currentGridPosition.x == hero.transform.position.x && currentGridPosition.y == hero.transform.position.y)
             if (transform.position == hero.transform.position)
            {

                Debug.Log("same pos:" + transform.position);
                Debug.Log("targetHero" + hero.name);
                //Attack opposite
                if (submitHeroData.gameObject != hero)
                {
                    Debug.Log(" attack happen");
                    //set target hero
                    var targetHero = hero.GetComponent<HeroData>();
                    StartCoroutine(RollDiceAndApplyDamage(targetHero));
                    // AttackHappenOneSpot?.Invoke();
                }

            }
        }

    }

    private IEnumerator RollDiceAndApplyDamage(HeroData targetHero)
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Roll dice");

        battleManager.targetHero = targetHero.GetComponent<HeroData>();

        //call attack after setting values
        yield return new WaitForSeconds(5);
        Debug.Log("give damage");
        battleManager.Attack();
        if (targetHero != null&& submitHeroData)
        {
            Animator animatorSelected = submitHeroData.gameObject.GetComponent<Animator>();
            Animator animator = targetHero.gameObject.GetComponent<Animator>();
            animatorSelected.SetBool("IsAtk", true);
            animator.SetBool("IsDmg", true);
        }
        isAttackEnd = true;
    }

    private void SetIndicatorInCurrentHeroPos()
    {
        var heros = GetCurrentTurnPlayerHeros();
        transform.position = heros[0].transform.position;
        //Debug.Log("oppsite hero name:" + heros[0].name);
        currentGridPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    private void SetIndicatorInOppositeHeroPos()
    {
        var heros = GetOppositHerosInTheScene();
        transform.position = heros[0].transform.position;
        Debug.Log("oppsite hero name:" + heros[0].name);
        currentGridPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }


    private void UpdatePlayerTurn()
    {
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            GameManager.Instance.currentTurn = GameManager.PlayerTurn.PlayerBlueSide;

        }
        else
        {
            GameManager.Instance.currentTurn = GameManager.PlayerTurn.PlayerRedSide;
        }
        GameManager.Instance.UpdateCoinCount();
    }

    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int i = Mathf.FloorToInt(worldPosition.x);
        int j = Mathf.FloorToInt(worldPosition.y);
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

        Debug.Log("red hero coun when select :" + herosInRedSide.Count);
        Debug.Log("blue hero coun when select :" + herosInBlueSide.Count);
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
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
        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide)
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

    public void ChooseHeroAction()
    {
        gameStateMachine.SwitchToUIState();
    }

    private bool IsOppsiteHeroHere(Vector2Int indicatorPosition)
    {
        var teamInfo = GridManager.Instance.GetHeroTeamAtPosition(indicatorPosition);
        Debug.Log("team" + teamInfo);
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerBlueSide && teamInfo == "red")
        {

            return true;
        }
        else if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide && teamInfo == "blue")
        {
            return true;
        }
        return false;
    }

    public void HandleSubmitHeroSelected()
    {
        Debug.Log("HandleSubmitHeroSelected");
        var indicatorPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        if (isHeroSubmited || (IsOppsiteHeroHere(indicatorPosition))) return;

        isOnHeroPosition = true;
        isHeroSubmited = true;
        isCancleSelected = false;
        var position = GetIndicatorPositon();
        Debug.Log("hero submit position:" + position);
        //selectedHeroPosition =new Vector2Int((int)transform.position.x,(int)transform.position.y);
        SetSubmitHero(position);

        currentSelectedHeroId = GetSubmitHeroPathIndex(position);
        Debug.Log("submitHeroID" + GetSubmitHeroPathIndex(position));
        onHeroPositon?.Invoke();//show the path

        GameManager.Instance.UpdateHeroSubmissionState(isHeroSubmited);
        ChooseHeroAction();

    }


    public Vector2Int GetSubmitHeroPositon()
    {
        return new Vector2Int((int)submitHeroData.transform.position.x, (int)submitHeroData.transform.position.y);
    }

    public Vector2Int GetIndicatorPositon()
    {
        //if is occupied
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public void SetSubmitHero(Vector2 position)
    {
        if (isCancleSelected) submitHeroData = null;
        else
        {
            var allHerosInScene = HeroPocketManager.Instance.GetAllHeroes();

            foreach (var hero in allHerosInScene)
            {
                if (position.x == hero.transform.position.x && position.y == hero.transform.position.y)
                {

                    //isHeroSubmited = true;
                    var heroData = hero.GetComponent<HeroData>();
                    submitHeroData = heroData;
                }

            }

        }
    }


    public HeroData GetSubmitHero(Vector2 position)
    {
        return submitHeroData;
    }

    public int GetSubmitHeroPathIndex(Vector2 position)
    {
        var allHerosInScene = HeroPocketManager.Instance.GetAllHeroes();

        foreach (var hero in allHerosInScene)
        {
            if (position.x == hero.transform.position.x && position.y == hero.transform.position.y)
            {
                //isHeroSubmited = true;
                var heroPath = hero.GetComponent<HeroPath>();
                var submitHeroPathID = heroPath.heroPathID;
                return submitHeroPathID;
            }
        }

        return 0;
    }
    public List<GameObject> GetOppositHerosInTheScene()
    {
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
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
        Debug.Log("kill him!!!!!!!");
    }

    internal void ActiveShopMenu()
    {
        gameStateMachine.SwitchToUIState();
    }

    internal void CancleSelected()
    {
        Debug.Log("cancle selected");
        isCancleSelected = true;
        finishSelection?.Invoke();
        var position = GetIndicatorPositon();
        SetSubmitHero(position);
        isHeroSubmited = false;
    }
}
