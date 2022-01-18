using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_Update : MonoBehaviour {

    public float playerSpeed = 5;
    public int playerJumpPower = 250;
    private float moveX;
    private float moveY;

    [Tooltip("Only change this if your character is having problems jumping when they shouldn't or not jumping at all.")]
    public float distToGround = 1.0f;
    private bool inControl = true;

    [Tooltip("Everything you jump on should be put in a ground layer. Without this, your player probably* is able to jump infinitely")]
    public LayerMask GroundLayer;

    private float doubleJumpPower;
    private bool canDoubleJump;

    private bool isFalling = false;

    public float moveSpeed = 5f;
    Vector2 movement; 

    private bool isJumping;
    private bool canClimb;
    private bool onFloor = false;
    private Rigidbody2D rb;

    private Vector2 target; 

    float maxVelY = 0f;
    float fallTime = 0f;
    float originalGravityScale;

    private bool movedRight = true;



    // bool isGrounded;


    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        
        // if (inControl)
        // {
        //     PlayerMove();
        // }

        if(!gameObject.GetComponent<Player_Interactions>().isInteracting)
        {
            PlayerMovement();
        }

    }

    void FixedUpdate() {
        if (!gameObject.GetComponent<Player_Interactions>().isInteracting){
            // if (!IsGrounded() && rb.velocity.y < 0) {
            if (rb.velocity.y < 0 && !canClimb && !IsGrounded()){
                fallTime += Time.time;
                maxVelY += rb.velocity.y; 

                if(!isFalling) {
                    if (rb.velocity.x == 0 && (isJumping || fallTime > 300)) {
                        GetComponent<Animator>().Play("Base Layer.PreFALL", 0, 1f);
                        GetComponent<Animator>().SetBool("IsFalling", true);
                        isFalling = true;
                    }  
                }

                    // if(!isFalling && !IsGrounded() && !canClimb) {
                    // if(!isFalling && !IsGrounded()) {
                    //     if (rb.velocity.x == 0 && (isJumping || (!IsGrounded() && fallTime > 300))) {
                    //         GetComponent<Animator>().Play("Base Layer.PreFALL", 0, 1f);
                    //         GetComponent<Animator>().SetBool("IsFalling", true);
                    //         isFalling = true;
                    //     }
                    // }

                    // if(!isFalling && !IsGrounded()) {
                    //     isFalling = true;
                    // }
                    // if ((rb.velocity.x == 0 && isJumping) || (rb.velocity.x == 0 && !IsGrounded() && fallTime > 300)) {
                    //     GetComponent<Animator>().Play("Base Layer.PreFALL", 0, 1f);
                    //     GetComponent<Animator>().SetBool("IsFalling", true);
                    // }                   
            }

            if(isFalling && IsGrounded()) {
                isJumping = false;
                isFalling = false;
                GetComponent<Animator>().SetBool("IsFalling", false);

                float velDelta = maxVelY / ((9.81f * originalGravityScale) * rb.mass); //change in velocity

                if ((velDelta + 80) <= -100) {
                    // Debug.Log("FALL DAMAGE " + velDelta);
                    FallDamage(velDelta);
                    FindObjectOfType<AudioManager>().Play("Fell");
                }

                maxVelY = 0f;
                fallTime = 0f;

            } else if(isJumping) { 
                if (rb.velocity.y > 0)
                {
                    maxVelY += rb.velocity.y;
                } 
            }

            PlayerFixedMove();
        }
    } 

    void PlayerMovement() {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        moveX = movement.x;
        moveY = movement.y;

        GetComponent<Animator>().SetFloat("Horizontal", movement.x);
        GetComponent<Animator>().SetFloat("Vertical", movement.y);
        GetComponent<Animator>().SetFloat("Speed", movement.sqrMagnitude);

        GetComponent<Animator>().SetBool("IsFalling", isFalling);

        // isGrounded = onGround();

            if (moveX != 0) {
                movedRight = moveX > 0 ? true : false;
                GetComponent<Animator>().SetBool("IsRunning", true);
            } else {
                GetComponent<Animator>().SetBool("IsRunning", false);
            }

            if(Input.GetButton("Jump") && !canClimb && !isJumping) {
                GetComponent<Animator>().SetBool("IsSquat", true);
            } else {
                GetComponent<Animator>().SetBool("IsSquat", false);
            }


            if (Input.GetButtonUp("Jump")) {
                GetComponent<Animator>().SetBool("IsSquat", false);
                if (IsGrounded()) {
                // if(onGround()){
                    canDoubleJump = true;
                    Jump();
                } else {
                    if (canDoubleJump) {  //double jump
                        doubleJumpPower = playerJumpPower;
                        GetComponent<Rigidbody2D>().AddForce(Vector2.up * doubleJumpPower);
                        GetComponent<Animator>().Play("Base Layer.JUMP", 0, 1f);
                        FindObjectOfType<AudioManager>().Play("Jump");
                        canDoubleJump = false;
                    }
                }
            }
    }

    void PlayerFixedMove() {
        if ((Input.GetAxis("Vertical") != 0) && isJumping && canClimb) {
            isJumping = false;

            if(isFalling) {
                maxVelY = 0f;
                isFalling = false;
                GetComponent<Animator>().SetBool("IsFalling", false);
            }
        }

        if(canClimb && !isJumping) {
            if(canClimb){
                gameObject.GetComponent<Animator>().SetBool("IsClimbing", true);
            }

            if (moveX != 0 || moveY != 0) {
                rb.gravityScale = 0;
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //x and y movement 
                if(canClimb){
                    rb.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);  //affects velocity after leaving moss
                }
            } else {
                rb.gravityScale = 0;
                rb.MovePosition(rb.position); //x and y movement 
            }

        } else {
            rb.gravityScale = originalGravityScale;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

            if (moveX != 0 || isJumping) {
                GetComponent<Animator>().SetBool("IsRunning", true);
            } else {
                GetComponent<Animator>().SetBool("IsRunning", false);
            }
        }
    }

    public bool IsGrounded() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distToGround, GroundLayer);
        if (hit.collider != null) {            
            return true;
        }
        return false;
    }

    void Jump() {
        isJumping = true;
        rb.AddForce(Vector2.up * playerJumpPower);
        gameObject.GetComponent<Animator>().Play("Base Layer.JUMP", 0, 1f);
        FindObjectOfType<AudioManager>().Play("Jump");
    }

    void FallDamage(float deltaVel) {
        int healthSegments = (int) Mathf.Floor(deltaVel / -100);

        gameObject.GetComponent<Player_Leaf_Update>().TakeDamage(healthSegments);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Save")){
            // Debug.Log("SAVING");    
            GameObject.Find("GameData").GetComponent<GameData>().Save();
        }

        if (collision.CompareTag("Moss") || collision.CompareTag("Bark"))
        {
            canClimb = true;
        } 

        // if (collision.CompareTag("Floor"))
        // {
        //     Debug.Log("On Floor");
        //     onFloor = true;
        // } 

    }

    void OnTriggerExit2D(Collider2D collision) {
		if (collision.CompareTag("Moss") || collision.CompareTag("Bark")) {
            canClimb = false;
            gameObject.GetComponent<Animator>().SetBool("IsClimbing", false);
        }

        // if (collision.CompareTag("Floor"))
        // {
        //     Debug.Log("Left Floor");
        //     onFloor = false;
        // } 
	}

    public bool PlayerLastMovedRight() {
        return movedRight;
    }
    

    public void SetControl(bool b) {
        inControl = b;
    }

    bool onGround() {
        float extraHeight = 0.01f;
        RaycastHit2D rcHit = Physics2D.Raycast(GetComponent<PolygonCollider2D>().bounds.center, Vector2.down, GetComponent<PolygonCollider2D>().bounds.extents.y + extraHeight, GroundLayer);
        // RaycastHit2D rcHit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.down, boxCollider2d.bounds.extents.y + extraHeight, GroundLayer);
        Color rayColor;

        if(rcHit.collider != null) {
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        Debug.DrawRay(GetComponent<PolygonCollider2D>().bounds.center, Vector2.down * (GetComponent<PolygonCollider2D>().bounds.extents.y + extraHeight), rayColor);
        // Debug.DrawRay(boxCollider2d.bounds.center, Vector2.down * (boxCollider2d.bounds.extents.y + extraHeight), rayColor);
        Debug.Log(rcHit.collider);

        return rcHit.collider != null;

    }
}
