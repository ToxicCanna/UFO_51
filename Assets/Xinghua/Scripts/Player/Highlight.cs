using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private float tileSize = 1f;

    public HeroMovementRule[] currentRule;//this rule is for player move path generate
    public List<Vector3> directions;
    [SerializeField] private GridIndicator gridIndicator;
    private List<GameObject> highlights = new List<GameObject>();
    void Start()
    {
        if (currentRule != null)
        {
            SetHeroRule(currentRule[0]);
        }
        else
        {
            Debug.LogError("currentRule is not assigned in the Inspector!");
        }

    }

    private void OnEnable()
    {
        if (gridIndicator != null)
        {
            gridIndicator.finishSelection += OnHeroSelectionFinished;
            gridIndicator.heroSelecting += OnHeroSelecting;
        }
        else
        {
            Debug.Log("gridIndicator is null");
        }
    }

    private void OnDisable()
    {
        if (gridIndicator != null)
        {
            gridIndicator.finishSelection -= OnHeroSelectionFinished;
            gridIndicator.heroSelecting -= OnHeroSelecting;
        }
    }
    private void OnHeroSelecting()
    {
        Debug.Log("show highlight path");
        Vector2Int currentGridPosition = GetGridPosition(transform.position);
        Vector2Int[] neighbors = GetNeighbors(currentGridPosition);
        DisplayHightlight(neighbors);

    }
    private void OnHeroSelectionFinished()
    {
        Debug.Log("show highlight path");

        HideHightlight();

    }
    private void HideHightlight()
    {
        foreach (var hightlight in highlights)
        {
            hightlight.gameObject.SetActive(false);

        }
    }
    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tileSize);
        int y = Mathf.RoundToInt(worldPosition.y / tileSize);
        return new Vector2Int(x, y);
    }
    private void DisplayHightlight(Vector2Int[] neighbors)
    {
        ClearHighlights();

        foreach (var neighbor in neighbors)
        {
            Vector3 neighborPosition = AlignToGrid(new Vector3(neighbor.x, neighbor.y, 0));
            GameObject highlight = Instantiate(highlightPrefab, neighborPosition, Quaternion.identity);
            highlight.gameObject.SetActive(true);
            highlights.Add(highlight);
        }
    }
    private void ClearHighlights()
    {
        foreach (var highlight in highlights)
        {
            Destroy(highlight);
        }
        highlights.Clear();
    }
    public Vector2Int[] GetNeighbors(Vector2Int currentPosition)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();


        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(0, 1),  
        new Vector2Int(0, -1), 
        new Vector2Int(-1, 0), 
        new Vector2Int(1, 0)  
        };


        foreach (var direction in directions)
        {
            neighbors.Add(currentPosition + direction);
            //neighbors.Add(currentPosition + direction * 2);
            //neighbors.Add(currentPosition + direction * 3);
        }

        return neighbors.ToArray();
    }
    private Vector3 AlignToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / tileSize) * tileSize;
        float y = Mathf.Round(position.y / tileSize) * tileSize;
        return new Vector3(x, y, position.z);
    }
    public void SetHeroRule(HeroMovementRule rule)
    {
        if (rule == null)
        {
            Debug.LogError("HeroMovementRule is null!");
            return;
        }
        currentRule[0] = rule;//this will make the highligh path different


        for (int i = 0; i < highlights.Count; i++)
        {
            highlights[i] = Instantiate(highlightPrefab);
            highlights[i].SetActive(false);
        }
    }
    void DisplayNeighbors(Vector2Int currentGridPosition)
    {
        Vector2Int[] neighbors = GetNeighbors(currentGridPosition);

        foreach (var neighbor in neighbors)
        {

            Vector3 neighborPosition = new Vector3(neighbor.x, neighbor.y, 0);


            GameObject indicator = Instantiate(highlightPrefab, neighborPosition, Quaternion.identity);


            indicator.transform.localScale = Vector3.one;
        }
    }

    private void UpdateHighlight()
    {
        ClearHighlights();

        Vector2Int currentGridPosition = GetGridPosition(transform.position);
        Vector2Int[] neighbors = GetNeighbors(currentGridPosition);

        foreach (var neighbor in neighbors)
        {
            Vector3 neighborPosition = AlignToGrid(new Vector3(neighbor.x, neighbor.y, 0));
            GameObject highlight = Instantiate(highlightPrefab, neighborPosition, Quaternion.identity);
            highlights.Add(highlight);
        }
    }





}
