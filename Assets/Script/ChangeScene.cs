using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public void switchscene()
    {
        SceneManager.LoadScene(sceneName: "SampleScene");
        GameObject.Find("LastQRCode").GetComponent<Renderer>().enabled = false;
    }
}
