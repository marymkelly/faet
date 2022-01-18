using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        optCanvas.SetActive(true);
    }
}
