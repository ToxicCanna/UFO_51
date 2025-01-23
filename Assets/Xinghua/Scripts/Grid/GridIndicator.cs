using UnityEngine;


public class GridIndicator : MonoBehaviour
{
    // [SerializeField] private List<ScriptableObject> items; 

    public enum PlayerTurn { PlayerRedSide, PlayerBlueSide }
    public PlayerTurn currentTurn = PlayerTurn.PlayerRedSide;

    [SerializeField] private GameObject playerRedHero;
    [SerializeField] private GameObject playerBlueHero;

    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private Vector2 gridOrigin = new Vector2(-10, -10);
    [SerializeField] private Vector2 gridSize = new Vector2(20, 20);
    [SerializeField] private float tileSize = 1f;
    private Vector2 currentGridPosition;
    //[SerializeField] private GameObject startPosition;//hero spawn front of the gate


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
        UpdateIndicatorPosition();
    }

    public void HandleInput(Vector2 direction)
    {
        if(direction!=Vector2.zero)
        {
            MoveIndicator(direction);
        }

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
    public void ConfirmMovePosition()
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
