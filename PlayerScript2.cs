//Mike Fortin, Basic Player Script
//All code in this file written by Mike Fortin thru 9/21
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerScript2 : MonoBehaviour {

	public float playerSpeed = 3f; //Used to set the players speed when moving
	public float playerHP = 100; //Used to store adjust and update the players health
	private Rigidbody2D playerBody; //Used to find and adjust the body of the player
	private Animator animator; //Used to set animation triggers off depending on movement

	// Used for initialization
	void Start () 
	{
		playerBody = FindObjectOfType<Rigidbody2D> ();
		playerBody.freezeRotation = true;
		animator = FindObjectOfType<Animator> ();
	}
	
	// This is how the player's movement and status will update 
	void FixedUpdate () {
		Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), 
			                         Input.GetAxisRaw ("Vertical"));
		GetComponent<Rigidbody2D> ().velocity = targetVelocity * playerSpeed; //moves the player

		//This series of ifs uses animation triggers according to directional movement
		//Priority animation given to left and right on diagonal movement
		if (targetVelocity.x < 0) 
		{
			animator.SetTrigger ("left");
		}
		else if (targetVelocity.x > 0) 
		{
			animator.SetTrigger ("right");
		}
		else if (targetVelocity.y > 0) 
		{
			animator.SetTrigger ("back");
		}
		else if (targetVelocity.y < 0)
		{
			animator.SetTrigger("forward");
		}


		//Kills the player if his health drops below 0
		if (playerHP <= 0) 
		{
			this.gameObject.SetActive(false);
		}
	}
}
