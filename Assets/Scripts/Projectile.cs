using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float lifeTime = 2.0f;
    private int lifeTimeFrames = 0;
    private int timeLived = 0;

    [SerializeField] private float relativeVelocity = 1f;
    private float velocity = 0f;

    // //mod
    private float speed = 8.0f;
    private Vector3 castDirection;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;

    private float damage = 1f;


	void Start () { 
    
        lifeTimeFrames = (int)(lifeTime / Time.fixedDeltaTime);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // xMax =  screenBounds.x + objectWidth;
        // xMin =  screenBounds.x * -1 - objectWidth;
        // yMax =  screenBounds.y + objectHeight;
        // yMin =  screenBounds.y * -1 - objectHeight;

        xMax = Camera.main.GetComponent<Camera_Controls>().xMax;
        xMin = Camera.main.GetComponent<Camera_Controls>().xMin;
        yMax = Camera.main.GetComponent<Camera_Controls>().yMax;
        yMin = Camera.main.GetComponent<Camera_Controls>().yMin;
	}
	
    public void Launch(float emitVelocity, bool goingRight)
    {
        if (goingRight)
        {
            velocity = emitVelocity + relativeVelocity;
        }
        else
        {
            velocity = emitVelocity - relativeVelocity;
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * velocity;
    }

    
    public void Setup(Vector3 castDir, Vector3 euler, float vel = 8.0f) {
            this.castDirection = castDir;
            this.transform.eulerAngles = euler;
            this.speed = vel;
    }
    

	void FixedUpdate () {
		if (timeLived < lifeTimeFrames)
        {
            timeLived++;
        } else
        {
            Destroy(gameObject);
        }

        if(transform.position.x > xMax + 6 || transform.position.y > yMax + 4 || transform.position.x < xMin - 6 || transform.position.y < yMin - 4) {
            Destroy(gameObject);
        }

        // if(transform.position.x > 38|| transform.position.y > 4 || transform.position.x < -11 || transform.position.y < -4) {
        //     Destroy(gameObject);
        // }

        
	}

    // void LateUpdate(){
    //     Vector3 viewPos = transform.position;
    //     viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
    //     viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
    //     transform.position = viewPos;
    // }
    void Update() {
        // xMax =  screenBounds.x + objectWidth;
        // xMin =  screenBounds.x * -1 - objectWidth;
        // yMax =  screenBounds.y + objectHeight;
        // yMin =  screenBounds.y * -1 - objectHeight;

        transform.position += castDirection * speed * Time.deltaTime;
    
        // if(transform.position.x > xMax || transform.position.y > yMax || transform.position.x < xMin || transform.position.y < yMin) {
        // if(transform.position.x > 38|| transform.position.y > 4 || transform.position.x < -11 || transform.position.y < -4) {
        //     Destroy(gameObject);
        // }
    }

    public void SetDamage(float d) {
        this.damage = d;
    }

    public float GetDamage() {
        return damage;
    }
}