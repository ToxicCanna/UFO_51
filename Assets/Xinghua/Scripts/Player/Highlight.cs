using UnityEngine;

public class HighLight : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private float tileSize = 1f;

    private GameObject[] highlights = new GameObject[4];
    private Vector3[] directions;


    void Start()
    {
        // initialize 
        directions = new Vector3[]
        {
            Vector3.up * tileSize,
            Vector3.down * tileSize,
            Vector3.left * tileSize,
            Vector3.right * tileSize,

            new Vector3(-1, 1, 0).normalized * tileSize,
            new Vector3(1, 1, 0).normalized * tileSize,
            new Vector3(-1, -1, 0).normalized * tileSize,
            new Vector3(1, -1, 0).normalized * tileSize
        };
        highlights = new GameObject[directions.Length];
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i] = Instantiate(highlightPrefab);
            highlights[i].SetActive(true);
        }
    }
    private void Update()
    {
        UpdateHighlights();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            highlightPrefab.SetActive(false);
        };
    }
    private void UpdateHighlights()
    {
        //inside of if() should be the player choose some hero already
        if (Input.anyKeyDown)
        {
            Vector3 playerPosition = transform.position;
            for (int i = 0; i < highlights.Length; i++)
            {
                Vector3 targetPosition = AlignToGrid(playerPosition + directions[i]);
                highlights[i].transform.position = targetPosition;
                Debug.Log("can move to target" + CanMoveTo(targetPosition));
                highlights[i].SetActive(CanMoveTo(targetPosition));

            }
        }

    }

    private bool CanMoveTo(Vector3 targetPosition)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, 0.1f, gridLayer);
        return hit == null;
    }
    private void HideIndicators()
    {
        foreach (var highlight in highlights)
        {
            highlight.SetActive(false);
        }

    }
    private Vector3 AlignToGrid(Vector3 position)
    {
        
        float x = Mathf.Round(position.x / tileSize) * tileSize;
        float y = Mathf.Round(position.y / tileSize) * tileSize;
        return new Vector3(x, y, position.z);
    }
}
