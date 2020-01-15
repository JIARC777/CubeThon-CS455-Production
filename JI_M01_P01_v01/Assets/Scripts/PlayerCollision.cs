using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	// Grab a reference to the movement script

	public PlayerMovement movement;
	// Set up listener for collision
	void OnCollisionEnter(Collision collInstnace)
	{
		if (collInstnace.collider.tag == "Obstacle" || collInstnace.collider.tag == "Seeker")
		{
			// Deactivate movement script upon impact
			Debug.Log("Hit Obstacle");
         	movement.enabled = false;
			FindObjectOfType<GameManager>().EndGame();
			
		}
	}
}
