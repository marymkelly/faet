using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private Text buttonLabel;
    [SerializeField] private Text hint;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;
    private NpcHandler npcHandler;

    void Start() {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        npcHandler = GetComponent<NpcHandler>();
        CloseDiologueBox();
    }

    public void SetInteractingNpc(Npc npc) {
        npcHandler.AddNpc(npc);
    }

    public void ShowDialogue(DialogueObject dialogueObject) {
        dialogueBox.SetActive(true);
        IsOpen = true;
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents) {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
        // yield return new WaitForSeconds(2);
        string npcName = npcHandler.GetNpcName();

        if(npcName != null){
            // yield return RunTypingEffect(npcName, characterName);
            characterName.text = npcName;

            if(dialogueObject.Dialogue.Length == 0) {
                characterName.text = "<color=#111111>Speaking with</color> " + npcName;
                nextButton.SetActive(false);   
            }
        }

        for(int i = 0; i < dialogueObject.Dialogue.Length;  i++) {
            nextButton.SetActive(true);
            string dialogue = dialogueObject.Dialogue[i];
            yield return RunTypingEffect(dialogue, textLabel);

            textLabel.text = dialogue;
            buttonLabel.text = "Continue  →";
            hint.text = "Press <color=#00dba0>Enter</color> to continue";

            if(i == dialogueObject.Dialogue.Length - 1 && !dialogueObject.HasResponses) buttonLabel.text = "Exit  →";
            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;  
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"));
        }

        if(dialogueObject.HasResponses) {
            hint.text = "";
            responseHandler.ShowResponses(dialogueObject.Responses);
        } else {
            hint.text = "";
            CloseDiologueBox();
        }
    
    }

    private IEnumerator RunTypingEffect(string dialogue, TMP_Text textObj) {
        typewriterEffect.Run(dialogue, textObj);
        

        while(typewriterEffect.IsRunning) {
            yield return null;

            if(Input.GetKeyDown(KeyCode.Space)){
                typewriterEffect.Stop();
            }
        }
    }

    public void CloseDiologueBox() {
        dialogueBox.SetActive(false);
        IsOpen = false;
        GameObject.FindWithTag("Player").GetComponent<Player_Interactions>().isInteracting = false;
        textLabel.text = string.Empty;
    }
}
