using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Interactions : MonoBehaviour
{
    public bool reviewedControls = false;
    public bool reviewedMagic = false;
    public bool reviewedHealth = false;
    public bool encounteredFlora = false;
    public bool encounteredHazard = false; 
    public bool encounteredEnemy = false;
    public bool encounteredCheckpoint = false;
    public bool encounteredClimbing = false;
    public bool encounteredLightpool = false;
    public bool encounteredRune = false;
    public bool castRune = false;

    public bool pineconeHint = false;
    public bool destroyedPinecone = false;
    public bool destroyedBriar = false;
    public bool defeatedBossOne = false;
    public bool defeatedBossTwo = false;

    [HideInInspector]
    public bool isInteracting = false; //interacting with NPC, Tutorials, or other game element
    public bool controlsOpen = false;
    public bool metPliney = false;
    

    private GameObject canvas;
    private GameObject tutorial;
    private GameObject instruction;
    private GameObject controls; 
    private GameObject player; 

    public int fragments = 0;
    // private int currentFragments = 0;
    public Sprite[] fragmentImages;

    [SerializeField] private GameObject runePuzzle;
    [SerializeField] private DialogueUI dialogueUI;
    // [SerializeField] private GameObject defeatUI;
    public bool defeatOn = false;

    [HideInInspector]
    public bool bossDialogue = false;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // Resolution[] resolutions = Screen.resolutions;

        // // Print the resolutions
        // foreach (var res in resolutions)
        // {
        //     Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
        // }

        canvas = GameObject.Find("Canvas");
        tutorial = canvas.transform.Find("Tutorial").gameObject; 
        instruction = canvas.transform.Find("Instruction").gameObject;
        controls = canvas.transform.Find("Controls").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        UpdateRunePuzzle();
    }

    private void Update() {
        if(dialogueUI != null && dialogueUI.IsOpen) {
            isInteracting = true;
            return;
        } 
    
        if(Input.GetKeyDown(KeyCode.E) || bossDialogue) {
            // bossDialogue = false;
            if(Interactable !=  null) {
                Interactable.Interact(this);
            }

            if(SceneManager.GetActiveScene().name == "Level_Two_P1" && pineconeHint) {
                bossDialogue = false;
            }
        }

        if (isInteracting && !bossDialogue) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }

        if(defeatOn) isInteracting = true;

        // if(Input.GetKeyDown(KeyCode.F)) {
        //     ShowFragment();
        // }
        // Debug.Log("INTERACTING? " + isInteracting);


        // if(Input.GetKey(KeyCode.G)) {
        //     isInteracting = !isInteracting;
        //     Debug.Log("G Key? " + isInteracting);
        // }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "BossDialogue") {
            bossDialogue = true;
        }

        if(collision.name == "Dwarf" && fragments > 0 && !pineconeHint) {
            bossDialogue = true;
            pineconeHint = true;
        }
        
        if (collision.name == "Enter Hollow")
        {
            instruction.GetComponent<Text>().text = "Press <color=#00dba0>E</color> to Enter";
        } 

          if(collision.name == "fragmentDrop") {
            ShowFragment();
            Destroy(collision.gameObject);
        }
        

        if (collision.CompareTag("Flora"))
        {
            if (!encounteredFlora)
            {
                StartCoroutine(StartInteraction(ShowTutorial("Flora")));
            }
        }

        else if (collision.name == "TutorialCheck")  //runs IEnumerator which shows any tutorials not previously reviewed
        {
            IEnumerator[] reviewtuts = new IEnumerator[3];
            int i = 0;

            if(!reviewedControls) {
                reviewtuts[i] = ShowControls();
                i++;
            }

            if (!encounteredFlora)
            {
                reviewtuts[i] = ShowTutorial("Flora");
                i++;
            }
            
            if (!encounteredHazard)
            {
                reviewtuts[i] = ShowTutorial("Hazard");
                i++;
            }

            Array.Resize(ref reviewtuts, i);
            StartCoroutine(MissingTutorials(reviewtuts));
        } else if (collision.name == "Checkpoint") 
        {
            if(!encounteredCheckpoint) {
                StartCoroutine(StartInteraction(ShowTutorial("Checkpoint")));
                encounteredCheckpoint = true;
            }
        } 
        else if (collision.name == "Lightpool") {
            if(!encounteredLightpool) {
                StartCoroutine(StartInteraction(ShowTutorial("Lightpool")));
                encounteredLightpool = true;
            }
        }
        else if (collision.name == "RuneTrigger")
        {
            if (!encounteredRune)
            {
                StartCoroutine(StartInteraction(ShowTutorial("Rune")));
                encounteredRune = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision) {
        if(collision.name == "Enter Hollow" && Input.GetKeyDown(KeyCode.E)) {
            GameObject.Find("GameData").GetComponent<GameData>().Save();
            SceneManager.LoadScene(4);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.name == "Enter Hollow") {
            instruction.GetComponent<Text>().text = "";
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hazard"))
        {
            if (!encounteredHazard)
            {
                StartCoroutine(StartInteraction(ShowTutorial("Hazard")));
            }
        }
    }

    public void FreezePlayer(bool interacting = true) { //takes bool in case method wants to be accessed elsewhere
        isInteracting = interacting;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        // if(interacting){
        //     Time.timeScale = 0f;
        // }
    }   

    public void FreePlayer(bool interacting = false) { //takes bool in case method wants to be accessed elsewhere
        isInteracting = interacting;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        // Time.timeScale = 1f;
    }  

    public void ShowRuneTut() {
        // StartCoroutine(DisplayTutorial("Rune"));
        StartCoroutine(StartInteraction(ShowTutorial("Rune")));
    } 

    public void ViewControls() {
        StartCoroutine(StartInteraction(ShowControls("Press <color=#00dba0>Tab</color> key to exit")));
    } 



    public void SetHasReviewedControls(bool status) {
        reviewedControls = status;
    }

    IEnumerator MissingTutorials(IEnumerator[] flashcards) {
        foreach(IEnumerator tut in flashcards) {
            yield return StartCoroutine(StartInteraction(tut));
        }
    }

    public IEnumerator DisplayTutorial(string name) {
        string hint = "Press <color=#00dba0>Enter/Return</color> to continue";

        tutorial.GetComponent<TutorialManager>().SetTutorial(name);
        tutorial.SetActive(true);

        instruction.GetComponent<Text>().text = hint;

        yield return null;
        while (!(Input.GetButtonDown("Submit") && Input.GetMouseButtonDown(0)))
        {
            yield return null;
        }

        tutorial.SetActive(false);
        instruction.GetComponent<Text>().text = "";
    }

    IEnumerator ShowTutorial(string name, string hint = "Press <color=#00dba0>Enter/Return</color> to continue")
    {
        instruction.GetComponent<Text>().text = hint;

        tutorial.GetComponent<TutorialManager>().SetTutorial(name);
        tutorial.SetActive(true);

        yield return null;
        while (!(Input.GetButtonDown("Submit") || Input.GetMouseButtonDown(0)))
        {
            yield return null;
        }

        if(name == "Flora") { //also shows magic tutorial if not already reviewed
            encounteredFlora = true; 
            yield return StartCoroutine(ShowTutorial("Magic"));
            reviewedMagic = true;
        } 

        else if(name == "Hazard") {  //also shows health tutorial if not already reviewed
            encounteredHazard = true;
            yield return StartCoroutine(ShowTutorial("Health"));
            reviewedHealth = true;
        }

        else if(name == "Checkpoint") {  //also shows climbing tutorial if not already encountered
            encounteredCheckpoint = true;
            yield return StartCoroutine(ShowTutorial("Climb"));
            encounteredClimbing = true;
        }

        tutorial.SetActive(false);
        
        if(name == "Climb") {
            instruction.GetComponent<Text>().text = "Try pressing <color=#00dba0>X</color>";
            yield return new WaitUntil(() => Input.GetKey(KeyCode.X));
        }

        instruction.GetComponent<Text>().text = "";
        
    }

    IEnumerator ShowControls(string hint = "Press <color=#00dba0>Tab</color> to continue")
    {
        controlsOpen = true;
        controls.SetActive(true);
        instruction.GetComponent<Text>().text = hint;

        yield return null;
        while (!Input.GetKeyDown(KeyCode.Tab))
        {
            yield return null;
        }

        controls.SetActive(false);
        instruction.GetComponent<Text>().text = "";
        reviewedControls = true;
        controlsOpen = false;
    }
    
    IEnumerator StartInteraction(IEnumerator next) {
        FreezePlayer(true);

        yield return StartCoroutine(next);

        FreePlayer();
    }

    public void ShowFragment(){
        StartCoroutine("DisplayFragment");
    }

    IEnumerator DisplayFragment() {
        isInteracting = true;
        canvas.transform.Find("Fragments").transform.Find("fragImg").GetComponent<Image>().sprite = fragmentImages[fragments];
        canvas.transform.Find("Fragments").gameObject.SetActive(true);

        instruction.GetComponent<Text>().text = "Press <color=#00dba0>Enter/Return</color> to continue";
        // yield return new WaitUntil(()  => Input.GetButtonDown("Submit") || Input.GetMouseButtonDown(0));
        yield return null;
        yield return new WaitUntil(()  => Input.GetButtonDown("Submit"));
        canvas.transform.Find("Fragments").gameObject.SetActive(false);
        instruction.GetComponent<Text>().text = "";

        fragments++;
        UpdateRunePuzzle();
        isInteracting = false;

        if(fragments == 3) {
            GameObject.Find("GameData").GetComponent<GameData>().Save();
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().StartLoadingLevel(8);
        }
    }

    private void UpdateRunePuzzle() {
        Transform fragContainer = runePuzzle.transform.Find("FragmentsContainer");

        int i = 0;

        if(fragments > 0) {
            runePuzzle.SetActive(true);

            foreach (Transform t in fragContainer)
            {
                if(i < fragments) {
                    t.gameObject.SetActive(true);
                } else {
                    t.gameObject.SetActive(false);
                }

                i++;
            } 
        }


    }
}