using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damage : MonoBehaviour
{
    public int health; 
    [SerializeField] private Object[] floras;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack")) {
            int damage = (int)collision.GetComponent<Projectile>().GetDamage();
            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
    }

    private void TakeDamage(int d)
    {
        health -= d;

        if(health <= 0) {
            FindObjectOfType<AudioManager>().Play("EnemyDefeat");

            GameObject drop = Instantiate(floras[Mathf.FloorToInt(Random.Range(0f, 2f))] as GameObject);
            drop.transform.position = this.transform.position;
            drop.SetActive(true);
            Destroy(gameObject);
        } 
    }
}
