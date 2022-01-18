using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriarScript : MonoBehaviour
{
    // Start is called before the first frame 
    private GameObject player;
    public Color color;
    private int frags;
    private bool canDamage;
    public float health;
    public GameObject legendary;
    public Color lightLegend;

    void Start()
    {   
        player = GameObject.Find("Player").gameObject;
        frags = player.GetComponent<Player_Interactions>().fragments;
        if(frags > 1) {
            GetComponent<SpriteRenderer>().color = color;
            canDamage = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") && canDamage)
        {
            int damage = (int)collision.GetComponent<Projectile>().GetDamage();
            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Attack") && canDamage)
        {
            float damage = collision.collider.GetComponent<Projectile>().GetDamage();
            TakeDamage(damage);
            Destroy(collision.gameObject);

            legendary.GetComponent<SpriteRenderer>().color = lightLegend;
        }
    }


    private void TakeDamage(float d)
    {
        health -= d;

        Debug.Log("Briar Damage " + d + " Health left " + health);

        if (health <= 0)
        {
            FindObjectOfType<AudioManager>().Play("EnemyDefeat");
            player.GetComponent<Player_Interactions>().destroyedBriar = true;

            Destroy(gameObject);
        } else {
            gameObject.GetComponentInParent<Flicker_Color>().Flicker();
        }
    }

}
