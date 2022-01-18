using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    public Sprite uncursedSprite;
    public Color uncursedColor;
    public int minFrags;
    public bool useBoth;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Interactions>().fragments >= minFrags) {
            if(uncursedSprite != null) {
                GetComponent<SpriteRenderer>().sprite = uncursedSprite;
                if(useBoth) {
                    GetComponent<SpriteRenderer>().color = uncursedColor;
                }
            } else {
                GetComponent<SpriteRenderer>().color = uncursedColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
