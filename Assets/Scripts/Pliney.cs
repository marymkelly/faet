using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pliney : MonoBehaviour
{
    public GameObject elevator;
    private bool movePliney = false;
    private Vector3 target;

    void Start()
    {
        target = new Vector3(10.36f, transform.position.y);

       if(GameObject.Find("Player").GetComponent<Player_Interactions>().metPliney){
           gameObject.GetComponentInParent<DialogueActivator>().NPC.Name = "Druid Pliney";
       }

       if(GameObject.Find("Player").GetComponent<Player_Interactions>().fragments >= 2) {
            elevator.SetActive(true);
            GameObject.Find("ElevatorPlatform").gameObject.SetActive(false);
            transform.position = target;
       }    
    }


    void Update()
    {
        if(movePliney) {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 2);
            if(transform.position.Equals(target)) movePliney = false;
        }
    }

    public void TalkedToPliney() {
        GameObject.Find("Player").GetComponent<Player_Interactions>().metPliney = true;
        gameObject.GetComponentInParent<DialogueActivator>().NPC.Name = "Druid Pliney";
    }

    public void StartPlineyCo(){
        StartCoroutine("PlineyGifts");
    }

    IEnumerator PlineyGifts() {
        GameObject cast = gameObject.transform.Find("Cast").gameObject;

        yield return new WaitUntil(() => GameObject.Find("Player").GetComponent<Player_Interactions>().fragments == 2);
        yield return new WaitForSeconds(1);

        cast.SetActive(true);
        cast.GetComponent<Animator>().SetTrigger("Cast");
        yield return new WaitForSeconds(1);
        cast.SetActive(false);
        elevator.SetActive(true);
        GameObject.Find("ElevatorPlatform").gameObject.SetActive(false);
        movePliney = true;
    }
}
