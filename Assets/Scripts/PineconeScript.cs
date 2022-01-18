using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineconeScript : MonoBehaviour
{
    private bool canDamage = false;
    public float health;
    private GameObject player;
    public Color entryColor;

    void Start() {
        player = GameObject.FindWithTag("Player").gameObject;

        if(player.GetComponent<Player_Interactions>().destroyedPinecone) {
            Destroy(gameObject);
        }
    }

    void Update() {
        if(player.GetComponent<Player_Interactions>().fragments > 0 && !canDamage) {
            SetCanDamage();
        }
    }

    public void SetCanDamage() {
        canDamage = true;
        GetComponent<SpriteRenderer>().color = entryColor;
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

    private void TakeDamage(float d)
    {
        health -= d;

        // Debug.Log("Pinecone Damage " + d + " Health left " + health);

        if (health <= 0)
        {
            player.GetComponent<Player_Interactions>().destroyedPinecone = true;
            FindObjectOfType<AudioManager>().Play("EnemyDefeat");
            Destroy(gameObject);
        } else {
            gameObject.GetComponentInParent<Flicker_Color>().Flicker();
        }
    }

}

