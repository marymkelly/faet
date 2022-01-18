using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Leaf_Update : MonoBehaviour {

    private GameObject respawn;

    public int maxHealth;
    public int currentHealth;

    public Leaf_HealthBar leafBar;

    private float timeSafe = 60f; //seconds 
    private float lastTimeDamaged = 0f;
    private bool isCounting = false;
    private bool restoreRunning = false; //if restore coroutine is running

    
    [SerializeField] private GameObject defeatUI;
    public Text defeatStatus;

    void Start()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn");

        maxHealth = leafBar.GetSegmentTotal();

        if(currentHealth == 0) {
            currentHealth = maxHealth;  
        } else if (currentHealth < maxHealth) {
            StartCoroutine("HealthCounter");
            // StartCoroutine("RestoreTest", new int[] { 4, 6 });// if wanting to implement interval control
        }
    }

    public void TakeDamage(int damage = 1) {
        currentHealth -= damage;
        lastTimeDamaged = 0f;

        CancelCoroutines();

        if(currentHealth > 0) {
            leafBar.SetCurrentSegment(currentHealth);
            StartCoroutine("HealthCounter"); //start new counter
        } else {
            if(SceneManager.GetActiveScene().name == "Boss_One" || SceneManager.GetActiveScene().name == "Boss_Two") {
                Dead();
                defeatStatus.GetComponent<Text>().text = "You <color=#FF0000>Lost!</color>";
                GetComponent<Player_Interactions>().defeatOn = true;
                defeatUI.SetActive(true);
            } else {
                // Respawn();
                if(respawn) {
                    Respawn();
                } else {
                    Dead(); 
                }
            }
        }
    }

    void AddHealth() {
        if(currentHealth < maxHealth){
            currentHealth++; 
            leafBar.SetCurrentSegment(currentHealth);
        }
    }

    void Dead() {
        Debug.Log("Dead called " + this + " + health " + currentHealth);
        CancelCoroutines();
        currentHealth = 0;
        // this.gameObject.SetActive(false);
    }

    public void BossDeath() {
            GameObject.Find("GameData").GetComponent<GameData>().diedOnBoss = true;
            GameObject.Find("GameData").GetComponent<GameData>().Save();
            if(SceneManager.GetActiveScene().name == "Boss_One") {
                SceneManager.LoadScene(2);
            } else if(SceneManager.GetActiveScene().name == "Boss_Two") {
                SceneManager.LoadScene(3);
                // SceneManager.LoadScene(6);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Hazard")) {
            FindObjectOfType<AudioManager>().Play("Hazard");
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death")) {
            Respawn();
        } else if (collision.CompareTag("Nurse")) {
            CancelCoroutines();

            collision.gameObject.GetComponentInParent<Animator>().SetBool("isNursing", true);
            StartCoroutine ("NurseCounter"); //continues lastDamageTime
            if(currentHealth == maxHealth) {
                StartCoroutine("RestoreHealth", 0);
            } else {
                StartCoroutine("RestoreHealth", 5);
            }
        } else if (collision.name == "Enemy_Projectile(Clone)"){
                Destroy(collision.gameObject);
                TakeDamage(); 
        }
 
    }

    void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Nurse")) {
            StopCoroutine ("NurseCounter");
            collision.gameObject.GetComponentInParent<Animator>().SetBool("isNursing", false);

            if(restoreRunning) {
                StopCoroutine ("RestoreHealth"); 
            }

            if(currentHealth < maxHealth) {
                StartCoroutine("HealthCounter");
            }
        }
	}

    IEnumerator HealthCounter() {  //auto begins health point respawn after avoiding harm after x time
        isCounting = true;
        Debug.Log("Health Co Started");
        
        // Debug.Log("health counter started");	
		for (float currentCount = lastTimeDamaged; currentCount <= timeSafe; currentCount++){
            // Debug.Log("CURRENT COUNT " + currentCount);
            lastTimeDamaged = currentCount;
			// yield return new WaitForSeconds (Time.deltaTime); 
            // yield return new WaitForSeconds (Time.fixedDeltaTime);
            yield return new WaitForSeconds(1);
		}	
        // Debug.Log("health counter complete: " + lastTimeDamaged);	
        isCounting = false;	

        lastTimeDamaged = timeSafe;
        StartCoroutine ("RestoreHealth", 0);
	}

    IEnumerator RestoreTest(int[] arr)
    { //restore health
        int wait = arr[0];
        int interval = arr[1];

        restoreRunning = true;
        // Debug.Log("Waiting to Restore");
        // yield return new WaitUntil(() => !isCounting && canRestore);
        // yield return new WaitUntil(() => !isCounting);
        // Debug.Log("waiting " + wait);
        yield return new WaitForSeconds(wait);

        // Debug.Log("Starting to restore");
        for (int current = currentHealth + 1; current < maxHealth; current++)
        {
            // Debug.Log("running interval " + interval);
            // Debug.Log("CURRENT HEALTH " + currentHealth);
            // Debug.Log("CURRENT HEALTH COUNT " + current);
            currentHealth = current;
            leafBar.SetCurrentSegment(currentHealth);
            // yield return new WaitForSeconds (wait * Time.deltaTime);
            // yield return new WaitForSeconds (wait * Time.fixedDeltaTime);
            yield return new WaitForSeconds(interval);
        }

        // Debug.Log("Restored"); 
        FindObjectOfType<AudioManager>().Play("HealthRestored");
        currentHealth = 5;
        leafBar.SetCurrentSegment(currentHealth);
        restoreRunning = false;
    }

    IEnumerator RestoreHealth(int wait)	{ //restore health
        restoreRunning = true;
        Debug.Log("Restore Started");
        // Debug.Log("Waiting to Restore");
        // yield return new WaitUntil(() => !isCounting && canRestore);
        // yield return new WaitUntil(() => !isCounting);
        yield return new WaitForSeconds(wait);

        // Debug.Log("Starting to restore");
		for (int current = currentHealth + 1; current < maxHealth; current++){
            // Debug.Log("CURRENT HEALTH " + currentHealth);
            // Debug.Log("CURRENT HEALTH COUNT " + current);
            currentHealth = current;
			leafBar.SetCurrentSegment(currentHealth);
			// yield return new WaitForSeconds (wait * Time.deltaTime);
            // yield return new WaitForSeconds (wait * Time.fixedDeltaTime);
            yield return new WaitForSeconds(4);
		}

        // Debug.Log("Restored"); 
        FindObjectOfType<AudioManager>().Play("HealthRestored");
		currentHealth = 5;	
        leafBar.SetCurrentSegment(currentHealth);	
        restoreRunning = false;	
	}

    IEnumerator NurseCounter()	{ //continue tracking time last damaged
        while(lastTimeDamaged <= timeSafe){
            lastTimeDamaged++;
            yield return new WaitForSeconds(1);
        }

        lastTimeDamaged = timeSafe;
	}

    public void Respawn()
    {
        //reset damage timer (just in case)
        lastTimeDamaged = timeSafe;  //should be outside of damage area and do not have to wait for counter

        //reset leafbar UI
        currentHealth = maxHealth;
        leafBar.SetCurrentSegment(maxHealth);

        //move player
        FindObjectOfType<AudioManager>().Play("Respawn");
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.transform.position = respawn.transform.position;
    }

    public void CancelCoroutines() {  
        if(restoreRunning) {  //stop if restoring
            StopCoroutine ("RestoreHealth");
            restoreRunning = false;
        }

        if(isCounting) {  //stop if counting
            StopCoroutine ("HealthCounter");
            isCounting = false;
        }
    }
}



