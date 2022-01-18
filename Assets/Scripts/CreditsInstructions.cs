using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsInstructions : MonoBehaviour
{
    public Text message;
    private bool hasMoved = false;
    private bool isMoving = false;
    private bool hintDisplayed = false;
    private bool exitHint = false;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = HintCounter(5);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") > 0.2f) {
            Debug.Log("Movement Registered");
            hasMoved = true;

            if(hintDisplayed || (hasMoved && !isMoving)) {
                StopCoroutine(coroutine);
                message.text = "";
                hintDisplayed = false;
            }

            isMoving = true;
        }

        if(isMoving && (Input.GetAxis("Horizontal") == 0f) && !exitHint){
            isMoving = false;
            coroutine = HintCounter(18);
            StartCoroutine(coroutine);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {

        if(gameObject.name == "LeftExitHint") {
            message.text = "Continue moving left to exit to <color=#00dba0>Main Menu</color>";
        } else if(gameObject.name == "RightExitHint") {
            message.text = "Continue moving right to exit to <color=#00dba0>Main Menu</color>";
        }

        exitHint = true;
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        message.text = "";
        exitHint = false;
    }

    IEnumerator HintCounter(int wait) {
        yield return new WaitForSeconds(wait);
        do {
            message.text = "Use the <color=#00dba0>Left</color> & <color=#00dba0>Right Arrow Keys</color> to navigate through credits";
            hintDisplayed = true;
            yield return new WaitForSeconds(10);
            message.text = "";
            hintDisplayed = false;

            yield return new WaitForSeconds(12);
        } while(!isMoving);
    }
}
