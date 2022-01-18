using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Spaced : MonoBehaviour
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
        
    //vars if consolidating with patrol paused
    // public int waitMinSeconds;
    // public int waitMaxSeconds;
    // public bool isRandom;

    void Start()
    {
        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, 0);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, 0);
    }

    void Update()
    {
        if(canPatrol) {
            StartPatrol();
            return;
        } else { 
            if(!isCounting) StartCoroutine("PatrolCounter");  
        }
    }

    void StartPatrol() {
        Vector3 thisPosition = new Vector3(transform.position.x, transform.position.y, 0);

        if (isRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed);
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

        // yield return new WaitUntil(() => !canPatrol); 
        yield return new WaitForSeconds((int)Random.Range(5.0f, 15.0f)); //5 - 15 sec

        canPatrol = true;
        isCounting = false;
    }

}
