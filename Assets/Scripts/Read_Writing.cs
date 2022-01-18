using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Read_Writing : MonoBehaviour
{
    private GameObject canvas;
    private Text displayHint;
    public string message;
    private bool isColliding = false;
    
    void Start()
    {
            canvas = GameObject.Find("Canvas");
            displayHint = GameObject.Find("Instruction").gameObject.GetComponent<Text>();
            displayHint.text = "";
    }

    void Update()
    {
        if (isColliding && Input.GetKeyDown("e")) {
            StartCoroutine("DisplayMessage");
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            isColliding = true;

            if(collider.gameObject.GetComponent<Player_Interactions>().reviewedControls) {
                displayHint.text =  "Press <color=#00dba0> E </color> to read sign";
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            isColliding = false;
            displayHint.text =  "";
        }
    }

    IEnumerator DisplayMessage() {
        displayHint.text =  message;

        yield return new WaitUntil(() => !GameObject.Find("Player").gameObject.GetComponent<Player_Interactions>().isInteracting && Input.GetButton("Submit"));
        
        displayHint.text =  "Press <color=#00dba0> E </color> to read sign";
    }
}
