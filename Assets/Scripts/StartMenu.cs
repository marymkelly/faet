using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{   
    public GameObject startButton;
    public GameObject returningBtns;
    public GameObject quitButton;

    void Start() {
        startButton.SetActive(!GameData.existingData);
        returningBtns.SetActive(GameData.existingData);

        if(Application.platform == RuntimePlatform.WebGLPlayer) {
            quitButton.SetActive(false);
        }
    }
}
