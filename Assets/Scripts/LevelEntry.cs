using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEntry : MonoBehaviour
{
    public GameObject symbol;
    public GameObject barrier;
    private GameObject  player;
    public int minFragments;
    private int playerFragments;
    public Color entryColor;
    public Color symbolColor;
    private Color barrierOriginalColor;
    private Color symbolOriginalColor;
    private bool canEnter = false;

    public float transitionSpeed = 12.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        playerFragments = player.GetComponent<Player_Interactions>().fragments;
        barrierOriginalColor = barrier.GetComponent<SpriteRenderer>().color;
        symbolOriginalColor = symbol.GetComponent<SpriteRenderer>().color;
    
        if(playerFragments >= minFragments) { 
            barrier.GetComponent<PineconeScript>().SetCanDamage();
            barrier.GetComponent<SpriteRenderer>().color = entryColor;
            canEnter = true;
        }
    }

    void Update()
    {
        if(canEnter && !player.GetComponent<Player_Interactions>().destroyedPinecone) {
            ChangeColor();
        }
    }

    public void ChangeColor() {
        // barrier.GetComponent<SpriteRenderer>().color = Color.Lerp(barrierOriginalColor, entryColor, Time.time / transitionSpeed);
        barrier.GetComponent<SpriteRenderer>().color = entryColor;
        symbol.GetComponent<SpriteRenderer>().color = Color.Lerp(symbolOriginalColor, symbolColor, Time.time / transitionSpeed);
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(playerFragments >= minFragments) {
                //  = Color.Lerp(Color.white, Color.black, Time.time);
                Debug.Log("frags enough");
                canEnter = true;

            }
        } 

    }


}
