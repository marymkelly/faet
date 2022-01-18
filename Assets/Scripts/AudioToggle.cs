using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    public GameObject audioButton;
    private AudioManager audioManager;
    
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if(audioManager) {
            if(!audioManager.AudioIsPlaying){
                audioButton.SetActive(false);
            }
            else{
                audioButton.GetComponent<Toggle>().isOn = !audioManager.isMuted;
            }
        } 
    }

    public void Audio() {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ToggleMute(!audioButton.GetComponent<Toggle>().isOn);
    }
}
