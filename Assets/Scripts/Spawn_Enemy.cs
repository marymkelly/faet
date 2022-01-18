using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 screenBounds;
    private Vector3 bossBounds;

    public GameObject enemyPf;
    public float respawnRate = 4f;
    private bool playerOutOfBounds = false; 

    void Start(){
        if(this.GetComponent<Rigidbody2D>()){
            rb = this.GetComponent<Rigidbody2D>();
        }

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        
        if(GameObject.FindWithTag("Boss")) {
            GetComponent<Animator>().Play("Base Layer.BossOne_Cast_Up", 0, 1f);
        }
        StartCoroutine(SpawnCycle());
    }

    void Update() {
            if(Mathf.Abs(transform.position.x - GameObject.FindWithTag("Player").GetComponent<Transform>().position.x) > 8) {
                playerOutOfBounds = true;
            } else {
                playerOutOfBounds = false;
            }
    }

    private void spawnEnemy() {
        GameObject enemy = Instantiate(enemyPf) as GameObject;
        bossBounds = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        enemy.transform.position = new Vector3(bossBounds.x - 2, bossBounds.y, bossBounds.z);
        enemy.SetActive(true);
    }

    IEnumerator Spawnerator() {
        while(GameObject.FindGameObjectsWithTag("Enemy").Length < 1 && GameObject.FindWithTag("Boss")) {

            if(playerOutOfBounds) {
                yield return new WaitUntil(() => !playerOutOfBounds);
            }

            if(GameObject.FindWithTag("Boss")) {
                GetComponent<Animator>().Play("Base Layer.BossOne_Cast_Up", 0, 1f);
                yield return new WaitForSeconds(0.7f);
            }

            spawnEnemy();
            yield return new WaitForSeconds(respawnRate);
        }

        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle() {
        yield return new WaitForSeconds(10);

        if(GameObject.FindWithTag("Boss")) {
            StartCoroutine(Spawnerator());
        }
    }
}

