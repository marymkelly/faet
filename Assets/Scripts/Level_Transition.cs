using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Make sure to add this, or you can't use SceneManager
using UnityEngine.SceneManagement;


public class Level_Transition : MonoBehaviour
{
    public int sceneIndex;
    private GameObject levelLoader;
    
    void Start() {
        levelLoader = GameObject.Find("LevelLoader").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //other.name should equal the root of your Player object
        if (other.name == "Player")
        {
            if(SceneManager.GetActiveScene().name == "Credits_Screen") {
                StartCoroutine(levelLoader.GetComponent<LevelLoader>().LoadLevel(sceneIndex));
            } else {
                // if(SceneManager.GetActiveScene().name != "Credits_Screen") {
                    GameObject.Find("GameData").GetComponent<GameData>().Save();
                // }

                if(sceneIndex == 6 && other.GetComponent<Player_Interactions>().defeatedBossTwo) {
                    sceneIndex = 7;
                }
                //The scene number to load (in File->Build Settings)
                // SceneManager.LoadScene(sceneIndex);
                levelLoader.GetComponent<LevelLoader>().StartLoadingLevel(sceneIndex);
            }

        }
    }
}