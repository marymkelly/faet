using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Trigger : MonoBehaviour  //Script that lets the GameObject act as an elevator button
{
    public GameObject elevator;
    public Transform elevatorPoint;
    public GameObject symbol;
    public Color symbolColor;
    private Color symbolOriginalColor;

    private bool elevatorIsMoving;

    void Start() {
        if(symbol) {
            symbolOriginalColor = symbol.GetComponent<SpriteRenderer>().color;
        }
    }

    void Update() {
        elevatorIsMoving = elevator.GetComponent<Elevator>().IsElevatorMoving();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") && !elevatorIsMoving) {
            Destroy(collision.gameObject);

            elevator.GetComponent<Elevator>().TriggerElevator(elevatorPoint);

            if(symbol) {
                symbol.GetComponent<SpriteRenderer>().color = symbolColor;
                StartCoroutine("ResetRuneColor");
            }

        }
    }

    IEnumerator ResetRuneColor()
    {
        yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => !elevatorIsMoving);
        symbol.GetComponent<SpriteRenderer>().color = symbolOriginalColor;
    }
}
