using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelSetup : MonoBehaviour
{
    private GameObject woodsContainer;
    private SpriteRenderer[] woods = new SpriteRenderer[2];
    private string sceneName;
    private bool fadeWoods = false;
    private float t = 1f;

    private GameObject dynamicBg;
    private GameObject player;
    private GameObject[] hazards;
    private GameObject[] enemies;

    private GameObject extensionPlatform;

    void Awake() {
        sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "Level_One" || sceneName == "Level_One_Second") {
            extensionPlatform = GameObject.Find("Extension").gameObject;
            dynamicBg = GameObject.Find("Background Layers").gameObject;
            dynamicBg.SetActive(false);
            extensionPlatform.SetActive(false);

        }else if(sceneName == "Level_Two" || sceneName == "Level_Two_Second" || sceneName == "Level_Two_P1") {

        } else if(sceneName == "Boss_One" || sceneName == "Boss_Two" || sceneName == "Boss_Two_Second") {

        } else {

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player")) {
            player = GameObject.FindGameObjectWithTag("Player").gameObject;
        }

        hazards = GameObject.FindGameObjectsWithTag("Hazard");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(sceneName == "Boss_One") {
            woodsContainer = GameObject.Find("Woods").gameObject;
        } else if(sceneName == "Level_Two") {
            dynamicBg = GameObject.Find("Background Layers").gameObject;
            if(player.GetComponent<Player_Interactions>().defeatedBossTwo) {
                dynamicBg.SetActive(true);
            } else {
                dynamicBg.SetActive(false);
            }

            if(player.GetComponent<Player_Interactions>().fragments < 3) {
                GameObject.Find("SecondLevEnd").SetActive(false);
                GameObject.Find("LeftVineBorder").SetActive(false);
            }


            if(player.GetComponent<Player_Interactions>().destroyedPinecone) {
                GameObject.Find("Pinecone").SetActive(false);
            } 

            if(player.GetComponent<Player_Interactions>().fragments >= 3) {
                foreach (GameObject hazard in hazards) {
                    hazard.SetActive(false);
                }
            }

        } else if(sceneName == "Level_One" || sceneName == "Level_One_Second") {
            if(player.GetComponent<Player_Interactions>().fragments < 2){
                GameObject.Find("ElevatorButton").SetActive(false);
            }


            if (player.GetComponent<Player_Interactions>().destroyedBriar)
            {
                GameObject.Find("Briar").SetActive(false);
                GameObject.Find("BriarDeath").SetActive(false);
            }

            if(player.GetComponent<Player_Interactions>().defeatedBossOne) {
                GameObject.Find("Main Camera").GetComponent<Camera_Controls>().xMax = 47.5f;
                GameObject.Find("NPC").SetActive(false);
                GameObject.Find("BossLevel").SetActive(false);
                GameObject.Find("Gameplay").GetComponent<Transform>().Find("Death Rightmost").GetComponent<Transform>().gameObject.SetActive(false);
                dynamicBg.SetActive(true);
                extensionPlatform.SetActive(true);

                foreach (GameObject hazard in hazards) {
                    hazard.SetActive(false);
                }

                foreach (GameObject enemy in enemies) {
                    enemy.SetActive(false);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    
        if(sceneName == "Boss_One") {

            if(woodsContainer.transform.childCount > 0) {
                woods[0] = woodsContainer.transform.Find("Woods0").GetComponent<SpriteRenderer>();
                woods[1] = woodsContainer.transform.Find("Woods1").GetComponent<SpriteRenderer>();
            }

            if(fadeWoods && t > 0f) {    
                // Debug.Log("FakeWoods True! " + t);

                woods[0].color = new Color(woods[0].color[0], woods[0].color[1], woods[0].color[2], t);
                woods[1].color = new Color(woods[1].color[0], woods[1].color[1], woods[1].color[2], t);

                t -= 0.15f * Time.deltaTime;
            }

            if(t <= 0){ 
                woodsContainer.SetActive(false);
            }
        } 

    }

    public void SetFadeWoods() {
        fadeWoods = true;
    }

    // public void StartChangeWoods() {
    //     StartCoroutine("ChangeWoods");
    // }

    // public IEnumerator ChangeWoods() {
    //     yield return new WaitForSeconds(1);
    //     // GameObject.Find("FloatingPlatforms").gameObject.SetActive(false);
    //     fadeWoods = true;
    // }
}
