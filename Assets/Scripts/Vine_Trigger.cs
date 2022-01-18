using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine_Trigger : MonoBehaviour
{
    public Vine vine; 
    public GameObject symbol;
    public Color symbolColor;
    private Color symbolOriginalColor;
    private Transform boss;

    void Start()
    {
        if(symbol) {
            symbolOriginalColor = symbol.GetComponent<SpriteRenderer>().color;
        }
        
        if(GameObject.FindGameObjectWithTag("Boss")) {
            boss = GameObject.FindGameObjectWithTag("Boss").transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Attack") && !boss.GetComponent<Boss_Two>().vineHit) {
            if(!vine.GetComponent<Vine>().triggered){
                Destroy(collision.gameObject);
                vine.GetComponent<Vine>().TriggerAttack();

                if(symbol) {
                    symbol.GetComponent<SpriteRenderer>().color = symbolColor;
                    StartCoroutine("ResetRuneColor");
                }
            }
        }
    }

    IEnumerator ResetRuneColor()
    {
        yield return new WaitForSeconds(2);
        // yield return new WaitUntil(() => !vine.GetComponent<Vine>().triggered);
        yield return new WaitUntil(() => !boss.GetComponent<Boss_Two>().vineHit);
        symbol.GetComponent<SpriteRenderer>().color = symbolOriginalColor;
    }
}
