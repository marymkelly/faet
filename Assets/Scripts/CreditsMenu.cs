using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    public GameObject optCanvas;

    void Start()
    {
        StartCoroutine("ShowOptions");
    }

    IEnumerator ShowOptions()
    {    
        yield return new WaitForSeconds(32);
        
        if(Application.platform == RuntimePlatform.WebGLPlayer) {
            SceneManager.LoadScene("Start_Screen");
        } else {
            optCanvas.SetActive(true);
        }
    }
}
