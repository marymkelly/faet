using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private DialogueUI dialogueUI;
    public Text statusText;
    public GameObject saveButton;
    public GameObject audioButton;
    private AudioManager audioManager;
    

    void Start() {
        if(SceneManager.GetActiveScene().name == "Boss_One" || SceneManager.GetActiveScene().name == "Boss_Two") {
            saveButton.SetActive(false);
        }

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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !(GameObject.FindWithTag("Player").GetComponent<Player_Interactions>().controlsOpen || GameObject.FindWithTag("Player").GetComponent<Player_Interactions>().defeatOn)) {
            if(GameObject.FindWithTag("Player").GetComponent<Player_Interactions>().isInteracting) {
                dialogueUI.CloseDiologueBox();
            } else {
                if(GameIsPaused) {
                    Resume();
                } else {
                    Pause();
                }
            }
        }
    }

    public void Resume() {
        GameObject.FindWithTag("Player").GetComponent<Player_Interactions>().isInteracting = false;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause() {
        GameObject.FindWithTag("Player").GetComponent<Player_Interactions>().isInteracting = true;
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowControls() {
        GameObject.Find("Player").GetComponent<Player_Interactions>().ViewControls();
    }

    public void LoadMenu() {  //to main screen
        Debug.Log("Loading menu...");
        statusText.text = "Loading menu...";
        statusText.gameObject.SetActive(true);
        Destroy(GameData.control);
        SceneManager.LoadScene("Start_Screen"); //had created variable for pages
    } 

    public void Save() {
        GameObject.Find("GameData").GetComponent<GameData>().Save();

        statusText.text = $"Last saved at <color=#00dba0>{GameObject.Find("GameData").GetComponent<GameData>().lastSaved.ToString()}</color>";
        statusText.gameObject.SetActive(true);
        StartCoroutine("StatusTimer");   
    }

    public void Audio() {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ToggleMute(!audioButton.GetComponent<Toggle>().isOn);
    }
    
    public void QuitGame() {
        Debug.Log("Quitting game...");
        statusText.text = "Quitting...";
        statusText.gameObject.SetActive(true);
        Application.Quit(); //does not quit inside editor
    }

    IEnumerator StatusTimer() {
        yield return new WaitForSeconds(5);
        statusText.gameObject.SetActive(false);
    }
}
