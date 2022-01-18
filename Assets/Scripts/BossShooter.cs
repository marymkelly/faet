using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    private Rigidbody2D rbdBoss;

    private GameObject player;
    private bool facingRight = false;

    private Transform projectileTransform; //projectile
    private bool stop = true;
    private bool cosCancelled = false;
    private bool bossTwo = false;

    void Start()
    {
        rbdBoss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Rigidbody2D>(); 
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        if(GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Two>()) bossTwo = true;

        if(!bossTwo) {
            StartCoroutine("Shooter"); 
        }
        
    }

    void Update() {
        // if(Input.GetKeyDown(KeyCode.M)) {
        //     stop = true;
        // }

        if(!GameObject.FindGameObjectWithTag("Player") || (bossTwo && GetComponentInParent<Boss_Two>().isDefeated)) {
            stop = true;
        } else {
            if((Mathf.Abs(transform.position.x - player.transform.position.x) > 8) || (bossTwo && (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Two>().vineHit || GetComponentInParent<Boss_Two>().inIntro))) {
                StopCoroutine("Shooter");
                StopCoroutine("BossCast");
                cosCancelled = true;
                stop = true;
            } else {
                stop = false;
                if(cosCancelled) {
                    cosCancelled = false;
                    StartCoroutine("Shooter");
                }
            }   

        }
    }

    IEnumerator BossCast() {
        GameObject shot = Instantiate(projectile);
        shot.transform.position = this.transform.position;
        shot.SetActive(true);

        facingRight = (this.transform.position.x < player.transform.position.x) ? true : false;
        
        float bossSpeed = rbdBoss.velocity.x;
        shot.GetComponent<Projectile>().Launch(bossSpeed, facingRight);

        projectileTransform = shot.GetComponent<Projectile>().transform;
        Vector3 castPosition = player.transform.position;  //where you're trying to cast at

        Vector3 castDirection = (castPosition - projectileTransform.position).normalized;  //vector math
        float angle = Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg;
        this.transform.eulerAngles  = new Vector3(0, 0, angle); 
        // shot.transform.eulerAngles  = new Vector3(0, 0, angle); 

        shot.GetComponent<Projectile>().Setup(castDirection, this.transform.eulerAngles);
        // shot.GetComponent<Projectile>().Setup(castDirection, shot.transform.eulerAngles);

        if(GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>().name == "Boss_One") {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().Play("Base Layer.BossOne_Cast", 0, 1f);
        } else if(GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>().name == "Boss_Two") {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().Play("Base Layer.BossTwo_CAST", 0, 1f);
        };
        yield return null;
    }

    IEnumerator Shooter() {
        // while(!stop && (bossTwo ? !GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss_Two>().inIntro : true)) {
        while(!stop) {
            yield return new WaitForSeconds(Random.Range(2.0f, 8.0f));
            StartCoroutine("BossCast");
        }
    }
}
