using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 0.3f;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;
    // [HideInInspector]
    public bool triggered = false;

    private bool retracting = false;
    public bool bossHit = false;
    // private bool released = false;

    private Transform bossTransform;


    void Start()
    {
        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, 0);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, 0);
    }

    void Update()
    {
        Vector3 thisPosition = new Vector3(transform.position.x, transform.position.y, 0);

        if (triggered) {
            if(!retracting) {
                transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed);
                GetComponent<Animator>().SetBool("isExtending", true);

                if (thisPosition.Equals(pointBPosition))
                {
                    GetComponent<Animator>().SetBool("isExtending", false);
                    retracting = true;
                    // Debug.Log ("hit Position b");
                }
            } else {
                transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed);
                Vector3 positionMoved =  new Vector3(transform.position.x - thisPosition.x, transform.position.y - thisPosition.y, 0);

                if(bossHit) {
                        // Debug.Log("Trying to drag Boss " + positionMoved.x + " " + positionMoved.y + " boundsY " + bossTransform.GetComponent<BoxCollider2D>().bounds.size.y);
                        Vector3 direction = pointA.position - bossTransform.position;
                        float halfSize = bossTransform.GetComponent<BoxCollider2D>().bounds.size.y / 2;
                        // bossTransform.position += positionMoved; 
                        bossTransform.position = Vector3.MoveTowards(bossTransform.position, pointB.position, speed);

                        bossTransform.GetComponent<Animator>().SetBool("isTransitioning", true);

                        // if(bossTransform.position.y <= pointB.position.y + halfSize) {
                        if(bossTransform.position.y <= pointB.position.y) {
                            // bossTransform.position = new Vector2(bossTransform.position.x + positionMoved.x, pointB.position.y + halfSize);
                            bossTransform.position = new Vector2(bossTransform.position.x + positionMoved.x, pointB.position.y);
                            bossTransform.GetComponent<Animator>().SetBool("isTransitioning", false);
                            bossTransform.GetComponent<Animator>().SetBool("isBound", true);
                        }
                }

                if (thisPosition.Equals(pointAPosition)) {
                    retracting = false;
                    triggered = false;
                    bossHit = false;
                    // Debug.Log ("hit Position a");
                }
            }
        } 
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Boss")) {
            bossHit = true;  
            bossTransform = collision.transform;
            bossTransform.GetComponent<Animator>().SetTrigger("Grab");
        }

    }

    public void TriggerAttack() {
        triggered = true;
    }
}
