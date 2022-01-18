using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Hazards : MonoBehaviour
{
    public GameObject hazard;
    public bool spawningHazards = false;
    private Vector2 targetPoint;
    public float speed = 2.0f;
    private int wait = 10;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(spawningHazards && (transform.position.x != targetPoint.x)) {
            Debug.Log("Moving towards target");
            transform.position =  Vector2.MoveTowards(transform.position, targetPoint, speed * 2 * Time.deltaTime);

            if(transform.position.x  == targetPoint.x) {
                Debug.Log("hit target");
            }
        }

        // if(Input.GetKeyDown(KeyCode.H)) {
        //     Debug.Log("Starting Hazard Spawn");
        //     StartCoroutine(SpawnGroup());
        // } 
    }

    public void StartHazardSpawn() {
        StartCoroutine(SpawnCounter());
    }
    
    private GameObject SpawnItem() {
        GameObject sticker = Instantiate(hazard) as GameObject;
        sticker.transform.position = this.transform.position;
        sticker.SetActive(true);
        return sticker;
    }

    IEnumerator SpawnGroup() {
        GameObject[] hazards = new GameObject[3];

        spawningHazards = true;

        for(int i = 0; i < 3; i++) {
            hazards[i] = SpawnItem();
            targetPoint = new Vector2(transform.position.x + ((GetComponent<Boss_Two>().movingLeft) ? 1 : -1), transform.position.y);

            yield return new WaitUntil(() => this.transform.position.x == targetPoint.x);
        }

        spawningHazards = false;

        foreach (GameObject h in hazards) {
            h.GetComponent<Rigidbody2D>().isKinematic = false;
            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator SpawnCounter() {
        yield return new WaitForSeconds(wait);
        if(GameObject.FindGameObjectsWithTag("Hazard").Length < 5 && GameObject.FindWithTag("Boss")) {
            StartCoroutine(SpawnGroup());
        }
    }

}
