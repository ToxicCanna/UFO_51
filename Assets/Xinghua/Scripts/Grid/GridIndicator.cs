﻿using System;
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
            Debug.Log("currentGridPosition after move" + currentGridPosition);
            transform.position += new Vector3(direction.x, direction.y, 0);
            //judge if this position have hero already
            var heros = GridManager.Instance.GetHeros();
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
           currentTurn =PlayerTurn.PlayerRedSide;
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
        Debug.Log("use X key to switch");
        Debug.Log("current indicator position"+ transform.position);
        Debug.Log("current turn" + GetCurrentPlayerTurn());
      


    }

}
