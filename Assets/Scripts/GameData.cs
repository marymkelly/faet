using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameData : MonoBehaviour
{
    public static GameData control; 
    public static bool existingData;

    public int currentHealth;

    public float maxMagic;
    public float currentMagic;
    public float magicPower;

    public int magicLevel;
    public int lifetimeFlora; //lifetime flora collected
    public int nextLevelFlora; //total flora needed to next level;
    public int currentFloraRound; //current number of flora collected between levels

    public bool reviewedControls; //if reviewed controls tutorial
    public bool reviewedMagic;
    public bool reviewedHealth;
    public bool encounteredFlora;
    public bool encounteredHazard;
    public bool encounteredEnemy;
    public bool encounteredCheckpoint;
    public bool encounteredClimbing;
    public bool encounteredLightpool;
    public bool encounteredRune;
    public bool castRune;
    public int fragments;

    public bool pineconeHint;
    public bool destroyedPinecone;
    public bool destroyedBriar;
    public bool defeatedBossOne;
    public bool defeatedBossTwo;
    public bool metPliney;

    public float[] position;
    public string sceneName;
    public DateTime lastSaved;
    private bool calledLoad = false;
    public bool diedOnBoss = false;

    private Vector3 scenePosition;
    // private bool sceneTransitioning = false;

    void Awake() {
        // Debug.Log(Application.persistentDataPath);
        // Debug.Log("GAME DATA AWAKE " + SceneManager.GetActiveScene().name);
        if(control == null) {
            control = this;
            DontDestroyOnLoad(gameObject);
        } else if(control != this){
            Destroy(gameObject);
        }

        // Debug.Log("APP STREAMING PATH " + Application.streamingAssetsPath);

        if(SceneManager.GetActiveScene().name != "Credits_Screen"){
            if(SceneManager.GetActiveScene().name == "Start_Screen"){
                GetPreviouslySaved();
            } else {
                Load();
            }
        }
    }

    // called first
    void OnEnable()
    {
        // Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Debug.Log("OnSceneLoaded: " + scene.name); //current scene
        // Debug.Log(mode);
        if(!(SceneManager.GetActiveScene().name == "Start_Screen" || SceneManager.GetActiveScene().name == "Intro_Scene" || SceneManager.GetActiveScene().name == "Credits_Screen")) {
            Load();
        }

        if(!calledLoad) {
            // Debug.Log("saved scene name = " + sceneName); //prev scene
            if(scene.name == "Level_One_Second") { //new scene
                if(sceneName == "Level_Two") { 
                    // Debug.Log("From Level 2!");
                    scenePosition = new Vector3(42.3f, 17.7f, 0f);
                    player.transform.position = scenePosition;   
                    
                } else if(sceneName == "Boss_Two_Second") {
                    // Debug.Log("From Boss Two Second!");
                    scenePosition = new Vector3(-19.25f, 18.95f, 0f);
                    player.transform.position = scenePosition;
                    GameObject.Find("Trapezoid Elevator").transform.position = new Vector3(-19.78f, 18.1f, 0f);
                    GameObject.Find("Trapezoid Elevator").GetComponent<Elevator>().TriggerElevator(GameObject.Find("PointB_Elevator").transform);
                }
            } else if(scene.name == "Level_Two") { //new scene
                if(sceneName == "Level_Two_P1") {
                    // Debug.Log("From P1!");
    
                    scenePosition = new Vector3(-36.13f, 5.17f, 0f);
                    player.transform.position = scenePosition;   
                } else if(sceneName == "Boss_Two" || sceneName == "Boss_Two_Second") {
                    // Debug.Log("From Boss Two!");

                    scenePosition = new Vector3(-73.38f, -2.00f, 0f);
                    player.transform.position = scenePosition;   
                } else if(sceneName == "Level_One_Second") {
                    // Debug.Log("From Lev one second!");
                    scenePosition = new Vector3(-27.6f, -27.72f, 0f);
                    player.transform.position = scenePosition;   
                }
            } else if(scene.name == "Boss_Two_Second") {
                if(sceneName == "Level_Two") {
                    scenePosition = new Vector3(-11.7f, -2.95f, 0f);
                    player.transform.position = scenePosition;   
                } else if(sceneName == "Level_One" || sceneName == "Level_One_Second") {
                    scenePosition = new Vector3(14.31f, -2.25f, 0f);
                    player.transform.position = scenePosition; 
                }
            }
        } else {
            calledLoad = false;
            Vector3 vecPos;
            vecPos.x = position[0];
            vecPos.y = position[1];
            vecPos.z = position[2];

            player.transform.position = vecPos;  
        }
    }


    void Update()
    {
        if(!(SceneManager.GetActiveScene().name == "Start_Screen" || SceneManager.GetActiveScene().name == "Intro_Scene" || SceneManager.GetActiveScene().name == "Credits_Screen")) {
            GetCurrentData();
        }
    }

    public void GetCurrentData() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = player.GetComponent<Player_Leaf_Update>().currentHealth;
        currentMagic = player.GetComponent<Player_Magic_Update>().currentMagic;
        maxMagic = player.GetComponent<Player_Magic_Update>().maxMagic;
        magicLevel = player.GetComponent<Player_Magic_Update>().magicLevel;
        magicPower = player.GetComponent<Player_Magic_Update>().magicPower;
        lifetimeFlora = player.GetComponent<Player_Magic_Update>().lifetimeFlora;
        nextLevelFlora = player.GetComponent<Player_Magic_Update>().nextLevelFlora;
        currentFloraRound = player.GetComponent<Player_Magic_Update>().currentFloraRound;
        reviewedControls = player.GetComponent<Player_Interactions>().reviewedControls;
        reviewedMagic = player.GetComponent<Player_Interactions>().reviewedMagic;
        reviewedHealth = player.GetComponent<Player_Interactions>().reviewedHealth;
        encounteredFlora = player.GetComponent<Player_Interactions>().encounteredFlora;
        encounteredHazard = player.GetComponent<Player_Interactions>().encounteredHazard;
        encounteredEnemy = player.GetComponent<Player_Interactions>().encounteredEnemy;
        encounteredCheckpoint = player.GetComponent<Player_Interactions>().encounteredCheckpoint;
        encounteredClimbing = player.GetComponent<Player_Interactions>().encounteredClimbing;
        encounteredLightpool = player.GetComponent<Player_Interactions>().encounteredLightpool;
        encounteredRune = player.GetComponent<Player_Interactions>().encounteredRune;
        castRune = player.GetComponent<Player_Interactions>().castRune;
        metPliney = player.GetComponent<Player_Interactions>().metPliney;
        fragments = player.GetComponent<Player_Interactions>().fragments;

        pineconeHint = player.GetComponent<Player_Interactions>().pineconeHint;
        destroyedPinecone = player.GetComponent<Player_Interactions>().destroyedPinecone;
        destroyedBriar = player.GetComponent<Player_Interactions>().destroyedBriar;
        defeatedBossOne = player.GetComponent<Player_Interactions>().defeatedBossOne;
        defeatedBossTwo = player.GetComponent<Player_Interactions>().defeatedBossTwo;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
    }
    
    public void Save() {
        BinaryFormatter BinForm = new BinaryFormatter(); //Creates a bin formatter
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat"); //Creates file
        PlayerData data = new PlayerData(); //Creates container for data
        GetCurrentData();

        data.currentHealth = currentHealth;
        data.maxMagic = maxMagic;
        data.currentMagic = currentMagic;
        data.magicLevel = magicLevel;
        data.magicPower = magicPower;
        data.lifetimeFlora = lifetimeFlora;
        data.nextLevelFlora = nextLevelFlora;
        data.currentFloraRound = currentFloraRound; 
        data.reviewedControls = reviewedControls;
        data.reviewedMagic = reviewedMagic;
        data.reviewedHealth = reviewedHealth;
        data.encounteredFlora = encounteredFlora;
        data.encounteredHazard = encounteredHazard;
        data.encounteredEnemy = encounteredEnemy;
        data.encounteredCheckpoint = encounteredCheckpoint;
        data.encounteredClimbing = encounteredClimbing;
        data.encounteredLightpool = encounteredLightpool;
        data.encounteredRune = encounteredRune;
        data.castRune = castRune;
        data.metPliney = metPliney;
        data.fragments = fragments;

        data.pineconeHint = pineconeHint;
        data.destroyedPinecone = destroyedPinecone;
        data.destroyedBriar = destroyedBriar;
        data.defeatedBossOne = defeatedBossOne;
        data.defeatedBossTwo = defeatedBossTwo;

    
        data.position = position;
        data.position[0] = position[0];
        data.position[1] = position[1];
        data.position[2] = position[2];

        data.sceneName = sceneName;

        data.lastSaved = System.DateTime.Now;
        data.diedOnBoss = diedOnBoss;

        // Debug.Log("SAVING....");

        BinForm.Serialize (file, data); //Serializes
        file.Close(); //Closes the file

        lastSaved = data.lastSaved;
    }

    public void CallForLoad(){
        // Debug.Log("Called for load!");
        calledLoad = true;
        Load();
    }

    public bool GetPreviouslySaved(){
       return existingData = File.Exists (Application.persistentDataPath + "/playerInfo.dat");
    }

    public void Load () { 
    // Debug.Log("Loading Data!");
    if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter BinForm = new BinaryFormatter ();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)BinForm.Deserialize(file);
            file.Close ();
      
            currentHealth = data.currentHealth;
            maxMagic = data.maxMagic;
            currentMagic = data.currentMagic;
            magicLevel = data.magicLevel;
            magicPower = data.magicPower;
            lifetimeFlora = data.lifetimeFlora;
            nextLevelFlora = data.nextLevelFlora;
            currentFloraRound = data.currentFloraRound;
            reviewedControls = data.reviewedControls;
            reviewedMagic = data.reviewedMagic;     
            reviewedHealth = data.reviewedHealth;
            encounteredFlora = data.encounteredFlora;
            encounteredHazard = data.encounteredHazard;
            encounteredEnemy = data.encounteredEnemy;
            encounteredCheckpoint = data.encounteredCheckpoint;
            encounteredClimbing = data.encounteredClimbing;
            encounteredLightpool = data.encounteredLightpool;
            encounteredRune = data.encounteredRune;
            castRune = data.castRune;
            metPliney = data.metPliney;
            fragments = data.fragments;

            pineconeHint = data.pineconeHint;
            destroyedPinecone = data.destroyedPinecone;
            destroyedBriar = data.destroyedBriar;
            defeatedBossOne = data.defeatedBossOne;
            defeatedBossTwo = data.defeatedBossTwo;

            sceneName = data.sceneName;
            lastSaved = data.lastSaved;
            diedOnBoss = data.diedOnBoss;

            position = new float[3];
            position[0] = data.position[0];
            position[1] = data.position[1];
            position[2] = data.position[2];

            if(!calledLoad) {
                SetLoadedData();
            } else {
                if(SceneManager.GetActiveScene().name != sceneName) {
                    if(sceneName == "Boss_One" && defeatedBossOne) {
                        sceneName = "Level_One_Second";
                        position[0] = -22f;
                        position[1] = 8;
                        position[2] = 0f;

                        SceneManager.LoadScene("Level_One_Second");
                    } else {
                        SceneManager.LoadScene(sceneName);
                    }
                    // Debug.Log("changing call load after chage");
                    // calledLoad = false;
                    // Vector3 vecPos;
                    // vecPos.x = position[0];
                    // vecPos.y = position[1];
                    // vecPos.z = position[2];

                    // GameObject player = GameObject.FindGameObjectWithTag("Player");
                    // player.transform.position = vecPos;  
                } 
                // else {
                //     Debug.Log("changing call load");
                //     calledLoad = false;
                    
                //     Vector3 vecPos;
                //     vecPos.x = position[0];
                //     vecPos.y = position[1];
                //     vecPos.z = position[2];

                //     GameObject player = GameObject.FindGameObjectWithTag("Player");
                //     player.transform.position = vecPos;   
                // }
            }
        } 
    }

    public void SetLoadedData() { //on transition
        // Debug.Log("Setting Loaded Data");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player_Leaf_Update>().currentHealth = currentHealth;
        player.GetComponent<Player_Magic_Update>().currentMagic = currentMagic;
        player.GetComponent<Player_Magic_Update>().maxMagic = maxMagic;
        player.GetComponent<Player_Magic_Update>().magicLevel = magicLevel;
        player.GetComponent<Player_Magic_Update>().magicPower = magicPower;
        player.GetComponent<Player_Magic_Update>().lifetimeFlora = lifetimeFlora;
        player.GetComponent<Player_Magic_Update>().nextLevelFlora = nextLevelFlora;
        player.GetComponent<Player_Magic_Update>().currentFloraRound = currentFloraRound;
        player.GetComponent<Player_Interactions>().reviewedControls = reviewedControls;
        player.GetComponent<Player_Interactions>().reviewedMagic = reviewedMagic;
        player.GetComponent<Player_Interactions>().reviewedHealth = reviewedHealth;
        player.GetComponent<Player_Interactions>().encounteredFlora = encounteredFlora;
        player.GetComponent<Player_Interactions>().encounteredHazard = encounteredHazard;
        player.GetComponent<Player_Interactions>().encounteredEnemy = encounteredEnemy;
        player.GetComponent<Player_Interactions>().encounteredCheckpoint = encounteredCheckpoint;
        player.GetComponent<Player_Interactions>().encounteredClimbing = encounteredClimbing;
        player.GetComponent<Player_Interactions>().encounteredLightpool = encounteredLightpool;
        player.GetComponent<Player_Interactions>().encounteredRune = encounteredRune;
        player.GetComponent<Player_Interactions>().castRune = castRune;
        player.GetComponent<Player_Interactions>().metPliney = metPliney;
        player.GetComponent<Player_Interactions>().fragments = fragments;

        player.GetComponent<Player_Interactions>().pineconeHint = pineconeHint;
        player.GetComponent<Player_Interactions>().destroyedPinecone = destroyedPinecone;
        player.GetComponent<Player_Interactions>().destroyedBriar = destroyedBriar;
        player.GetComponent<Player_Interactions>().defeatedBossOne = defeatedBossOne;
        player.GetComponent<Player_Interactions>().defeatedBossTwo = defeatedBossTwo;

        if(diedOnBoss && (SceneManager.GetActiveScene().name == "Level_One_Second" || SceneManager.GetActiveScene().name == "Level_Two")) {
            player.transform.position = GameObject.Find("Boss_Checkpoint").transform.position;
            diedOnBoss = false;
        }

    }

    public void NewGame(){
        if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) { 
            File.Delete (Application.persistentDataPath + "/playerInfo.dat");
        }

        SceneManager.LoadScene("Intro_Scene");
    }
}


[Serializable]
class PlayerData {
    //health and magic
    public int currentHealth;
    public float maxMagic;
    public float currentMagic;
    public float magicPower;
    public int magicLevel;
    public int lifetimeFlora; //lifetime flora collected
    public int nextLevelFlora; //total flora needed to next level;
    public int currentFloraRound; //current number of flora collected between levels

    //Interaction Controls
    public bool reviewedControls;
    public bool reviewedMagic;
    public bool reviewedHealth;
    public bool encounteredFlora;
    public bool encounteredHazard;
    public bool encounteredEnemy;
    public bool encounteredCheckpoint;
    public bool encounteredClimbing;
    public bool encounteredLightpool;
    public bool encounteredRune;
    public bool castRune;

    //fragment & other 
    public int fragments;
    public bool metPliney;

    //player position
    public float[] position;
    public string sceneName;

    //barriers and bosses
    public bool pineconeHint; 
    public bool destroyedPinecone;
    public bool destroyedBriar;
    public bool defeatedBossOne;
    public bool defeatedBossTwo;
    
    public DateTime lastSaved;
    public bool diedOnBoss;
}