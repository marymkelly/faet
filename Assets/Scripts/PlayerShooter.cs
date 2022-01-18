using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {

    [SerializeField] private GameObject projectile;
    private bool facingRight = true;
    [SerializeField] private int MaxActiveShots;
    private Rigidbody2D rbdPlayer;
    private GameObject player;
    
    private bool isClimbing = false;

    private Camera cam;
    private Transform castTransform; // "PewPew" child of player
    private Transform projectileTransform; //projectile

    Vector3 mousePosition;
    private bool xIsPressed = false;
    private GameObject lastShot;

    Player_Magic_Update playerMagic;
    private float currentMagic;

    private float castPower;
    private bool isInteracting = false;

    public bool casting = false;
    private bool lastFacingRight;
    private bool usingAim;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        rbdPlayer = player.GetComponent<Rigidbody2D>(); 
        isClimbing =  player.GetComponent<Animator>().GetBool("IsClimbing");  
        playerMagic = player.GetComponent<Player_Magic_Update>(); 
        castPower = player.GetComponent<Player_Magic_Update>().magicPower; 
        currentMagic = playerMagic.GetCurrentMagic();
        castTransform = player.transform.Find("PewPew");
        usingAim = player.GetComponent<AimShotX>().aimOn;

        cam = Camera.main;
        mousePosition = GetWorldMousePos();

        isInteracting = player.GetComponent<Player_Interactions>().isInteracting;
        isClimbing =  player.GetComponent<Animator>().GetBool("IsClimbing");
        lastFacingRight = player.GetComponent<Player_Move_Update>().PlayerLastMovedRight();
        
    }

    // Update is called once per frame
    void Update() {   
        isInteracting = player.GetComponent<Player_Interactions>().isInteracting;
        isClimbing =  player.GetComponent<Animator>().GetBool("IsClimbing");
        currentMagic = playerMagic.GetCurrentMagic();
        castPower = player.GetComponent<Player_Magic_Update>().magicPower;
        usingAim = player.GetComponent<AimShotX>().aimOn;
        casting = usingAim;

        if(lastFacingRight != player.GetComponent<Player_Move_Update>().PlayerLastMovedRight()) {
            lastFacingRight = player.GetComponent<Player_Move_Update>().PlayerLastMovedRight();
            // Debug.Log("Last facing RIGHT " +lastFacingRight);
        }
        

        if (!(isInteracting && isClimbing)){
            //mouse controls
            // mousePosition = GetWorldMousePos();
            // MouseAimMagic();
            // MouseCastMagic();

            //keyboard controls
            // ControlLastShot();

            if (GameObject.FindGameObjectsWithTag("Attack").Length < MaxActiveShots && currentMagic >= castPower){
            
                CheckDirection();

                if(Input.GetKeyUp(KeyCode.X)) {    
                    CastShot(casting);
                }

                // else if(Input.GetKeyUp(KeyCode.C)) {
                //     CastShot(casting);
                // } 

            } else {
                if(Input.GetKeyDown(KeyCode.X)) {
                    FindObjectOfType<AudioManager>().Play("SelectionError");
                }
            }  

            //necessary for ControlLastShot() abilities
            // if(Input.GetKeyUp(KeyCode.X)) { 
            //     casting = false;
            //     xIsPressed = false;
            //     rbdPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
            // } 
        }
    }

    private void CheckDirection() {     
        if (facingRight){
            if (!lastFacingRight) {
                transform.RotateAround(rbdPlayer.transform.position, Vector3.up, 180f);
                facingRight = false;
            }
        } else if (lastFacingRight){
            transform.RotateAround(rbdPlayer.transform.position, Vector3.up, 180f);
            facingRight = true;
        }
    }

    private void CastShot(bool aiming) {
        GameObject shot = Instantiate(projectile);
        shot.transform.position = this.transform.position;
        shot.GetComponent<Projectile>().SetDamage(castPower);

        shot.SetActive(true);

        if(aiming){
            // projectileTransform = shot.GetComponent<Projectile>().transform;
            Vector3 castToPosition = player.GetComponent<AimShotX>().GetAimWorldPoint();

            Vector3 castDirection = (castToPosition - shot.transform.position).normalized;  //vector math
            // Vector3 castDirection = (castToPosition - projectileTransform.position).normalized;  //vector math
            float angle = Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg;
            // this.transform.eulerAngles  = new Vector3(0, 0, angle); 
            shot.GetComponent<Projectile>().Setup(castDirection, new Vector3(0, 0, angle), player.GetComponent<AimShotX>().speed);

        } else {
            shot.GetComponent<SpriteRenderer>().flipX = !facingRight;
            float playerSpeed = rbdPlayer.velocity.x;
            shot.GetComponent<Projectile>().Launch(playerSpeed, facingRight);
        }

        // if(!aiming){
        //     shot.GetComponent<SpriteRenderer>().flipX = !facingRight;
        // }

        // shot.SetActive(true);
        // float playerSpeed = rbdPlayer.velocity.x;
        // shot.GetComponent<Projectile>().Launch(playerSpeed, facingRight);
        // lastShot = shot;

        // if(aiming){
        //     projectileTransform = shot.GetComponent<Projectile>().transform;
        //     Vector3 castToPosition = player.GetComponent<AimShotX>().GetAimWorldPoint();

        //     Vector3 castDirection = (castToPosition - projectileTransform.position).normalized;  //vector math
        //     float angle = Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg;
        //     this.transform.eulerAngles  = new Vector3(0, 0, angle); 

        //     shot.GetComponent<Projectile>().Setup(castDirection, this.transform.eulerAngles);
        // }

        lastShot = shot;
        playerMagic.UseMagic(10);
        CastAnimation();
    }

    private void CastAnimation() {
        if(facingRight) {
            player.GetComponent<Animator>().Play("Base Layer.CAST", 0, 1f);
        } else {
            player.GetComponent<Animator>().Play("Base Layer.CAST_left", 0, 1f);
        }

        FindObjectOfType<AudioManager>().Play("PlayerCastMagic");
    }



    // All Following Functions currently not used

    void ControlLastShot() {
            Vector2 shootDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if(xIsPressed && (lastShot != null)) {
                casting = true;
                if((Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") !=  0)) {
                    rbdPlayer.constraints = RigidbodyConstraints2D.FreezeAll;
                    Rigidbody2D moveShot = lastShot.GetComponent<Rigidbody2D>();
                    moveShot.MovePosition(moveShot.position + (shootDir) * 8.0f * Time.deltaTime);
                } 
            }
    }

    private void MouseAimMagic() {  //for aiming with mouse
        // Vector3 mousePosition = getWorldMousePos();
        Vector3 castDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg;
        castTransform.eulerAngles  = new Vector3(0, 0, angle); 
    }

    private void MouseCastMagic() { //for shooting with mouse
        // Vector3 mousePosition = getWorldMousePos();
        Vector2 rbdRelMousePos = rbdPlayer.GetPoint(mousePosition);

        if(Input.GetMouseButtonDown(0) && !GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Interactions>().isInteracting) {
            if(!isClimbing) {
                if (facingRight) {
                    if (Input.GetAxis("Horizontal") < 0 || rbdRelMousePos.x < 0) {
                        transform.RotateAround(rbdPlayer.transform.position, Vector3.up, 180f);
                        facingRight = false;
                    }
                } else if (Input.GetAxis("Horizontal") > 0 || rbdRelMousePos.x > 0) {
                    transform.RotateAround(rbdPlayer.transform.position, Vector3.up, 180f);
                    facingRight = true;
                }

                if (GameObject.FindGameObjectsWithTag("Attack").Length < MaxActiveShots && currentMagic >= 20) {
                    GameObject shot = Instantiate(projectile);
                    shot.transform.position = this.transform.position;

                    shot.SetActive(true);
                    lastShot = shot;

                    float playerSpeed = rbdPlayer.velocity.x;
                    shot.GetComponent<Projectile>().Launch(playerSpeed, facingRight);

                    projectileTransform = shot.GetComponent<Projectile>().transform;
                    Vector3 castPosition = mousePosition;

                    Vector3 castDirection = (castPosition - projectileTransform.position).normalized;
                    shot.GetComponent<Projectile>().Setup(castDirection, castTransform.eulerAngles);
                    
                    if(facingRight) {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Play("Base Layer.CAST", 0, 1f);
                    } else {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().Play("Base Layer.CAST_left", 0, 1f);
                    }

                    FindObjectOfType<AudioManager>().Play("PlayerCastMagic");
                    playerMagic.UseMagic(10);
                } else {
                    FindObjectOfType<AudioManager>().Play("SelectionError");
                }
            }
        }
    }

    public Vector3 GetWorldMousePos() {
        cam = Camera.main;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
