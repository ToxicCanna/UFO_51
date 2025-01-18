using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class GridIndicator : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private Vector2 gridSize = new Vector2(-100, 100); 
    [SerializeField] private float tileSize = 1f; 
    private Vector2 currentGridPosition; 

    void Start()
    {
        //should get the current player position
        transform.position = heroPrefab.transform.position;
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
            Vector3 targetPosition = transform.position;
            heroPrefab.transform.position = targetPosition;
        }
    }

    private void MoveIndicator(Vector2 direction)
    { 
        Vector2 newPosition = currentGridPosition + direction;  
        if (newPosition.x >= 0 && newPosition.x < gridSize.x &&
            newPosition.y >= 0 && newPosition.y < gridSize.y)
        {
            currentGridPosition = newPosition; 
        }
    }

    private void UpdateIndicatorPosition()
    {
        Vector3 worldPosition = new Vector3(currentGridPosition.x * tileSize, currentGridPosition.y * tileSize, 0);
        transform.position = worldPosition;
    }
}
