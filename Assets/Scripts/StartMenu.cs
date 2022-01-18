using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{   
    public GameObject startButton;
    // public GameObject loadButton;
    public GameObject returningBtns;


    void Start() {
        startButton.SetActive(!GameData.existingData);
        returningBtns.SetActive(GameData.existingData);
    }

    void Update()
    {
        // loadButton.GetComponent<Button>().interactable = GameData.existingData;
    }
}
