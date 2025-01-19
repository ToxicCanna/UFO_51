using UnityEngine;

public class HighLight : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private float tileSize = 1f;

    private GameObject[] highlights = new GameObject[4];
    private Vector3[] directions;
    private int selectedDirection = 0;

    void Start()
    {
        // initialize 
        directions = new Vector3[]
        {
            Vector3.up * tileSize,
            Vector3.down * tileSize,
            Vector3.left * tileSize,
            Vector3.right * tileSize
        };

        for (int i = 0; i < 4; i++)
        {
            highlights[i] = Instantiate(highlightPrefab);
            highlights[i].SetActive(false);
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
        Vector3 playerPosition = transform.position;
        for (int i = 0; i < highlights.Length; i++)
        {
            Vector3 targetPosition = playerPosition + directions[i];
            highlights[i].transform.position = targetPosition;
            highlights[i].SetActive(CanMoveTo(targetPosition));

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
}
