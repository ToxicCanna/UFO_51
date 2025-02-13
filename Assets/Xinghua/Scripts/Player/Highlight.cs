﻿using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private GameObject abilityTangeTilePerfab;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private float tileSize = 1f;
    public HeroMovementRule[] currentRule;//this rule is for player move path generate
    Vector2Int[] neighbors;
    Vector2Int[] neighborsForAbilityRange;
    [SerializeField] private GridIndicator gridIndicator;
    private List<GameObject> highlights = new List<GameObject>();
    private List<GameObject> highlightsForAbilityRange = new List<GameObject>();

    private void OnEnable()
    {
        if (gridIndicator != null)
        {
            gridIndicator.hideHighlight += HideAllHightlights;
            gridIndicator.onHeroPositon += ShowHeroPath;//this will show the targt path and ability range both ; in different color

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
            gridIndicator.hideHighlight -= HideAllHightlights;
            gridIndicator.onHeroPositon -= ShowHeroPath;

        }
    }

    public void ShowHeroPath()
    {
       
        Vector2Int currentGridPosition = GetGridPosition(gridIndicator.transform.position);//this position need been hero selected 
        var heros = gridIndicator.GetSameSideHerosInTheScene();
        foreach (var hero in heros)
        {
            var index = hero.gameObject.GetComponent<HeroPath>().heroPathID;
            if (gridIndicator.transform.position == hero.transform.position)
            {
                neighbors = GetNeighbors(currentGridPosition, index);
                DisplayHightlight(neighbors);

                if (index == 3 || index == 4)
                {
                    neighborsForAbilityRange = GetNeighborsForAbilityRange(currentGridPosition, index);
                    DisplayRangedAbility(neighborsForAbilityRange);
                }
            }
        }
    }

    private void ClearHighlights()
    {
        foreach (var highlight in highlights)
        {
            Destroy(highlight.gameObject);
        }
        highlights.Clear();
    }

    private void ClearAbilityRangeDisplay()
    {
        foreach (var abilityTangeTile in highlightsForAbilityRange)
        {
            Destroy(abilityTangeTile.gameObject);
        }
        highlightsForAbilityRange.Clear();
    }

    private void DisplayHightlight(Vector2Int[] neighbors)
    {
        ClearHighlights();
        foreach (var neighbor in neighbors)
        {
            if (gridIndicator.IsWithinBounds(neighbor))
            {
                Vector3 neighborPosition = AlignToGrid(new Vector3(neighbor.x, neighbor.y, 0));
                GameObject highlight = Instantiate(highlightPrefab, neighborPosition, Quaternion.identity);
                highlight.gameObject.SetActive(true);
                highlights.Add(highlight);
            }
        }
    }

    private void DisplayRangedAbility(Vector2Int[] neighbors)
    {
        ClearAbilityRangeDisplay();
        foreach (var neighbor in neighbors)
        {
            Debug.Log("show the range highlight");
            Debug.Log(" range highlight:" + neighbors.Length);
            if (gridIndicator.IsWithinBounds(neighbor))
            {
                Vector3 neighborPosition = AlignToGrid(new Vector3(neighbor.x, neighbor.y, 0));
                GameObject highlight = Instantiate(abilityTangeTilePerfab, neighborPosition, Quaternion.identity);
                highlight.gameObject.SetActive(true);
                highlightsForAbilityRange.Add(highlight);

            }
        }
    }

    public void HideAllHightlights()
    {
        Debug.Log("hide highlights");
        HideHightlightMovePath();
        HideHightlightForAbility();
    }

    private void HideHightlightMovePath()
    {
        ClearHighlights();
        foreach (var hightlight in highlights)
        {
            hightlight.gameObject.SetActive(false);

        }
    }
    private void HideHightlightForAbility()
    {
        ClearAbilityRangeDisplay();
        foreach (var hightlight in highlightsForAbilityRange)
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

    public Vector2Int[] GetNeighborsForAbilityRange(Vector2Int currentPosition, int ID)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (ID == 4)
        {
       
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    neighbors.Add(new Vector2Int(currentPosition.x + x, currentPosition.y + y));
                }
            }
        }
        else if (ID == 3)
        {
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
             };
            foreach (var direction in directions)
            {
                neighbors.Add(currentPosition + direction);
                neighbors.Add(currentPosition + direction * 2);
                neighbors.Add(currentPosition + direction * 3);
            }
        }
        return neighbors.ToArray();
    }

    //this is for the move
    public Vector2Int[] GetNeighbors(Vector2Int currentPosition, int ID)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
   

        if (ID == 0 || ID == 4)
        {
            Vector2Int[] directions = new Vector2Int[]
         {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),

                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)
         };

            foreach (var direction in directions)
            {
                neighbors.Add(currentPosition + direction);

            }
        }
        if (ID == 1)
        {
            Vector2Int[] directions = new Vector2Int[]
             {
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),

                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)
             };

            foreach (var direction in directions)
            {
                neighbors.Add(currentPosition + direction);

            }
        }
        else if (ID == 2)
        {
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
                 new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)


            };
            foreach (var direction in directions)
            {
                neighbors.Add(currentPosition + direction);
                if (direction.x == 0 || direction.y == 0)
                {
                    neighbors.Add(currentPosition + direction * 2);
                }
            }
        }
        else if (ID == 3)
        {
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
            };
            foreach (var direction in directions)
            {
                neighbors.Add(currentPosition + direction);
            }
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
        var id = gridIndicator.GetSubmitHeroPathIndex(currentGridPosition);
        Vector2Int[] neighbors = GetNeighbors(currentGridPosition, id);

        foreach (var neighbor in neighbors)
        {

            Vector3 neighborPosition = new Vector3(neighbor.x, neighbor.y, 0);


            GameObject indicator = Instantiate(highlightPrefab, neighborPosition, Quaternion.identity);


            indicator.transform.localScale = Vector3.one;
        }
    }

}
