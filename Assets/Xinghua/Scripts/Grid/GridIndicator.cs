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

    [SerializeField] private GameObject heroPrefab;
    // private Vector2 currentGridPosition;
    //[SerializeField] private GameObject startPosition;//hero spawn front of the gate
    [SerializeField] HeroSelect heroSelect;
    public event Action finishSelection;
    public event Action heroSelecting;

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
    private bool IsWithinBounds(Vector2Int position)
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
        Vector2Int intDirection = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
        Vector2Int targetPosition = currentGridPosition + intDirection;
        if (IsWithinBounds(targetPosition))
        {
            Debug.Log("move indicator");
            currentGridPosition = targetPosition;
            transform.position += new Vector3(direction.x, direction.y, 0);
            newIndicatorLocation = transform.position;
            heroSelecting?.Invoke();//this if for path highlight to listen
        }
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

        if (GetPlayerTurn() == PlayerTurn.PlayerBlueSide)
        {
            playerBlueHero.transform.position = transform.position;//PlayerBlue Hero need Dynamic from array
        }
        else
        {
            playerRedHero.transform.position = transform.position;//PlayerRedHero need Dynamic
        }
        UpdateIndicatorPosition();
    }
    private PlayerTurn GetPlayerTurn()
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

    private void UpdateIndicatorPosition()
    {

        if (currentTurn == PlayerTurn.PlayerBlueSide)
        {
            transform.position = playerRedHero.transform.position;
        }
        else
        {
            transform.position = playerBlueHero.transform.position;
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


}
