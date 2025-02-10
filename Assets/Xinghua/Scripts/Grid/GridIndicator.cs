using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] Effect effect;
    private HighLight highLight;

    private HeroSelect heroSelect;
    public event Action hideHighlight;
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
    private bool isAttackEnd = false;
    public TMP_Text controlHintText;



    //(Killian)
    //Reference to BattleManager
    [SerializeField] private BattleManager battleManager;

    void Start()
    {

        currentGridPosition = WorldToGridPosition(transform.position);


        transform.position = GridToWorldPosition(currentGridPosition);

        minI = 0; maxI = 9;
        minJ = 0; maxJ = 7;

        herosInRedSide = HeroPocketManager.Instance.GetAllRedSideHeroes();
        herosInBlueSide = HeroPocketManager.Instance.GetAllBlueSideHeroes();
        highLight = FindAnyObjectByType<HighLight>();
        controlHintText.text = "WASD_move;Enter_submit ;Q_cancle";
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


    private int currentIndex = 0;
    private bool canMove;

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
        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt((int)direction.x), Mathf.RoundToInt((int)direction.y));
        Vector2Int targetPosition = currentGridPosition + intDirection;
        if (IsWithinBounds(targetPosition))
        {
            //Debug.Log("move indicator");
            currentGridPosition = targetPosition;
            transform.position = GridToWorldPosition(currentGridPosition);

        }
    }



    private bool isWithinMoveRange()
    {
        // check the target (I,J)value is in range
        var isWithInMoveRange = false;

        return isWithInMoveRange;
    }


    public void MoveHeroToTargetPosition()
    {
        Debug.Log("!isOnHeroPosition" + !isOnHeroPosition);
        Debug.Log("isCancleSelected" + isCancleSelected);
        if (!isOnHeroPosition)
        {
            //GameManager.Instance.DisplayControlText( " can not choose opposite side hero");
            return;
        }
        if ( isCancleSelected) return;
        currentGridPosition = WorldToGridPosition(transform.position);
        if (IsIndicatorOnOriginalPosition(currentGridPosition)) return;//this make the player can not choose curent selected hero positon as target
        Debug.Log("move hero");
        foreach (var pos in validTargetPos)
        {
            Debug.Log("currentGridPosition" + currentGridPosition);
            Debug.Log("Valid Target Positions: " + string.Join(", ", validTargetPos));
            if ((int)pos.x == (int)currentGridPosition.x && (int)pos.y == (int)currentGridPosition.y)
            {
                Debug.Log("Valid Target Positions: " + string.Join(", ", validTargetPos));

                //update the gride status ;if not occupied empty it
                oldIndicatorLocation = submitHeroData.gameObject.transform.position;


                GridManager.Instance.RemoveOccupiedGrid(WorldToGridPosition(oldIndicatorLocation), submitHeroData.gameObject, submitHeroData.side);



                // submitHeroData.gameObject.transform.position = transform.position;//move the hero
                StartCoroutine(MoveHeroToPosition(transform.position, 1f));
                hideHighlight?.Invoke();//if finish move hide the highlight
            }
            else
            {
                Debug.Log("target is invalid");
            }
        }



        gameStateMachine.SwitchToGameplayState();//this is very important
    }


    private IEnumerator MoveHeroToPosition(Vector2 targetPosition, float speed)
    {
        Animator animatorSelected = submitHeroData.gameObject.GetComponent<Animator>();
        Vector2 startPosition = (Vector2)submitHeroData.transform.position;
        Vector2 direction = targetPosition - startPosition;


        bool needTurn = Mathf.Abs(direction.x) > 0 && Mathf.Abs(direction.y) > 0;

        animatorSelected.SetBool("IsRun", true);

        if (needTurn)
        {

            Vector2 intermediatePosition = new Vector2(targetPosition.x, startPosition.y);
            yield return StartCoroutine(MoveHeroToPointAndUpdatIndicator(intermediatePosition, speed));


            yield return StartCoroutine(MoveHeroToPointAndUpdatIndicator(targetPosition, speed));
        }
        else
        {

            yield return StartCoroutine(MoveHeroToPointAndUpdatIndicator(targetPosition, speed));
        }
        currentGridPosition = WorldToGridPosition(transform.position);

        GridManager.Instance.AddHeroWithTeamInfo(WorldToGridPosition(submitHeroData.gameObject.transform.position), submitHeroData.gameObject, submitHeroData.side);

        CheckAutoAttack();

        if (!isAutoAttack)
        {
            isHeroSubmited = false;
            GameManager.Instance.UpdateHeroSubmissionState(isHeroSubmited);
            UpdatePlayerTurn();
            SetIndicatorInCurrentHeroPos();
            isCancleSelected = false;

            animatorSelected.SetBool("IsRun", false);
        }
    }


    private IEnumerator MoveHeroToPointAndUpdatIndicator(Vector2 destination, float speed)
    {

        while ((Vector2)submitHeroData.transform.position != destination)
        {
            submitHeroData.transform.position = Vector2.MoveTowards(
                submitHeroData.transform.position,
                destination,
                speed * Time.deltaTime
            );

            yield return null;
        }

    }

    bool isAutoAttack = false;
    private void CheckAutoAttack()
    {
        Debug.Log("CheckAttackTargets");
        battleManager.currentHero = submitHeroData.GetComponent<HeroData>();
        //this just when two player in one spot attack each other
        var herosOpposite = GetOppositeHeros();
        foreach (var hero in herosOpposite)
        {
            if (WorldToGridPosition(transform.position) == WorldToGridPosition(hero.transform.position))
            {
                isAutoAttack = true;
                /* Debug.Log("same pos:" + transform.position);
                 Debug.Log("targetHero" + hero.name);*/
                //Attack opposite
                if (submitHeroData.gameObject != hero)
                {
                    Debug.Log(" attack happen");
                    GameManager.Instance.isBattling = true;
                    hideHighlight?.Invoke();
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
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Roll dice");
        Effect.Instance.PlayAttackEffect(targetHero.transform.position);


        battleManager.targetHero = targetHero.GetComponent<HeroData>();

        //call attack after setting values
        yield return new WaitForSeconds(2);
        Effect.Instance.HideAttackEffect();

        yield return new WaitForSeconds(1);
        Debug.Log("give damage");
        battleManager.Attack();
        if (targetHero != null && submitHeroData != null)
        {
            Animator animatorSelected = submitHeroData.gameObject.GetComponent<Animator>();
            if (isAutoAttack)
            {
               
                animatorSelected.SetBool("IsAtk", true);
            }
            else
            {
                animatorSelected.SetBool("IsAtk", false);
            }
            Animator animatorTarget = targetHero.gameObject.GetComponent<Animator>();
         
            animatorTarget.SetBool("IsDmg", true);

            yield return new WaitForSeconds(1);
            isAttackEnd = true;
            gameStateMachine.SwitchToGameplayState();
            isHeroSubmited = false;


            //must check null ,cause sometime destroyed hero die
            if (animatorSelected != null && animatorSelected.gameObject != null)
            {
                animatorSelected.SetBool("IsAtk", false);
            }

            if (animatorTarget != null && animatorTarget.gameObject != null)
            {
                animatorTarget.SetBool("IsDmg", false);
            }
          
                UpdatePlayerTurn();
                SetIndicatorInCurrentHeroPos();
                gameStateMachine.SwitchToGameplayState();
            
        }
    }

    private void SetIndicatorInCurrentHeroPos()
    {
        var heros = GetCurrentTurnPlayerHeros();
        if (heros != null)
        {
            transform.position = heros[0].transform.position;
        }
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
        GameManager.Instance.AddCoinCount();
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


    private bool IsIndicatorOnOriginalPosition(Vector2Int indicatorPosition)
    {
        var isIndicatorOnCurrentHero = false;
        var heros = GetSameSideHerosInTheScene();
        foreach (var hero in heros)
        {
            if (transform.position == hero.transform.position)
            {
                isIndicatorOnCurrentHero = true;
            }
        }
        return isIndicatorOnCurrentHero;
    }
    private bool IsIndicatorOnSameSideHeroPosition(Vector2Int indicatorPosition)
    {
        var IsIndicatorOnSameSideHeroPosition = false;
        var heros = GetSameSideHerosInTheScene();
        foreach (var hero in heros)
        {
            if (hero != null)
            {
                if (transform.position == hero.transform.position)
                {
                    IsIndicatorOnSameSideHeroPosition = true;
                }
            }
        }
        return IsIndicatorOnSameSideHeroPosition;
    }

    Vector2Int[] validTargetPos;
    public void HandleHeroSelected()
    {
        Debug.Log("HandleHeroSelected");
        var indicatorPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        Debug.Log("isHeroSubmited" + isHeroSubmited);
        Debug.Log("!IsIndicatorOnSameSideHeroPosition(indicatorPosition)" + !IsIndicatorOnSameSideHeroPosition(indicatorPosition));
        // if (isHeroSubmited || !IsIndicatorOnOriginalPosition(indicatorPosition)) return;
        // if (isHeroSubmited) return;
        if (!IsIndicatorOnSameSideHeroPosition(indicatorPosition))
        {
            GameManager.Instance.DisplayControlText(" can not choose opposite side hero");
            return;
        }
        if (isHeroSubmited) return;
        SetSelectedHero(transform.position);

        isOnHeroPosition = true;
        isHeroSubmited = true;
        isCancleSelected = false;
        var position = GetIndicatorPositon();
        currentSelectedHeroId = GetSubmitHeroPathIndex(position);

        onHeroPositon?.Invoke();//show the path
        validTargetPos = highLight.GetNeighbors(GetSelectedHeroPositon(), currentSelectedHeroId);
        GameManager.Instance.UpdateHeroSubmissionState(isHeroSubmited);
        gameStateMachine.SwitchToUIState();//UI state for choose action
        Debug.Log("Valid Target Positions when selected: " + string.Join(", ", validTargetPos) + currentSelectedHeroId);
    }


    public Vector2Int GetSelectedHeroPositon()
    {
        if (submitHeroData != null)
        {
            Vector2Int gridPos = WorldToGridPosition(submitHeroData.transform.position);
            return gridPos;
        }
        return Vector2Int.zero;
    }

    public Vector2Int GetIndicatorPositon()
    {
        //if is occupied
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public void SetSelectedHero(Vector2 position)
    {

        if (isCancleSelected) submitHeroData = null;
        else
        {
            var allHerosInScene = GetSameSideHerosInTheScene();

            foreach (var hero in allHerosInScene)
            {
                if (position.x == hero.transform.position.x && position.y == hero.transform.position.y)
                {

                    //isHeroSubmited = true;
                    var heroData = hero.GetComponent<HeroData>();
                    submitHeroData = heroData;
                    Debug.Log("Set selected hero" + heroData.name);
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
        //var allHerosInScene = HeroPocketManager.Instance.GetAllHeroes();
        var allHerosInScene = GetSameSideHerosInTheScene();
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

    public List<GameObject> GetSameSideHerosInTheScene()
    {
        if (GameManager.Instance.currentTurn == GameManager.PlayerTurn.PlayerRedSide)
        {
            var heros = HeroPocketManager.Instance.GetAllRedSideHeroes();
            return heros;
        }
        else
        {
            var heros = HeroPocketManager.Instance.GetAllBlueSideHeroes();
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

                /*                Debug.Log("Target hero selected: " + battleManager.targetHero.name);
                                Debug.Log("Attacking with hero: " + battleManager.currentHero.name);*/
                //end of Killians code
            }
        }
        Debug.Log("kill him!!!!!!!");
    }

    internal void ActiveShopMenu()
    {
        gameStateMachine.SwitchToShopState();

    }

    internal void CancleSelected()
    {
        Debug.Log("cancle selected");
        isCancleSelected = true;
        hideHighlight?.Invoke();
        var position = GetIndicatorPositon();
        SetSelectedHero(position);
        isHeroSubmited = false;
    }

    internal void HandleAttack()
    {
        CheckAutoAttack();
        if (isAutoAttack) return;
        /* Debug.Log("CheckAttackRange");
         Debug.Log("occupied count"+ GridManager.Instance.occupiedGrids.Count);*/
        currentGridPosition = WorldToGridPosition(transform.position);
        Vector2Int[] validAttackRangePositions = highLight.GetNeighborsForAbilityRange(GetSelectedHeroPositon(), currentSelectedHeroId);
        /*   
           Debug.Log("CheckAttackRange" + validAttackRangePositions.Length);
           Debug.Log("Valid Target Positions: " + string.Join(", ", validAttackRangePositions));*/
        List<GameObject> heroesOpposite = HeroPocketManager.Instance.GetOppositeHeros();//all the enemy
        //Debug.Log("occupied count" + heroesOpposite.Count);
        foreach (var hero in heroesOpposite)
        {

            if (validAttackRangePositions.Contains(WorldToGridPosition(hero.transform.position)))
            {
                /*   Debug.Log("enemy there");
                   Debug.Log(" attack happen");*/
                //set target hero
                hideHighlight?.Invoke();//hide the high light
                var targetHero = hero.GetComponent<HeroData>();
                var currentHero = submitHeroData;
                StartCoroutine(RollDiceAndApplyDamage(targetHero));
            }
            else
            {
                Debug.Log("clear");

                gameStateMachine.SwitchToMoveHeroState();
            }
        }


    }

    public void CheckHealRange()
    {
        Debug.Log("hero My hero");
    }

}
