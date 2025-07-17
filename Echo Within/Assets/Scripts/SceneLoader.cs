using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Exact name of the scene to load.")]
    public string sceneToLoad;

    public void LoadSceneByName()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name not set in SceneLoader script on " + gameObject.name);
        }
    }
    public void ExitGame()
    {
        Debug.Log("Quitting Game...");

        #if UNITY_EDITOR
            // For Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // For build
            Application.Quit();
        #endif
    }
}
