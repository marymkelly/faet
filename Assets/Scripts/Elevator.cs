using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Transform requestPoint;
    public float speed = 0.3f;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;
    private bool called = false;
    private bool onTheWay = false;
    private bool autoDescendPrimed = false; 
    private bool autoDescending = false;
    void Start()
    {
        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, 0);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, 0);
    }
    void FixedUpdate()
    {
        if(called || autoDescending) {
            MoveElevator();
        }
    }

    void MoveElevator() {
        Vector3 thisPosition = new Vector3(transform.position.x, transform.position.y, 0);

        if (!requestPoint) { //auto descend
            if ((thisPosition != pointAPosition)) {
                transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed);
            }
            else {
                called = false;
                autoDescending = false;
            }
        }


        if(requestPoint == pointA){ //lower point
            if (onTheWay) {
                transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed);

                if (thisPosition.Equals(pointAPosition)) {
                    StartCoroutine("PauseOnArrive");
                }
            } else {
                transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed);

                if (thisPosition.Equals(pointBPosition)){
                    called = false;
                    autoDescending = false;
                    requestPoint = null;
                    StartCoroutine("AutoDescend");
                }
            }
        }

        if(requestPoint == pointB) { //higher point
            if (onTheWay) {
                transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed);

                if (thisPosition.Equals(pointBPosition)){
                    StartCoroutine("PauseOnArrive");
                }
            } else {
                transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed);

                if (thisPosition.Equals(pointAPosition)){
                    called = false;
                    autoDescending = false;
                    requestPoint = null;
                    StartCoroutine("AutoDescend");
                }
            }
        }
    }
    public void TriggerElevator(Transform point) {
        requestPoint = point;

        if (autoDescendPrimed) {
            autoDescendPrimed = false;
            StopCoroutine("AutoDescend");
        }

        if(autoDescending) {
            autoDescending = false;
        }

        //check if elevator position to see if it needs to move to meet
        if(new Vector3(point.position.x, point.position.y, 0) != new Vector3(transform.position.x, transform.position.y, 0)) { 
            onTheWay = true;
        }

        called = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    public bool IsElevatorMoving() {
        return (called == true || autoDescending == true) ? true : false;
    }


    IEnumerator AutoDescend() { 
            autoDescendPrimed = true;

            yield return new WaitForSeconds(5);
            
            autoDescendPrimed = false;
            autoDescending = true;
    }


    IEnumerator PauseOnArrive()
    {
        yield return new WaitForSeconds(2);
        onTheWay = false;
    }
}
