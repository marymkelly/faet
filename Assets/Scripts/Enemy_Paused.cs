using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Paused : MonoBehaviour
{
        
    public Transform pointA;
    public Transform pointB;
    public bool isRight = true;
    public float speed = 0.3f;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;
    // Use this for initialization
    private bool canPatrol = false;
    private bool isCounting = false;

    public int pauseSeconds;

    void Start()
    {
        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, 0);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, 0);
    }

    void Update()
    {
        if(canPatrol) {
            // StartPatrol();
            // Debug.Log("patrol in paused");

            if(gameObject.GetComponent<Animator>() && (gameObject.GetComponent<Animator>().GetBool("isCounting") == true)) {
                gameObject.GetComponent<Animator>().SetBool("isCounting", false);
            }
            return;
        } else { 
            if(!isCounting) StartCoroutine("PatrolCounter");  

            if(gameObject.GetComponent<Animator>() && (gameObject.GetComponent<Animator>().GetBool("isCounting") ==  false)) {
                gameObject.GetComponent<Animator>().SetBool("isCounting", true);
            }
        }
    }

    

    void FixedUpdate() {
        if(canPatrol) {
            // Debug.Log("patrol fixed in paused, speed " + speed);
            StartPatrol();
        }
    }

    void StartPatrol() {
        // Vector3 thisPosition = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 thisPosition;

        if (isRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed);
            thisPosition = new Vector3(transform.position.x, transform.position.y, 0);
        
            if (thisPosition.Equals(pointBPosition))
            {
                //Debug.Log ("Position b");
                isRight = false;
                // GetComponent<SpriteRenderer>().flipX = true; //originally
                GetComponent<SpriteRenderer>().flipX = false;
                canPatrol = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed);
            thisPosition = new Vector3(transform.position.x, transform.position.y, 0);

            if (thisPosition.Equals(pointAPosition))
            {
                //Debug.Log ("Position a");
                isRight = true;
                // GetComponent<SpriteRenderer>().flipX = false; //originally
                GetComponent<SpriteRenderer>().flipX = true;
                canPatrol = false;
            }
        }
    }

    IEnumerator PatrolCounter() {
        isCounting = true;

        yield return new WaitForSeconds(pauseSeconds); 

        canPatrol = true;
        isCounting = false;
    }
}
