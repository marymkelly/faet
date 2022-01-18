using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Two : MonoBehaviour
{

    public int maxHealth = 200;
    public int currentHealth;
    public float bossSpeed = 2;
    public float patrolSpeed = 1;
    public Transform[] stopPoints;
    private bool movingToStop = false;
    int currentStop;

    Vector3 targetPoint;

    [HideInInspector]
    public bool movingLeft = false;

    public Transform pointA, pointB;

    public Boss_Healthbar bossBar;

    private Transform player;
    private SliderJoint2D playerJoint;
    private Vector3 pointAPosition, pointBPosition, pointADifference, pointBDifference;

    public float frequency = 1f;
    public float magnitude = 1f;

    Vector3 pos;
    
    [HideInInspector]
    public bool vineHit = false;

    public GameObject defeatUI;
    public Text defeatStatus;
    public GameObject retryButton;
    public GameObject zap;

    public bool inIntro = true;

    private GameObject spkVine;
    private GameObject borderVine;

    public bool isDefeated = false;

    public GameObject background;
    private Transform[] bgs;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        bossBar.SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerJoint = player.GetComponent<SliderJoint2D>();
        playerJoint.enabled = false;

        spkVine = GameObject.Find("SpikeyVine").gameObject;
        borderVine = GameObject.Find("Border Vine_01").gameObject;
        spkVine.SetActive(false);
        borderVine.SetActive(false);
        zap.SetActive(false);
        // targetPoint = new Vector3(stopPoints[currentStop].position.x, stopPoints[currentStop].position.y, 0);

        pos = transform.position;
        
        int i = 0;
        bgs = new Transform[background.transform.childCount];

        foreach (Transform t in background.transform)
        {
            bgs[i] = t;
            bgs[i].GetComponent<Rotate_Colors>().enabled = false;

            i++;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inIntro) {
            if(movingToStop) {
                player.GetComponent<Player_Interactions>().isInteracting = true;
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * (bossSpeed * 3.1f));
                player.position = Vector3.MoveTowards(player.position, GameObject.Find("PlayerStop").transform.position, Time.deltaTime * (bossSpeed * 3.2f));
                zap.SetActive(true);

                Vector3 zapDirection = (new Vector3(transform.position.x + 1.2f, transform.position.y + .43f, 0) - player.position).normalized;
                float angle = Mathf.Atan2(zapDirection.y, zapDirection.x) * Mathf.Rad2Deg;
                zap.transform.eulerAngles = new Vector3(0, 0, angle);

                if(transform.position.x >= 2) {
                    zap.transform.position = new Vector3(zap.transform.position.x, -2.0f);
                    // zap.transform.InverseTransformPoint(new Vector3(zap.transform.position.x, -4.0f));
                }

                if(player.position.x >= (spkVine.transform.position.x + 1)) {
                    StartCoroutine(DropVine());
                }
 
                if(player.position.x >= GameObject.Find("PlayerStop").transform.position.x - (player.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2)) {
                    // Debug.Log("hit player stop");
                    zap.SetActive(false);
                }

            }

        } else {
            if(!GetComponent<Spawn_Hazards>().spawningHazards && !vineHit && !movingToStop && !isDefeated) {
                if(movingLeft) {
                    transform.position = Vector3.MoveTowards(transform.position, pointB.position, patrolSpeed * Time.deltaTime);

                    if(transform.position.Equals(pointB.position)) {
                        movingLeft = false;
                    }

                } else {
                    transform.position = Vector3.MoveTowards(transform.position, pointA.position, patrolSpeed * Time.deltaTime);
            
                    if(transform.position.Equals(pointA.position)) {
                        movingLeft = true;
                    }
                }
            }
        }
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;
        bossBar.SetHealth(currentHealth);

        if(currentHealth <= 0) {
            FindObjectOfType<AudioManager>().Play("BossDefeat");
            StopAllCoroutines();
            StartCoroutine(BossDefeated());
            // Destroy(gameObject); 
            // gameObject.SetActive(false);



            // defeatStatus.GetComponent<Text>().text = "You <color=#00dba0>Win!</color>";
            // player.GetComponent<Player_Interactions>().defeatOn = true;
            // retryButton.SetActive(false);
            // defeatUI.SetActive(true);
        } 

    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Vine")) {
            vineHit = true;  
            TakeDamage(5);
            StartCoroutine(BreakCount(collision));
        } else if (collision.name == "Projectile(Clone)") {
            // Debug.Log("Collision Damage " + collision.GetComponent<Projectile>().GetDamage());
            int damage = (int)collision.GetComponent<Projectile>().GetDamage();

            if(!inIntro) {
                TakeDamage(damage);
            }

            Destroy(collision.gameObject);    
        
        }
    }

    IEnumerator BossDefeated() {
        isDefeated = true;
        player.GetComponent<Player_Interactions>().defeatedBossTwo = true;

        GetComponent<Animator>().ResetTrigger("Grab");
        GetComponent<Animator>().ResetTrigger("Flicker");
        GetComponent<Animator>().ResetTrigger("Break");
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetTrigger("Defeat");
        yield return new WaitForSeconds(3);
        GetComponent<Animator>().ResetTrigger("Defeat");
        GetComponent<Animator>().SetBool("isDefeated", true);
        foreach (Transform t in bgs)
        {
            t.GetComponent<Rotate_Colors>().enabled = true;
        }
        yield return new WaitForSeconds(2); //changed from 3
        GameObject.Find("GameData").GetComponent<GameData>().Save();
        GameObject.Find("LevelLoader").gameObject.GetComponent<LevelLoader>().LoadNextLevel(7);




    }

    public void RunPostDiologueMove() {
        StartCoroutine(MoveToStopPoint(1));
    }

    IEnumerator BreakCount(Collider2D collision) {
        // Debug.Log("Break count started, bosshit   " + collision.GetComponent<Vine>().bossHit);
        yield return new WaitUntil(() => !collision.GetComponent<Vine>().bossHit);
        // Debug.Log("Bosshit over"); 
        GetComponent<Animator>().SetBool("isBound", true);
        yield return new WaitForSeconds(2); 
        GetComponent<Animator>().ResetTrigger("Grab");
        yield return new WaitForSeconds(4);
        GetComponent<Animator>().SetBool("isTransitioning", false);
        GetComponent<Animator>().SetTrigger("Flicker"); 
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().ResetTrigger("Flicker"); 
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetTrigger("Break");
        GetComponent<Animator>().SetBool("isBound", false); 
        vineHit = false;
        // GetComponent<Animator>().ResetTrigger("Break");
    }

    IEnumerator DropVine() {
        spkVine.SetActive(true);
        spkVine.GetComponent<Animator>().SetTrigger("Drop");
        yield return new WaitForSeconds(1);
        spkVine.GetComponent<Animator>().ResetTrigger("Drop");
        borderVine.SetActive(true);
        spkVine.SetActive(false);
        zap.SetActive(false);
    }   

    IEnumerator MoveToStopPoint(int point) {
        player.GetComponent<SliderJoint2D>().enabled = false;
        Vector3 target = new Vector3(stopPoints[point].position.x, stopPoints[point].position.y, 0);
        targetPoint = target;

        movingToStop = true;

        yield return new WaitUntil(() => transform.position == target);

        pos = transform.position;
        Destroy(playerJoint);
        movingToStop = false;
        yield return null;

        player.GetComponent<Player_Interactions>().FreePlayer();
        player.GetComponent<Player_Interactions>().bossDialogue = false;
        inIntro = false;

        // GetComponent<Spawn_Hazards>().StartHazardSpawn();
    }
}
