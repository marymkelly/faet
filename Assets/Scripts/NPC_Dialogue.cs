using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class NPC_Dialogue : MonoBehaviour
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
    public NPChar npchar;
    private bool hasClicked = false;
    private bool onQuestions = false;  //questions are displayed?

    private bool playerEngaged = false;
    private bool playerColliding = false;
    // private bool runIntro = false;
    private bool runningAnswer = false;


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
    }

    void Update()
    {
        // if (hasClicked) StartConvo();

        if(playerColliding && Input.GetKeyDown(KeyCode.E) && !GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().isInteracting) {
            // hasClicked = true;
            StartConvo();
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

    public NPCQuestion[] GetQuestionsFromIndices(int[] indices) {
        NPCQuestion[] questions = new NPCQuestion[indices.Length];
        int i = 0;

        foreach(int index in indices) {
            questions[i++] = npchar.questions[index];
        }

        return questions;
    }

    void SetQuestionButtons(NPCQuestion[] questions) {
        for (int i = 0; i < buttons.Length; i++) {
            Button button = buttons[i].gameObject.GetComponent<Button>();
            Text text = buttons[i].Find("Text").GetComponent<Text>();

            int btnIndex = i;
            
            if(i < questions.Length){ //usedtobe npc.questions.Length
                button.onClick.AddListener(() => ShowAnswer(questions[btnIndex].Answer));

                text.text = questions[i].Question;
                buttons[i].gameObject.SetActive(true);
            } else {
                buttons[i].gameObject.SetActive(false);
            }
        }

        onQuestions = true;
    }

    void SetDiologueBox() {
            diologue.SetActive(true);
            convo.gameObject.SetActive(onQuestions ? false : true);
            continueBtn.gameObject.SetActive(onQuestions ? false : true);
            GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().FreezePlayer(true);
    }

    void HideButtons() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].gameObject.SetActive(false);
        }

        onQuestions = false;
    }

    void ShowAnswer(string[] answer) {
        // Debug.Log("answer:  "  + answer);
        diologue.GetComponent<VerticalLayoutGroup>().spacing = 24;
        continueBtn.transform.Find("Text").GetComponent<Text>().text = "Continue  →";

        // convo.text =  answer;
        HideButtons();
        convo.gameObject.SetActive(true);
        StartCoroutine(DisplayConvo(answer, false));
        continueBtn.gameObject.SetActive(true);
        // onQuestions = false;
    }

    public void HandleContinue() {
        if(!runningAnswer) {
            if(!onQuestions && npchar.questions.Length > 0) {
                diologue.GetComponent<VerticalLayoutGroup>().spacing = 14;
                convo.gameObject.SetActive(false);

                continueBtn.transform.Find("Text").GetComponent<Text>().text = "Exit  →";

                if(npchar.name == "RuneRando") {
                    SetQuestionButtons(new NPCQuestion[2] {npc.questions[2], npc.questions[3]});
                } else {
                    NPCQuestion[] questions = GetQuestionsFromIndices(npchar.prompts[1].qIndices);
                    SetQuestionButtons(questions);
                    // SetDiologueBox();
                }

            } else {
                // EndDiologueBox();
            } 
        }
    }

    public void StartConvo() {
        hasClicked = false;
        character.text = npchar.name;
        SetDiologueBox();

        if(npchar.questions.Length == 0) {
            StartCoroutine(DisplayConvo(npc.sentences, true));
        } else {
            diologue.GetComponent<VerticalLayoutGroup>().spacing = 14;
            // convo.gameObject.SetActive(false);

            if(!npchar.startsFirst) {
                SetQuestionButtons(npchar.questions);
                // diologue.SetActive(true);
                // continueBtn.gameObject.SetActive(true);
                // GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().FreezePlayer(true);
                SetDiologueBox();
            } else {
                onQuestions = false;

                if(npchar.name == "RuneRando") {
                    if(!GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().castRune) {
                        StartCoroutine(NPCIntro(new string[1] { npc.sentences[0] }, new NPCQuestion[2] {npc.questions[0], npc.questions[1]}));
                    } else {
                        StartCoroutine(NPCIntro(new string[1] { npc.sentences[1] }, new NPCQuestion[2] {npc.questions[2], npc.questions[3]}));
                    }
                } else if(npchar.name == "Impy") {
                    if(!playerEngaged) {
                        continueBtn.transform.Find("Text").GetComponent<Text>().text = "Continue  →";
                        NPCQuestion[] questions = GetQuestionsFromIndices(npchar.prompts[0].qIndices);
                        StartCoroutine(NPCIntro(npchar.prompts[0].sentences, questions));
                    } else {
                        continueBtn.transform.Find("Text").GetComponent<Text>().text = "Continue  →";
                        NPCQuestion[] questions = GetQuestionsFromIndices(npchar.prompts[1].qIndices);
                        StartCoroutine(NPCIntro(npchar.prompts[1].sentences, questions));
                    }
                }

                // diologue.SetActive(true);
                // continueBtn.gameObject.SetActive(true);
                // GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().FreezePlayer(true);
                // SetDiologueBox();
            }
        }

    }

    void EndDiologueBox() {
        runningAnswer = false;
        GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().FreePlayer(false);
        convo.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);
        diologue.SetActive(false);
    }

    public void Click() {
        hasClicked = true;
    }

    IEnumerator DisplayConvo(string[] messageArr, bool closeDiologueBox) {
        onQuestions = false;
        runningAnswer = true;

        foreach (string text in messageArr)
        {
            yield return null;
            convo.text = text;

            while (!(Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")))
            {
                yield return null;
            }

            yield return null;
        }

        if(closeDiologueBox) {
            EndDiologueBox();
        }
        
        runningAnswer = false;
    }

    // IEnumerator DisplayQuestions(NPCQuestion[] questions, IEnumerator intro) {
    //         // if(!npc.startsFirst) {

    //         // }

    //         if(intro != null && !runIntro) {
    //             Debug.Log("INTRO CALLSED");
    //             yield return StartCoroutine(intro);
                
    //             SetDiologueBox();
    //             runIntro = true;
    //         } else {        
    //             SetQuestionButtons(questions);
    //             // SetDiologueBox();
    //         }


    //     // EndDiologueBox();

    // }

    IEnumerator NPCIntro(string[] messageArr, NPCQuestion[] questions) {
        runningAnswer = true;
        convo.gameObject.SetActive(true);
        foreach (string text in messageArr) {
            convo.text = text;

            while (!(Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")))
            {
                yield return null;
            }

            yield return null;
        }

        if(questions != null) {
             SetQuestionButtons(questions);
             SetDiologueBox();
        }
        // convo.gameObject.SetActive(false);
        playerEngaged = true;
        runningAnswer = false;
    }
}

[System.Serializable]
public class NPCharacter {
    public string name;

    [TextArea(0,10)]
    public string[] sentences;
    public NPCQuestion[] questions;
    public bool startsFirst;
}

[System.Serializable]
public class NPCQuestion {

    public string Question;

    [TextArea(0, 4)]
    public string[] Answer;
    // public string Answer;

    public NPCQuestion(string q, string[] a)
    // public NPCQuestion(string q, string a)
    {
      Question = q;
      Answer = a;
    }
}


[System.Serializable]
public class NPChar {

    public string name;
    // public QnA[] questions;
    public NPCQuestion[] questions;
    public NPCPrompt[] prompts;
    public bool startsFirst;
    private bool playerEngaged;

    public bool GetPlayerEngaged() {
        return playerEngaged;
    }

    // public bool SetPlayerEngaged(bool status) {
    //     playerEngaged = status;
    // }

}

[System.Serializable]
public class NPCPrompt {

    [TextArea(0, 6)]
    public string[] sentences;
    public int[] qIndices;
    // private QnA[] Responses;

    public NPCPrompt(string[] s, int[] i)
    {
      sentences = s;
      qIndices = i; 
    }

    // public void setResponses() {

    // }
}

// [System.Serializable]
// public class QnA {

//     public string Question;
//     [TextArea(0, 4)]
//     public string[] Answer;

//     public QnA(string q, string[] a)
//     {
//       Question = q;
//       Answer = a;
//     }
// }