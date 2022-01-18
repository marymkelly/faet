using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TestDiologueTrigger : MonoBehaviour
{
    //canvas items
    private GameObject canvas;
    private GameObject diologue;
    private GameObject continueBtn;
    private GameObject instruction;
    private Text character;
    private Text convo;
    private Text hint;

    private Transform buttonsGroup;
    private Transform[] buttons;

    //npc related vars
    public NPCharacter npc;
    private bool hasClicked = false;
    private bool onQuestions = false;  //questions are displayed?

    private bool playerColliding = false;


    void Start()
    {
        canvas = GameObject.Find("Canvas");
        diologue = canvas.transform.Find("TestDiologue").gameObject;
        continueBtn = canvas.transform.Find("Continue").gameObject;
        character = diologue.transform.Find("Character").GetComponent<Text>(); //character / npc text
        convo = diologue.transform.Find("Convo").GetComponent<Text>();
        instruction = canvas.transform.Find("Instruction").gameObject;
        hint = instruction.GetComponent<Text>();
        
        //setup buttons
        continueBtn.GetComponent<Button>().onClick.AddListener(() => HandleContinue());

        buttonsGroup = diologue.transform.Find("Choices").transform;
        buttons = new Transform[buttonsGroup.childCount];
        int i = 0;

        foreach (Transform t in buttonsGroup) {
            buttons[i++] = t;
        }

        if(npc.questions.Length > 0) {
            diologue.GetComponent<VerticalLayoutGroup>().spacing = 14;
            convo.gameObject.SetActive(false);
            SetQuestionButtons();
        } 
    }

    void Update()
    {
        if (hasClicked) StartConvo();

        if(playerColliding && Input.GetKeyDown(KeyCode.E) && !GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().isInteracting) {
            hasClicked = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            playerColliding = true;

            if(!other.gameObject.GetComponent<Player_Interactions>().isInteracting) hint.text = "Press <color=#00dba0>E</color> to interact";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerColliding = false;
            hint.text = "";
        }
    }



    void SetQuestionButtons() {
        for (int i = 0; i < buttons.Length; i++) {
            Button button = buttons[i].gameObject.GetComponent<Button>();
            Text text = buttons[i].Find("Text").GetComponent<Text>();

            int btnIndex = i;
            
            if(i < npc.questions.Length){
                button.onClick.AddListener(() => ShowAnswer(npc.questions[btnIndex].Answer));

                text.text = npc.questions[i].Question;
                buttons[i].gameObject.SetActive(true);
            } else {
                buttons[i].gameObject.SetActive(false);
            }
        }

        onQuestions = true;
    }

    void HideButtons() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }

    void ShowAnswer(string[] answer) {
        // Debug.Log("answer:  "  + answer);
        diologue.GetComponent<VerticalLayoutGroup>().spacing = 24;
        continueBtn.transform.Find("Text").GetComponent<Text>().text = "Continue  →";

        // convo.text =  answer;
        convo.gameObject.SetActive(true);
        continueBtn.gameObject.SetActive(true);
        DisplayConvo(answer, false);
        HideButtons();

        onQuestions = false;
    }
    public void HandleContinue() {
        if(!onQuestions && npc.questions.Length > 0) {
            diologue.GetComponent<VerticalLayoutGroup>().spacing = 14;
            convo.gameObject.SetActive(false);

            continueBtn.transform.Find("Text").GetComponent<Text>().text = "Exit  →";
            SetQuestionButtons();

        } else {
            EndConvo();
        } 
    }

    public void StartConvo() {
        hasClicked = false;
        character.text = npc.name;
        SetQuestionButtons();
        diologue.SetActive(true);
        continueBtn.gameObject.SetActive(true);
        GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().FreezePlayer(true);

        if(npc.questions.Length == 0) {
            StartCoroutine(DisplayConvo(npc.sentences, true));
        }
    }

    void EndConvo() {
        GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().FreePlayer(false);
        continueBtn.gameObject.SetActive(false);
        diologue.SetActive(false);
    }

    public void Click() {
        hasClicked = true;
    }

    IEnumerator DisplayConvo(string[] messageArr, bool closeDiologueBox) {
        foreach (string text in messageArr)
        {
            convo.text = text;

            while (!(Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")))
            {
                yield return null;
            }

            yield return null;
        }

        if(closeDiologueBox) {
            EndConvo();
        }

    }
}

// [System.Serializable]
// public class NPCharacter {
//     public string name;

//     [TextArea(0,10)]
//     public string[] sentences;
//     public NPCQuestion[] questions;
// }

// [System.Serializable]
// public class NPCQuestion {

//     public string Question;

//     [TextArea(0, 4)]
//     // public string[] Answer;
//     public string Answer;

//     // public NPCQuestion(string q, string[] a)
//     public NPCQuestion(string q, string a)
//     {
//       Question = q;
//       Answer = a;
//     }
// }
