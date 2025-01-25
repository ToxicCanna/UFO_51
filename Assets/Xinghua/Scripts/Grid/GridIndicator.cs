using System;
using UnityEngine;


public class GridIndicator : MonoBehaviour
{
    // [SerializeField] private List<ScriptableObject> items; 

    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;

    [SerializeField] private GameObject playerRedHero;
    [SerializeField] private GameObject playerBlueHero;

    [SerializeField] private GameObject heroPrefab;
    private Vector2 currentGridPosition;
    //[SerializeField] private GameObject startPosition;//hero spawn front of the gate
    [SerializeField] HeroSelect heroSelect;
    public event Action finishSelection;

    private Vector3 heroPosition;
    private Vector3 newIndicatorLocation;

    void Start()
    {
        transform.position = playerRedHero.transform.position;
        currentTurn = PlayerTurn.PlayerRedSide;
    }

    //private void UpdateIndicatorToSelectedHero()
    //{
    //    var currentHeroPosition = heroSelect.GetSelectedHeroPosition();
    //}
  

    public void HandleIndicatorMove(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, direction.y, 0);
        //newIndicatorLocation = transform.position + new Vector3(direction.x+1,direction.y,0);
    }

    //private void UpdateHeroPosition()
    //{
    //    heroPosition = heroSelect.GetSelectedHeroPosition();
    //}
    public void MoveToTargetIndicator()
    {
        //Debug.Log("ConfirmMovePosition");
        finishSelection?.Invoke();//this if for path highlight to listen
        //GridManager.Instance.AddOccupiedGrid(newIndicatorLocation);
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




}
