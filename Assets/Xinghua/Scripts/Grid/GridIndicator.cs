using System.Collections.Generic;
using UnityEngine;


public class GridIndicator : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> items; 
    private int currentIndex = 0;
    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;

    [SerializeField] private GameObject playerRedHero;
    [SerializeField] private GameObject playerBlueHero;
    
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private Vector2 gridOrigin = new Vector2(-10, -10);
    [SerializeField] private Vector2 gridSize = new Vector2(20, 20);
    [SerializeField] private float tileSize = 1f;
    private Vector2 currentGridPosition;
    [SerializeField] private GameObject startPosition;


    void Start()
    {
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
        HandleInput();
        UpdateIndicatorPosition();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) MoveIndicator(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) MoveIndicator(Vector2.down);
        if (Input.GetKeyDown(KeyCode.A)) MoveIndicator(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) MoveIndicator(Vector2.right);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentTurn == PlayerTurn.PlayerRedSide)
            {
                playerRedHero.transform.position = transform.position;
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

    private void UpdateIndicatorPosition()
    {
        Vector3 worldPosition = new Vector3(currentGridPosition.x * tileSize + +gridOrigin.x, currentGridPosition.y * tileSize + gridOrigin.y, 0);
        transform.position = worldPosition;
    }

}
