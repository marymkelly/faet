using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 0.7f;
    private float originalChaseSpeed;
    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private Vector2 moveTo;

    private float xDifference;
    public float minPlayerDist = 5.0f;

    // private Vector2 bounceVel = new Vector2(0.0f, 0.0f);
    private Vector2 bounceTarget = new Vector2(0.0f, 0.0f);

    private bool returning;
    private Enemy_Paused patrol;

    private bool canPatrol = true;

    void Start(){
        if(this.GetComponent<Rigidbody2D>()){
            rb = this.GetComponent<Rigidbody2D>();
        }

        originalPosition = this.transform.position;
        originalChaseSpeed = chaseSpeed;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        xDifference = Mathf.Abs(transform.position.x - player.position.x);

        patrol = this.GetComponent<Enemy_Paused>();

        if(!(patrol.pointA && patrol.pointB)) {
            canPatrol = false;
            patrol.enabled = false;
            // Debug.Log("NO PATROL POINTS " + canPatrol);
        }

        // if(patrol.pointA && patrol.pointB) {
            // Debug.Log("HAS PATROL POINTS " + canPatrol);
        // }
    }

    // void Update(){
        
    //     // rb.isKinematic = returning || ? true : false;

    //     // if((transform.position.x <= (originalPosition.x + 0.5f)) && (transform.position.x >= (originalPosition.x - 0.5f))) {
    //     //     // if(returning) {
    //     //     //     Debug.Log("update returned");
    //     //     //     returning = false;
    //     //     // } 

    //     //     if(!returning && !patrol.enabled) {  //check incase coroutine was not called
    //     //         patrol.enabled = true;
    //     //     }
    //     // }
    // }


    private void FixedUpdate() {
        xDifference = Mathf.Abs(transform.position.x - player.position.x);

        if(xDifference >= minPlayerDist && canPatrol) {  //player out of bounds
            // rb.isKinematic = true;

            // if(!returning && !((transform.position.x >= patrol.pointA.position.x) && (transform.position.x <= patrol.pointB.position.x))) {
            //     returning = true;
            //     StartCoroutine("ReturnToPatrol");
            // }

            if(!((transform.position.x >= patrol.pointA.position.x) && (transform.position.x <= patrol.pointB.position.x))) { //if out of patrol bounds
                if(!returning) {
                    if(!patrol.enabled) {
                        returning = true;
                        StartCoroutine("ReturnToPatrol");
                    }
                } else {
                    // Debug.Log("moving to return position");
                    chaseSpeed = originalChaseSpeed;

                    Vector3 direction = originalPosition - transform.position;
                    direction.Normalize();
                    moveTo = direction;

                    moveEnemy(moveTo);
                }
            } else { //if inside patrol bounds

                //orig
                // if(returning && ((transform.position.x <= (originalPosition.x + 0.5f)) && (transform.position.x >= (originalPosition.x - 0.5f)))) {
                //     Debug.Log("update returned");
                //     if(!patrol.enabled) {
                //         returning = false;
                //     }
                // }

                if(returning) {
                    if((transform.position.x <= (originalPosition.x + 0.5f)) && (transform.position.x >= (originalPosition.x - 0.5f))){
                        // Debug.Log("update returned");
                        if(!patrol.enabled) {
                            returning = false;
                        }
                    } else { 
                        // Debug.Log("still returning to return position");
                        chaseSpeed = originalChaseSpeed;

                        Vector3 direction = originalPosition - transform.position;
                        direction.Normalize();
                        moveTo = direction;

                        moveEnemy(moveTo);
                    }
                }
            }

        } else {
            if(chaseSpeed > 0f) {
                if(patrol.enabled) {
                    patrol.enabled = false;
                    rb.isKinematic = false;
                }

                // Debug.Log("enemy is chasing");

                // if(bounceVel == Vector2.zero) {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                if(bounceTarget == Vector2.zero) {
                    // Debug.Log("bounceTarg 0 ");
                    Vector3 direction = player.position - transform.position;
                    direction.Normalize();
                    moveTo = direction;

                    moveEnemy(moveTo);
                } else {
                   transform.position = Vector2.MoveTowards(transform.position, bounceTarget, Time.fixedDeltaTime);
    
                    if(Mathf.Approximately(transform.position.x, bounceTarget.x)) {
                        // bounceVel = Vector2.zero;
                        bounceTarget = Vector2.zero;
                        chaseSpeed = 0.45f;
                    }
                }
            }
        }

        // if(returning){
        //     chaseSpeed = originalChaseSpeed;

        //     Vector3 direction = originalPosition - transform.position;
        //     direction.Normalize();
        //     moveTo = direction;

        //     moveEnemy(moveTo);
        // }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.name == "Player") {
            // if(bounceVel == Vector2.zero) {
            if(bounceTarget == Vector2.zero) {
                Vector2 bounceVel = collision.relativeVelocity;
                bounceTarget  = new Vector2(transform.position.x + (1.0f * bounceVel.x), transform.position.y + (-1.0f * bounceVel.y));
                chaseSpeed = originalChaseSpeed;
            }
            // transform.Translate(new Vector3(1.0f, 0.0f, 0.0f) * collision.relativeVelocity.x);
            // transform.Translate(new Vector3(0.0f, -1.0f, 0.0f) * collision.relativeVelocity.y);
        }
    }

    void moveEnemy(Vector2 direction){ //chase
    // Debug.Log("MOVE ENEMY");
        GetComponent<SpriteRenderer>().flipX = (direction.x > 0) ? true : false;
        rb.MovePosition((Vector2)transform.position + (direction * chaseSpeed * Time.fixedDeltaTime));
        rb.velocity = new Vector2(direction.x, direction.y) * chaseSpeed;
    }

    IEnumerator ReturnToPatrol() {
        // patrol.enabled = false;
        // Debug.Log("Return to patrol coroutine started");

        yield return new WaitUntil(() => !returning);
            // Debug.Log("restoring patrol");
        // if(!patrol.enabled) {
            // chaseSpeed = originalChaseSpeed;
            patrol.enabled = true;
            rb.isKinematic = true;
        // }
    }
}

