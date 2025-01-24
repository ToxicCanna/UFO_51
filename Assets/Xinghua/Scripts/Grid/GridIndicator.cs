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
    [SerializeField] private Vector2 gridOrigin;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private float tileSize = 1f;
    private Vector2 currentGridPosition;
    //[SerializeField] private GameObject startPosition;//hero spawn front of the gate
    [SerializeField] HeroSelect heroSelect;
    public event Action finishSelection;

    private Vector3 heroPosition;

    void Start()
    {
        Debug.Log($"Grid Origin: {gridOrigin}, Tile Size: {tileSize}");
        //should get the current player position
        Vector3 heroPosition = playerRedHero.transform.position;
        currentGridPosition = new Vector2(
            Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
            Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
        );
        transform.position = playerRedHero.transform.position;
    }

    void Update()
    {
        UpdateIndicatorPosition();
    }

    public void HandleConfirmInput()
    {
        Debug.Log("swith in the hero array ");//swith in the hero array I already buy then confirm one to move
        UpdateIndicatorToSelectedHero();
    }

    private void UpdateIndicatorToSelectedHero()
    {
        finishSelection?.Invoke();
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            var currentHeroPosition = heroSelect.GetSelectedHeroPosition();
            currentHeroPosition = transform.position;
            currentTurn = PlayerTurn.PlayerBlueSide;
            Vector3 heroPosition = playerBlueHero.transform.position;
            currentGridPosition = new Vector2(
                Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
                Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
            );

            Debug.Log($"current player is:{currentTurn}");

        }
        else if (currentTurn == PlayerTurn.PlayerBlueSide)
        {
            Vector3 heroPosition = playerRedHero.transform.position;
            currentGridPosition = new Vector2(
                Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
                Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
            );

            playerBlueHero.transform.position = transform.position;
            currentTurn = PlayerTurn.PlayerRedSide;
            //transform.position = playerRedHero.transform.position;
            Debug.Log($"current player is:{currentTurn}");
        }

    }
    public void HandleInput(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            MoveIndicator(direction);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentTurn == PlayerTurn.PlayerRedSide)
            {

                playerRedHero.transform.position = transform.position;
                //currentTurn = PlayerTurn.PlayerBlueSide;
                UpdateHeroPosition();
                heroPosition = playerBlueHero.transform.position;
                currentGridPosition = new Vector2(
                    Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
                    Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
                );

                Debug.Log($"current player is:{currentTurn}");

            }
            else if (currentTurn == PlayerTurn.PlayerBlueSide)
            {
                UpdateHeroPosition();
                //heroPosition = playerRedHero.transform.position;
                currentGridPosition = new Vector2(
                    Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
                    Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
                );

                playerBlueHero.transform.position = transform.position;
                currentTurn = PlayerTurn.PlayerRedSide;
                //transform.position = playerRedHero.transform.position;
                Debug.Log($"current player is:{currentTurn}");
            }
        }
    }
    private void UpdateHeroPosition()
    {
        heroPosition = heroSelect.GetSelectedHeroPosition();
    }
    public void ConfirmMovePosition()
    {
        finishSelection?.Invoke();
        if (currentTurn == PlayerTurn.PlayerRedSide)
        {
            playerRedHero.transform.position = transform.position;//this should be the current in the array
            currentTurn = PlayerTurn.PlayerBlueSide;
            Vector3 heroPosition = playerBlueHero.transform.position;
            currentGridPosition = new Vector2(
                Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
                Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
            );
            GridManager.Instance.AddOccupiedGrid(currentGridPosition);

            Debug.Log($"current player is:{currentTurn}");

        }
        else if (currentTurn == PlayerTurn.PlayerBlueSide)
        {
            Vector3 heroPosition = playerRedHero.transform.position;
            currentGridPosition = new Vector2(
                Mathf.Round((heroPosition.x - gridOrigin.x) / tileSize),
                Mathf.Round((heroPosition.y - gridOrigin.y) / tileSize)
            );

            playerBlueHero.transform.position = transform.position;
            currentTurn = PlayerTurn.PlayerRedSide;
            //transform.position = playerRedHero.transform.position;
            Debug.Log($"current player is:{currentTurn}");
        }
    }
    private void MoveIndicator(Vector2 direction)
    {
        Vector2 newPosition = currentGridPosition + direction;
        if (newPosition.x >= gridOrigin.x && newPosition.x < gridSize.x + gridOrigin.x &&
            newPosition.y >= gridOrigin.y && newPosition.y < gridSize.y + gridOrigin.y)
        {
            currentGridPosition = newPosition;
        }
    }

    public void UpdateIndicatorPosition()
    {
        Vector3 worldPosition = new Vector3(currentGridPosition.x * tileSize + +gridOrigin.x, currentGridPosition.y * tileSize + gridOrigin.y, 0);
        transform.position = worldPosition;
    }

}
