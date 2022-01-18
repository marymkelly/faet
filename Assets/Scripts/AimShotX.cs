using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimShotX : MonoBehaviour
{
    public Transform aimDot;
    public Transform circlePath;
    public Transform crossHair;
    public float speed = 5.0f;
    private Vector2 pointA;
    private Vector2 pointB;

    private float circleWidth;
    private float radius;
    
    [HideInInspector]
    public bool aimOn = false;
    int holdCount = 0;

    private float counter = 0.0f;
    private float animationTime = 150f;
    private float angle = 0.0f;
    private float xPos;
    private float yPos;

    // public Text textCounter;

    void Start()
    {
        aimDot.gameObject.SetActive(false);
        circlePath.gameObject.SetActive(false);
        crossHair.gameObject.SetActive(false);
        circleWidth = circlePath.GetComponent<SpriteRenderer>().bounds.size.x * 2; //bounds get positive value
        radius = circleWidth / 2;
        xPos = radius;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)){
            aimOn = false;
        } 

        if(Input.GetKeyUp(KeyCode.X)){
        // Debug.Log("C Key up " + holdCount);
            if(aimOn){
                aimDot.gameObject.SetActive(false);
                circlePath.gameObject.SetActive(false);
                crossHair.gameObject.SetActive(false);
                aimOn = false;
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Interactions>().FreePlayer(false);
    
            holdCount = 0;
        } 


        // if(Input.GetKeyUp(KeyCode.X)){
        //     if(aimOn && (holdCount < 10)) { 
        //         aimDot.gameObject.SetActive(false);
        //         circlePath.gameObject.SetActive(false);
        //         crossHair.gameObject.SetActive(false);
        //         // aimOn = false;
        //     }
            
        //     GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Interactions>().FreePlayer(false);
            
        //     holdCount = 0;
        // } 


        if(Input.GetKey(KeyCode.X)){
            holdCount++;            
            
            if(holdCount >= 10) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Interactions>().FreezePlayer(false);
                aimDot.gameObject.SetActive(true);
                circlePath.gameObject.SetActive(true);
                crossHair.gameObject.SetActive(true);
                aimOn = true;

            if(Input.GetKey(KeyCode.LeftArrow)){
                counter += animationTime * Time.deltaTime;

                if(counter >= 180) {
                    counter = 180;
                }
            } else if(Input.GetKey(KeyCode.RightArrow)){
                counter -= animationTime * Time.deltaTime;

                if(counter <= 0) {
                    counter = 0;
                }
            }
            
            angle  = counter; 
            
            if(angle != Mathf.Round(counter)){ 
                angle = Mathf.Round(counter);
            }

            float aimX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float aimY = Mathf.Sin(angle * Mathf.Deg2Rad);

            xPos = Mathf.Clamp(radius * aimX, -radius, radius);
            yPos = Mathf.Clamp(radius * aimY, 0, radius);
            }
        } 
    }

    public Vector3 GetAimWorldPoint() {
        return aimDot.transform.position;
    }

    void FixedUpdate() {
        aimDot.transform.localPosition = new Vector3(xPos, yPos, 0);
        crossHair.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
