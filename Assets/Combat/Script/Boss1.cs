// Mike Fortin, BossScript
//For more in depth comments see EnemyScript
//Very similar with minor size, speed and strength differences
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
//[RequireComponent (typeof (Animator))]
public class Boss1 : MonoBehaviour {

	public float enemySpeed = 1.5f; //Used to set the enemies walking speed
	public float enemyHP = 1000;
	private Vector2 playerPosition; //Will be used to locate the player's location
	private PlayerScript2 player; //Finds the player object by finding the PlayerScript2 object
	private Rigidbody2D enemyBody; //Used to control the enemy body 
	private bool canWalk = true; //Will be used to make the enemy pause if a condition is met
	private bool canDamage = true;
	private Vector2 targetPosition;
	private Animator animator;
	private bool playerInRange;

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerScript2> ();
		enemyBody = FindObjectOfType<Rigidbody2D> ();
		enemyBody.freezeRotation = true;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (canWalk == false) 
		{
			StartCoroutine (walkDelay()); //After he's been stopped, call the function to restart his walking
		}

		if (canWalk == true && playerInRange == true) 
		{
			//This is where the magic happens
			playerPosition = (player.transform.position);
			transform.position = Vector2.MoveTowards(transform.position, playerPosition, enemySpeed * Time.deltaTime);
			//StartCoroutine (targetPosition);

		}

		if (enemyHP <= 0) 
		{
			this.gameObject.SetActive (false);
		}
	}

	public void takeDamage(float damage)
	{
		enemyHP -= damage;
	}

	void OnCollisionStay2D(Collision2D thing) //If the enemy collides with the player, inflict damage to him
	{
		if (thing.gameObject.name == "Player" && player.canTakeDamage == true && canDamage == true) { //The new canTakeDamage works // with parrying
			player.playerHP -= 100; //Damage done, can be adjusted
			canDamage = false;
			StartCoroutine (damageDelay ());
		} 
		canWalk = false; 
	}

	IEnumerator walkDelay() //Used to get the enemy walking after a collision
	{
		yield return new WaitForSeconds (2.5f); //THE KEY TO VICTORY
		canWalk = true;
	}

	IEnumerator damageDelay()
	{
		yield return new WaitForSeconds (.5f);
		canDamage = true;
	}

	/*IEnumerator standStill()
	{
		yield return new WaitForSeconds (1.5f);
		canWalk = false;
	}
	*/
	void OnTriggerEnter2D(Collider2D thing)
	{
		if (thing.gameObject.tag == "Player") 
		{
			playerInRange = true;
		}
	}
	void OnTriggerExit2D(Collider2D thing)
	{
		if (thing.gameObject.tag == "Player") 
		{
			playerInRange = false;
		}
	}
}
