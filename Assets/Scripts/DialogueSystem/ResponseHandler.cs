using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;
    [SerializeField] private RectTransform nextButton;
    // [SerializeField] private RectTransform responsePivotBox;

    private DialogueUI diologueUI;
    private ResponseEvent[] responseEvents;

    List<GameObject> tempResponseButtons = new List <GameObject>();
    private ActionsManager actionManager;

    void Start() {
        diologueUI = GetComponent<DialogueUI>();
        actionManager = FindObjectOfType<ActionsManager>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents) {
        this.responseEvents = responseEvents;
    }   

    public void ShowResponses(Response[] responses){ 
        nextButton.gameObject.SetActive(false);
        float responseBoxHeight = 90;

        for(int i = 0; i < responses.Length; i++) {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponentInChildren<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y + 15;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        // responsePivotBox.gameObject.SetActive(true);
        responseBox.gameObject.SetActive(true);
        // responseBox.GetComponentInParent<RectTransform>().sizeDelta = new Vector2(responseBox.GetComponentInParent<RectTransform>().sizeDelta.x, 80);
    }

    private void OnPickedResponse(Response response, int responseIndex) {
        // responsePivotBox.gameObject.SetActive(false);
        responseBox.gameObject.SetActive(false);
        // responseBox.GetComponentInParent<RectTransform>().sizeDelta = new Vector2(responseBox.GetComponentInParent<RectTransform>().sizeDelta.x, 0);

        foreach(GameObject button in tempResponseButtons) {
            Destroy(button);
        }

        tempResponseButtons.Clear();

        if(responseEvents != null && responseIndex <= responseEvents.Length) {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;

        if(response.actionResponse != ""){
            Debug.Log("ACTION RESPONSE FOUND " + response.actionResponse);

            if(response.actionResponse == "Wait") {
                if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Interactions>().fragments < 2) {
                    GameObject.Find("Pliney").gameObject.GetComponent<Pliney>().StartPlineyCo();
                    diologueUI.ShowDialogue(response.DialogueObject);
                } else {
                    diologueUI.CloseDiologueBox();
                }
            } else {
                if(response.actionResponse == "Pliney") {
                    diologueUI.ShowDialogue(response.DialogueObject);
                } else {
                    diologueUI.CloseDiologueBox();
                } 

                actionManager.InvokeAction(response.actionResponse);
            }
        } else if(response.DialogueObject) {
            diologueUI.ShowDialogue(response.DialogueObject);
        } else {
            diologueUI.CloseDiologueBox();
        }
    }
}
