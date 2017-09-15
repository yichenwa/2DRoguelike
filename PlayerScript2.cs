using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
public class PlayerScript2 : MonoBehaviour {

	public float playerSpeed = 3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), 
			                         Input.GetAxisRaw ("Vertical"));
		GetComponent<Rigidbody2D> ().velocity = targetVelocity * playerSpeed;
		
	}
}
