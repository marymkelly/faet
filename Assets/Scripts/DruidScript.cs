using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DruidScript : MonoBehaviour
{

    int frags = 0;
    public int minFrags;
    [SerializeField] private DialogueActivator activator;
    [SerializeField] private DialogueObject dialogue;

    void Start()
    {
       frags = GameObject.Find("Player").GetComponent<Player_Interactions>().fragments;

    //    if(GameObject.Find("Player").GetComponent<Player_Interactions>().metPliney){
    //        gameObject.GetComponentInParent<DialogueActivator>().NPC.Name = "Druid Pliney";
    //    }

       if(frags >= minFrags) {
           activator.UpdateDialogueObject(dialogue);
       }   
    }


    void Update()
    {
        if(frags >= minFrags && (activator.diaObj != dialogue)) {
           activator.UpdateDialogueObject(dialogue);
       }   
    }

    // public void TalkedToPliney() {
    //     GameObject.Find("Player").GetComponent<Player_Interactions>().metPliney = true;
    //     gameObject.GetComponentInParent<DialogueActivator>().NPC.Name = "Druid Pliney";
    // }
}
