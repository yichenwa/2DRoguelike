using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove : MonoBehaviour {

	// Use this for initialization
	public float BulletSpeed = 5;
	private PlayerScript2 player;
	//private bool bulletMoving;
	private Vector2 direction;
	//private BoxCollider2D bulletBox;
	public float strength = 50; //Bullets and slashing are equal to start

	void Start () 
	{
		//bulletBox = FindObjectOfType<BoxCollider2D> ();
		player = FindObjectOfType<PlayerScript2> ();

		//bulletMoving = false;

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

	void OnCollisionEnter2D (Collision2D thing)
	{
		if (thing.gameObject.name != "Player") //Will need changing if there are more boxcolliders introduced
		{
			thing.gameObject.SendMessageUpwards ("takeDamage", strength);
			Destroy (gameObject);
		}

	}
}
