using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker_Color : MonoBehaviour
{
    public Color[] colors;
    public float transitionSpeed; 
    public float flickerLength;
    
    private Color originalColor;
    private float flickerCounter;

    void Start()
    {
        originalColor = GetComponent<SpriteRenderer>().color;
        flickerCounter = flickerLength; 
    }

    void Update()
    {
        // if(Input.GetKey(KeyCode.B)) { //for testing purposes
        //     flickerCounter = 0;   //set to 0 to start flicker 
        // }
        // if(GetComponentInParent<PineconeScript>()){
        if(TryGetComponent(out PineconeScript cone)){
            originalColor = cone.entryColor;
        }

        if(flickerCounter < flickerLength) {
            GetComponent<SpriteRenderer>().color = Color.Lerp(colors[0], colors[1], Mathf.PingPong((Time.time/transitionSpeed), 1));
            flickerCounter++;

        } else { //restore sprite to original color after reaching flickerLength
            GetComponent<SpriteRenderer>().color = originalColor;
        }
    }

    public void Flicker() {
        flickerCounter = 0;
    }
}
