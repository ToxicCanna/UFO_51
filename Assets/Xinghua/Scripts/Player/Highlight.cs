﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HighLight : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private float tileSize = 1f;

    private GameObject[] highlights = new GameObject[4];

    public HeroMovementRule currentRule;
    public List<Vector3> directions;

    public void SetHeroRule(HeroMovementRule rule)
    {
        if (rule == null)
        {
            Debug.LogError("HeroMovementRule is null!");
            return;
        }
        currentRule = rule; 
        UpdateDirections(); 

        
        highlights = new GameObject[directions.Count];
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i] = Instantiate(highlightPrefab);
            highlights[i].SetActive(false);
        }
    }
    private void UpdateDirections()
    {
        directions = new List<Vector3>();

        for (int i = 1; i <= currentRule.upSteps; i++) directions.Add(new Vector3(0, i * tileSize, 0));
        for (int i = 1; i <= currentRule.downSteps; i++) directions.Add(new Vector3(0, -i * tileSize, 0));
        for (int i = 1; i <= currentRule.leftSteps; i++) directions.Add(new Vector3(-i * tileSize, 0, 0));
        for (int i = 1; i <= currentRule.rightSteps; i++) directions.Add(new Vector3(i * tileSize, 0, 0));

       
        for (int i = 1; i <= currentRule.diagonalSteps; i++)
        {
            directions.Add(new Vector3(i * tileSize, i * tileSize, 0)); 
            directions.Add(new Vector3(-i * tileSize, i * tileSize, 0)); 
            directions.Add(new Vector3(i * tileSize, -i * tileSize, 0));
            directions.Add(new Vector3(-i * tileSize, -i * tileSize, 0)); 
        }
    }
    void Start()
    {
        if (currentRule != null)
        {
            SetHeroRule(currentRule);
        }
        else
        {
            Debug.LogError("currentRule is not assigned in the Inspector!");
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
        if (highlights == null || directions == null || highlights.Length == 0 || directions.Count == 0)
        {
            Debug.LogWarning("Highlights or directions are not initialized.");
            return;
        }
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
