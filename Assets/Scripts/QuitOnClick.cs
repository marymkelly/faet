using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitOnClick : MonoBehaviour {
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        if(Application.platform == RuntimePlatform.WebGLPlayer) {
            SceneManager.LoadScene("Start_Screen");
        } else {
            Application.Quit();
        }
#endif
    }
}
