//Mike Fortin, Used For Bullets Movement Mechanics
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemybulletmove : MonoBehaviour
{

	// Use this for initialization
	public float BulletSpeed = 5;
	private PlayerScript2 player;
	//private bool bulletMoving;
	private Vector2 direction;
	//private BoxCollider2D bulletBox;
	public float strength = 50; //Bullets and slashing are equal to start

	void Start()
	{
		player = FindObjectOfType<PlayerScript2>();
		shootRight();

	}

	//Moves the bullet across the screen
	void Update()
	{

		transform.Translate(direction * BulletSpeed * Time.deltaTime);

	}

	//Used to set the shooting direction
	void shootRight()
	{
		direction = Vector2.right;
	}

	//This function makes bullets do damages to enemies and destroys them on collision 
	void OnCollisionEnter2D(Collision2D thing) 
	{
		if (thing.gameObject.tag == "Player" && player.canTakeDamage == true) //Will need changing if there are more boxcolliders introduced
		{
			player.playerHP -= strength;
			Destroy(gameObject);
		}
		if (thing.gameObject.tag != "Enemy")
		{
			Destroy(gameObject);
		}

	}
}