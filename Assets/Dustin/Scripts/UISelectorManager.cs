using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISelectorManager : MonoBehaviour
{
    public static UISelectorManager Instance;
    public Button[] buttons; // Assign buttons in order
    public RectTransform selector; // Assign the selector GameObject
    private int currentIndex = 0; // Tracks the currently selected button

    private IEnumerator FixSelectorPosition()
    {
        yield return new WaitForEndOfFrame(); // Ensures UI layout is finalized
        UpdateSelectorPosition();
    }

    private void Start()
    {
        if (buttons.Length == 0)
        {
            Debug.LogWarning("[UISelector] No buttons assigned!");
            return;
        }

        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector GameObject is not assigned!");
            return;
        }

        // Debug.Log("[UISelector] Script initialized. Starting at button index: " + currentIndex);
        UpdateSelectorPosition(); // Position the selector on the first button
        StartCoroutine(FixSelectorPosition()); // Ensures it updates after UI layout settles
    }

    private void Update()
    {
       // HandleNavigation(); // Move selector
       // HandleSelection(); // Select the current button
    }

    public void HandleNavigation(Vector2 direction)
    {
        if (direction.x > 0)
        {
            MoveSelector(1); // Move to the next button
        }
        else if (direction.x < 0)
        {
            MoveSelector(-1); // Move to the previous button
        }
        /* if (Input.GetKeyDown(KeyCode.RightArrow))
         {
             MoveSelector(1); // Move right
         }
         else if (Input.GetKeyDown(KeyCode.LeftArrow))
         {
             MoveSelector(-1); // Move left
         }*/
    }
    
    public void MoveSelector(int direction)
    {
        if (buttons.Length == 0) return;

        currentIndex = (currentIndex + direction + buttons.Length) % buttons.Length; // Wrap around
        // Debug.Log("[UISelector] Moved to button index: " + currentIndex + " (" + buttons[currentIndex].name + ")");
        UpdateSelectorPosition();
    }

    private void UpdateSelectorPosition()
    {
        if (selector == null)
        {
            Debug.LogWarning("[UISelector] Selector is missing!");
            return;
        }

        if (buttons.Length > 0)
        {
            selector.position = buttons[currentIndex].transform.position;
            // Debug.Log("[UISelector] Selector moved to: " + selector.position);
        }
    }

    private void HandleSelection()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[UISelector] Selected button: " + buttons[currentIndex].name);
            buttons[currentIndex].onClick.Invoke();
        }
    }
}
