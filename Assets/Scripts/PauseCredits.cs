using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseCredits : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public GameObject audioButton;
    private AudioManager audioManager;
    private bool paused = false;

    void Start() {
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


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(paused) {
                    Resume();
                } else {
                    Pause();
            }
        }
    }

    public void MainMenu() {  //to main screen
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start_Screen"); //had created variable for pages
    } 

    public void Resume() {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause() {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame() {
        Application.Quit(); //does not quit inside editor
    }

    public void Audio() {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ToggleMute(!audioButton.GetComponent<Toggle>().isOn);
    }
}
