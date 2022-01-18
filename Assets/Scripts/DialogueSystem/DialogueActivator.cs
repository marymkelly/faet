using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private Npc npc;
    [SerializeField] private DialogueObject dialogueObject;
    public Npc NPC => npc;
    public DialogueObject diaObj => dialogueObject;

    [HideInInspector]
    public Text hint;
    private string[] gratitude = new string[]{"\"Thank you for restoring the village!\"", "\"You're the best!\"", "\"Thanks!\"", " ", "\"Much thanks\"", "\"Go raibh maith agat\"", "[smiles]", "[deep in thought]"};

    void Start() {
        hint = GameObject.Find("Instruction").gameObject.GetComponent<Text>();
    }
    
    public void UpdateDialogueObject(DialogueObject dialogueObject){
        this.dialogueObject = dialogueObject;
    }
    
    public void Interact(Player_Interactions player) {
        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>()) {
            if(responseEvents.DialogueObject == dialogueObject) {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        player.DialogueUI.SetInteractingNpc(NPC);
        player.DialogueUI.ShowDialogue(dialogueObject);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && other.TryGetComponent(out Player_Interactions player)){
            Debug.Log("Dialogue Activator " + other);

            if(player.fragments == 3) {
                hint.text = gratitude[(int)Mathf.Floor(Random.value * gratitude.Length)];
            } else if((NPC.Name == "Erza" || NPC.Name == "Doug") && player.fragments == 0) {
                hint.text = ""; 
            } else {
                if(NPC.Name == "???" && SceneManager.GetActiveScene().name == "Boss_Two") {                
                    player.transform.position = new Vector2(-7.1f, -3f);
                    player.GetComponent<Player_Interactions>().FreezePlayer();
                    hint.text = " "; 
                } else {
                    if (!player.isInteracting) hint.text = "Press <color=#00dba0>E</color> to interact";
                }

                player.Interactable = this;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player") && other.TryGetComponent(out Player_Interactions player)){
            if(player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this) {
                hint.text = " ";
                player.Interactable = null;
            }

            hint.text = "";
        }
    }
}
