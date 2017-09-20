//Mike Fortin, Basic Player Script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerScript2 : MonoBehaviour {

	public float playerSpeed = 3f;
	public float playerHP = 100;
	private Rigidbody2D playerBody;
	private Animator animator;
	// Use this for initialization
	void Start () 
	{
		playerBody = FindObjectOfType<Rigidbody2D> ();
		playerBody.freezeRotation = true;
		animator = FindObjectOfType<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), 
			                         Input.GetAxisRaw ("Vertical"));
		GetComponent<Rigidbody2D> ().velocity = targetVelocity * playerSpeed;

		if (targetVelocity.y > 0) 
		{
			animator.SetTrigger ("back");
		}
		if (targetVelocity.y < 0)
		{
			animator.SetTrigger("forward");
		}
		if (targetVelocity.x < 0) 
		{
			animator.SetTrigger ("left");
		}
		if (targetVelocity.x > 0) 
		{
			animator.SetTrigger ("right");
		}

		if (playerHP <= 0) 
		{
			this.gameObject.SetActive(false);
		}
	}
}
