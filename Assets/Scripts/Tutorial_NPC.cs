using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial_NPC : MonoBehaviour
{
    private Text message; //npc message
    public string nickname;
    private bool notHit;
    private GameObject canvas;
    private GameObject controls;
    private GameObject instruction; 
    private Text hint; //ui messages / on-screen hints
    private Collider2D cldr;

    private bool playerColliding = false;
    private bool needFlip=false;

    string[][] tutorialNpc = { new string[] { "Oh man, oh man, oh man....", "Is this really happening?", "I think the mushrooms I had were a little too magical", "Do you see those strange symbols in boxes!?", "They just vanished!", "But anyway, should really get out of here...", "And watch out for sharp object or else you'll have a bad trip!"}, new string[] { "If I were you, I'd get to higher ground.", "Watch out for the thorns and prickles!", "There's probably a council meeting in the Trunk District", "(mutters to self)", "I'll catch you on the flip side", "Are you still here or am I seeing things?" } };
    void Start()
    {
        //Create Text component of a canvas object through unity named "Message"
        message = GameObject.Find("Message").GetComponent<Text>();
        canvas = GameObject.Find("Canvas");
        controls = canvas.transform.Find("Controls").gameObject;
        instruction = canvas.transform.Find("Instruction").gameObject;
        hint = instruction.GetComponent<Text>();
        cldr = GameObject.Find("Player").gameObject.GetComponent<Collider2D>();
        
        message.text = "";
        notHit=true;
    }

    void Update() {
        if(!notHit && playerColliding) {
            if(Input.GetKeyDown(KeyCode.E)) StartCoroutine(EngageInteraction(cldr, DisplayControls(GameObject.Find("Player").gameObject.GetComponent<Collider2D>())));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && notHit)
        {
            playerColliding = true;
            
            if (nickname.ToLower() == "tutorial") {
                if((other.gameObject.transform.position.x < transform.position.x) && !GetComponent<Patrol>().isRight) {
                    needFlip = true;
                    GetComponent<SpriteRenderer>().flipX = true;
                }

                StartCoroutine(EngageInteraction(other, displayText(other, tutorialNpc[0])));
            }
        }
        //Message to display if main dialogue is done
        else if(other.gameObject.tag == "Player" && !notHit)
        {
            playerColliding = true;
            
            if (nickname.ToLower() == "tutorial" && !other.gameObject.GetComponent<Player_Interactions>().isInteracting) {
                message.text = tutorialNpc[1][(int)Mathf.Floor(Random.value * tutorialNpc[1].Length)];
                hint.text = "Press <color=#00dba0>E</color> to view Controls";
            }
        }
    }


    IEnumerator EngageInteraction(Collider2D other, IEnumerator next) {
        other.gameObject.GetComponent<Player_Interactions>().isInteracting = true;
        other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponentInParent<Patrol>().speed = 0;

        yield return StartCoroutine(next);

        other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponentInParent<Patrol>().speed = 0.015f;
        other.gameObject.GetComponent<Player_Interactions>().isInteracting = false;
    }

    IEnumerator displayText(Collider2D other, string[] messageArr)
    {

        foreach (string text in messageArr)
        {
            message.text = text;

            while (!(Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")))
            {
                yield return null;
            }

            if (text.EndsWith("!?"))
            {
                yield return StartCoroutine(DisplayControls(other));
            }

            yield return null;
        }

        if(needFlip) {
            GetComponent<SpriteRenderer>().flipX = false;
            needFlip = false;
        }
        
        notHit = false;
    }

    IEnumerator DisplayControls(Collider2D other) {
        controls.SetActive(true);
        hint.text = "Press <color=#FF0000>Enter/Return</color> to continue";
        yield return null;
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        controls.SetActive(false);
        hint.text = "";
        other.gameObject.GetComponent<Player_Interactions>().SetHasReviewedControls(true);
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerColliding = false;

            if(message) {
                message.text = "";
            }

            if (!other.gameObject.GetComponent<Player_Interactions>().isInteracting)
            {
                hint.text = "";
            }
        }
    }
}