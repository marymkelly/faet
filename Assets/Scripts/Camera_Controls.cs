using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controls : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;

    [Tooltip("Pressing up or down makes the camera move in that direction! Useful if you want to stop and look around.")]
    public float yLookDistance = 1f;

    [Tooltip("The amount in the x-axis the camera is offset from the player")]
    public float xOffset = 0f;
    [Tooltip("The amount in the y-axis the camera is offset from the player")]
    public float yOffset = 0f;

    [Tooltip("This makes smooth camera movement. Higher speeds mean the camera follows the player more closely")]
    public float speed = 2.0f;

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    private GameObject boss;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.Find("Boss_One");
    }

    // void Update() {
    //     // bool casting = player.GetComponentInChildren<PlayerShooter>().casting;
    //     // yLookDistance =  casting ? 0f : 0.6f;
    //     if(Input.GetAxis("Horizontal") < 0 ) { 
    //         xOffset *= -1;
    //     }

    // }

    void FixedUpdate()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = transform.position;
        // if (player.GetComponent<Player_Move_Update>().IsGrounded() || (position.y >= (player.transform.position.y + yOffset + Input.GetAxis("Vertical") * yLookDistance))) {
        if (player.GetComponent<Player_Move_Update>().IsGrounded() || (position.y >= (player.transform.position.y + yOffset + Input.GetAxis("Vertical") * yLookDistance))) {
            position.y = Mathf.Lerp(this.transform.position.y, Mathf.Clamp(player.transform.position.y + yOffset + Input.GetAxis("Vertical") * yLookDistance, yMin, yMax), interpolation);
        }

        if(boss) {
            // if(player.transform.position.x <= -4) {
            //     xOffset = 2;
            // } 
            // else if(player.transform.position.x > -4 && player.transform.position.x < -2) {
            //     xOffset = 2;
            // }
            //  else {
            //     xOffset = 4;
            // }

            if(((player.transform.position.x - boss.transform.position.x) > 2) && (boss.transform.position.x < player.transform.position.x) && player.transform.position.x > 0) {
                xOffset = Mathf.Lerp(-4, 0, (-1 * player.transform.position.x));
            } else {
                xOffset = Mathf.Lerp(4, -2, (-1 * player.transform.position.x));
            }

            if(!boss.GetComponent<Boss_One>().cameraAdjusting) {
                position.x = Mathf.Lerp(transform.position.x, Mathf.Clamp(player.transform.position.x + xOffset, xMin, xMax), interpolation);
                position.y = Mathf.Lerp(transform.position.y, Mathf.Clamp(player.transform.position.y + yOffset, yMin, yMax), interpolation);
                transform.position = position;
            }
        } else {
        // if(Input.GetAxis("Horizontal") < 0 ) {
        //     position.x = Mathf.Lerp(transform.position.x, Mathf.Clamp(player.transform.position.x - xOffset, xMin, xMax), interpolation);
        // } else {
        position.x = Mathf.Lerp(transform.position.x, Mathf.Clamp(player.transform.position.x + xOffset, xMin, xMax), interpolation);
        // }
        
        position.y = Mathf.Lerp(transform.position.y, Mathf.Clamp(player.transform.position.y + yOffset, yMin, yMax), interpolation);
        transform.position = position;
        }
    }
}
