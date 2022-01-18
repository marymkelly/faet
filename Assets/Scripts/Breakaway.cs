using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breakaway : MonoBehaviour
{
    public int health = 15;
    private GameObject canvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {     
            int damage = (int)collision.GetComponent<Projectile>().GetDamage();
            TakeDamage(damage);
            Destroy(collision.gameObject);

            canvas = GameObject.Find("Canvas");
            canvas.transform.Find("Instruction").GetComponent<Text>().text = " ";
        }

        void TakeDamage(int d) {
            health -= d;
            if (health <= 0)
            {
                FindObjectOfType<AudioManager>().Play("EnemyDefeat");
                Destroy(gameObject);
            }
        }
    }
}
