using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get count of loaded Scenes and last index
        var lastSceneIndex = SceneManager.sceneCount - 1;

        // Get last Scene by index in all loaded Scenes
        var lastLoadedScene = SceneManager.GetSceneAt(lastSceneIndex);

// Unload Scene
        SceneManager.UnloadSceneAsync(lastLoadedScene);
    }

}
