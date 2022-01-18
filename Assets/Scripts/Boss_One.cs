using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_One : MonoBehaviour {

    public int maxHealth = 100;
    public int currentHealth;
    public float bossSpeed = 2;
    public float patrolSpeed = 1;
    public Transform[] stopPoints;
    int currentStop;

    public Transform pointA, pointB;

    public Boss_Healthbar bossBar;

    private Transform player;

    private float xDifference;
    private float yDifference;
    private bool playerOnLeft = true;
    private bool movingLeft = false;
    Vector3 targetPoint;
    Vector3 pos, localScale;
    private Vector3 pointAPosition, pointBPosition, pointADifference, pointBDifference;

    public float frequency = 20f;
    public float magnitude = 1f;

    private bool running = false;
    private bool movingToStop = false;

    public float playerDist = 5.0f;

    float quarterDamage;
    int damageQuarter = 3;

    public GameObject defeatUI;
    public Text defeatStatus;
    public GameObject retryButton;

    private float dist;
    [SerializeField]

    private GameObject fragThree;
    private float t = 1f;

    private bool fadeWoods = false;
    private Transform cameraMain;
    private float cameraTargetX;
    private Vector3 cameraTarget;
    public bool cameraAdjusting = false;
    private Vector3 velocity = Vector3.zero;
    private GameObject levelLoader;

    void Start() {
        cameraMain = GameObject.Find("Main Camera").transform;
        levelLoader = GameObject.Find("LevelLoader").gameObject;

        currentHealth = maxHealth;
        bossBar.SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPoint = new Vector3(stopPoints[currentStop].position.x, stopPoints[currentStop].position.y, 0);

        pos = transform.position;

        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, pointA.position.x);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, pointB.position.x);
        pointADifference = new Vector3(pointA.position.x - transform.position.x, pointA.position.y - transform.position.y, pointA.position.z - transform.position.z);
        pointBDifference = new Vector3(pointB.position.x - transform.position.x, pointB.position.y - transform.position.y, pointB.position.z - transform.position.z);
        quarterDamage = maxHealth / 4;

        StartCoroutine(InitStopPoint(2));
    }

    // Update is called once per frame
    void Update()
    {
        //37.64f
        //-10.28

        if(cameraAdjusting) {
            // cameraMain.position = Vector3.SmoothDamp(cameraMain.position, cameraTarget, ref velocity, 0.05f);
            cameraMain.position = Vector3.MoveTowards(cameraMain.position, cameraTarget, 2f * Time.deltaTime);
        }

        dist = Vector3.Distance(player.transform.position, transform.position); //distance between boss and player

        if(transform.position.x > 31.7f) {
            currentStop = 3;
            transform.position = new Vector3(31.7f, transform.position.y, transform.position.z);
        }

        if(transform.position.x < -7.74f) {
            currentStop = 0;
            transform.position = new Vector3(-7.74f, transform.position.y, transform.position.z);
        }

        if(Input.GetAxis("Horizontal") > 0 && transform.position.x >= 27.5f && currentStop != 3 && !movingToStop) {
            currentStop = 3;
            StartCoroutine(MoveToStopPoint(currentStop));
        }

        if(currentStop == 3 && dist <= 2.5f && !movingToStop) {
            currentStop = 0;
            StartCoroutine(MoveToStopPoint(currentStop));
        }

        if (transform.position.x <= pointAPosition.x) {
            movingLeft = false;
        }

        if (transform.position.x >= pointBPosition.x) {
            movingLeft = true;
        }

        playerOnLeft = (player.position.x < transform.position.x) ? true : false;
        GetComponent<SpriteRenderer>().flipX = playerOnLeft ? false : true;

        xDifference = Mathf.Abs(transform.position.x - player.position.x);
        yDifference = Mathf.Abs(transform.position.y - player.position.y);
    
        // if(Input.GetKey(KeyCode.K)) {
        //     StartCoroutine(MoveToStopPoint(1));
        //     // transform.position = Vector3.MoveTowards(transform.position, targetPoint, (Time.deltaTime * bossSpeed));
        // }

        if(movingToStop) {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * bossSpeed);
        }


        if((currentHealth <= (quarterDamage * damageQuarter)) && (currentHealth > quarterDamage * (damageQuarter - 1))) { 
            damageQuarter--;

            if(currentStop < 3){
                currentStop++;
            } else {
                currentStop = 0;
            }

            StartCoroutine(MoveToStopPoint(currentStop));
        }

        // if(GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Boss_One_DESTROY")) {
        //     Debug.Log("Animator state defeat " + GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Defeat"));
        // }

    }

    void FixedUpdate() {
        if(xDifference >= playerDist) {
            running = false;
        }

        if(!movingToStop && currentStop != 3) {
            if((Input.GetAxis("Horizontal") > 0 && xDifference < playerDist) || (Input.GetAxis("Horizontal") == 0 && running)) {
                if(playerOnLeft) {
                    transform.Translate(Vector3.right * Time.fixedDeltaTime * bossSpeed);
                } else {
                    transform.Translate(Vector3.left * Time.fixedDeltaTime * bossSpeed);
                }

                pos = transform.position;
                pointA.position = new Vector3(pointADifference.x + transform.position.x, pointA.position.y, pointA.position.z);
                pointB.position = new Vector3(pointBDifference.x + transform.position.x, pointB.position.y, pointB.position.z);
                pointAPosition = new Vector3(pointA.position.x, pointA.position.y, pointA.position.x);
                pointBPosition = new Vector3(pointB.position.x, pointB.position.y, pointB.position.x);
                
                running = true;

            } else {
                if(movingLeft) {
                    MoveLeft();
                } else {
                    MoveRight();
                }
            }
        }
    }

    void MoveRight() {
        pos += Vector3.right * Time.fixedDeltaTime * patrolSpeed;
        transform.position = pos + Vector3.up * (Mathf.Sin(Time.time * frequency) * magnitude);
    }

    void MoveLeft() {
        pos += Vector3.left * Time.fixedDeltaTime * patrolSpeed;
        transform.position = pos + transform.up * (Mathf.Sin(Time.time * frequency) * magnitude);
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;

        bossBar.SetHealth(currentHealth);

        if(currentHealth <= 0) {
            if(!player.GetComponent<Player_Interactions>().defeatedBossOne) {
                player.GetComponent<Player_Interactions>().defeatedBossOne = true;
            }

                StartCoroutine("DefeatBoss");
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies) {
                    Destroy(enemy);
                }

                GameObject.Find("Boss Canvas").gameObject.SetActive(false);
                GameObject.Find("Caster").gameObject.SetActive(false);

        } else {
            gameObject.GetComponentInParent<Flicker_Color>().Flicker();
        }
                
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == "Projectile(Clone)")
        {
            // Debug.Log("Collision Damage " + collision.GetComponent<Projectile>().GetDamage());
            int damage = (int)collision.GetComponent<Projectile>().GetDamage();

            TakeDamage(damage);
            Destroy(collision.gameObject);            
        }
    }

    IEnumerator InitStopPoint(int wait) {
        yield return new WaitForSeconds(wait);

        currentStop = 1;
        StartCoroutine(MoveToStopPoint(currentStop));
    }
    IEnumerator AdjustCamera() {
        // Mathf.Clamp(player.transform.position.x + xOffset, xMin, xMax); //current
        // float startingX = camera.position.x;
        // float targetX = Mathf.Clamp(player.transform.position.x - camera.GetComponent<Camera_Controls>().xOffset, camera.GetComponent<Camera_Controls>().xMin, camera.GetComponent<Camera_Controls>().xMax);

        cameraTargetX = Mathf.Clamp((player.transform.position.x + (-1f * cameraMain.GetComponent<Camera_Controls>().xOffset)), cameraMain.GetComponent<Camera_Controls>().xMin, cameraMain.GetComponent<Camera_Controls>().xMax);

        Vector3 target = new Vector3(cameraTargetX, cameraMain.position.y, cameraMain.position.z);
        cameraTarget = target;

        cameraAdjusting = true;
        yield return new WaitUntil(() => cameraMain.position == cameraTarget);
        // yield return new WaitUntil(() => !cameraAdjusting);
        cameraAdjusting = false;
        cameraMain.GetComponent<Camera_Controls>().xOffset *= -1f;
    }

    IEnumerator MoveToStopPoint(int point) {
        if(point == 0) {
            StartCoroutine(AdjustCamera());
        }

        Vector3 target = new Vector3(stopPoints[point].position.x, stopPoints[point].position.y, 0);
        targetPoint = target;

        movingToStop = true;

        yield return new WaitUntil(() => transform.position == target);

        pos = transform.position;
        pointA.position = new Vector3(pointADifference.x + transform.position.x, pointADifference.y + transform.position.y, pointA.position.z);
        pointB.position = new Vector3(pointBDifference.x + transform.position.x, pointBDifference.y + transform.position.y, pointB.position.z);
        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, pointA.position.x);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, pointB.position.x);

        movingToStop = false;
        yield return null;
    }

    IEnumerator DefeatBoss() {
        FindObjectOfType<AudioManager>().Play("BossDefeat");
        GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().SetTrigger("Defeat");
    
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Boss_One_DESTROY"));
        GameObject.Find("LevelSetup").GetComponent<LevelSetup>().SetFadeWoods();
        if(cameraMain.GetComponent<Camera_Controls>().xOffset < 0) {
            StartCoroutine(AdjustCamera());
        }
        yield return new WaitForSeconds(.8f);
        if(player.GetComponent<Player_Interactions>().fragments >= 3) {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().StartLoadingLevel(8);
            Destroy(gameObject);
            
        } else {
            fragThree.transform.position = this.transform.position;
            fragThree.SetActive(true);
            Destroy(gameObject);
        }
    }      
}