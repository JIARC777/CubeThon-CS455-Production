//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    // grab Rigidbody from Player Model for Physics
    public Rigidbody playerRB;
    // handle to control force used to move player forward
    public float zForcePerFrame = 1000f;
    // handle to control force applied to X+ and X- for strafing movement 
    public float xForcePerFrame = 100f;
    
    // Update is called once per frame
    void Update()
    {
        // Use Dynamic force per frame for movement
        // Continuous forward movement system
    //    playerRB.AddForce(0, 0, zForcePerFrame * Time.deltaTime); 
        
        // Handle user input for player control
        // If key "D" is pressed move right
        if (Input.GetKey("w"))
        {
            playerRB.AddForce(0, 0, zForcePerFrame * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey("s"))
        {
            playerRB.AddForce(0, 0, -zForcePerFrame * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey("d"))
		{
            // ForceMode used to ignore inertia when changing direction
            playerRB.AddForce(xForcePerFrame * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        } 
        // if key "A" is pressed move left
        if (Input.GetKey("a"))
        {
            playerRB.AddForce(-xForcePerFrame * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (playerRB.position.y < -1)
		{
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
 