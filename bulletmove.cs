using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove : MonoBehaviour {

	// Use this for initialization
	public float BulletSpeed ;
	private PlayerScript2 player;
	private bool bulletMoving;
	private Vector2 direction;
	private BoxCollider2D bulletBox;
	public float strength = 75;

	void Start () 
	{
		bulletBox = FindObjectOfType<BoxCollider2D> ();
		player = FindObjectOfType<PlayerScript2> ();

		bulletMoving = false;

		if (player.getDirectionString () == "r") {
			shootRight ();

		}
		if (player.getDirectionString () == "l") {
			shootLeft ();
			//transform.rotation()
		}
		if (player.getDirectionString () == "u") {
			shootUp ();

		}
		if (player.getDirectionString () == "d") {
			shootDown ();

		} 

	}

	void Update() 
	{
		
		transform.Translate (direction * BulletSpeed * Time.deltaTime);

	}
	void shootRight()
	{
		direction = Vector2.right;
	}
	void shootLeft()
	{
		direction = Vector2.left;
	}
	void shootUp()
	{
		direction = Vector2.up;
	}
	void shootDown()
	{
		direction = Vector2.down;
	}

	void OnTriggerEnter2D (Collider2D thing)
	{
		thing.SendMessageUpwards ("takeDamage", strength);
	}
}
