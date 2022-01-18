using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour  //allows ability to add tutorial name and sprite in editor
{
    public Tutorial[] tutorials;
    public void SetTutorial(string name) {
        Tutorial t = Array.Find(tutorials, tutorial => tutorial.name == name);
        if(t == null) {
            Debug.Log("Tutorial " + name + "was not found");
        } else {
            gameObject.GetComponent<Image>().sprite = t.source;
        }
    }
}


[System.Serializable]
public class Tutorial
{
    public string name;
    public Sprite source;
}