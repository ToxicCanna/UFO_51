using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadScene(string name)
    {
        Time.timeScale = 1.0f;
        StartCoroutine(ReinitializeAfterSceneLoad());
    }

    private IEnumerator ReinitializeAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.1f); 

        GridIndicator gridIndicator = FindAnyObjectByType<GridIndicator>();
        if (gridIndicator == null)
        {
            Debug.LogError("GridIndicator is still null after scene reload!");
        }
        else
        {
            Debug.Log("GridIndicator successfully reinitialized after replay.");
        }

       // HandleInput(InputManager.Instance); 
    }


    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }



}
