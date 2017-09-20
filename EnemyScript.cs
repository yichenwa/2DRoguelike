// Mike Fortin, Basic Enemy Script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class EnemyScript : MonoBehaviour {

	public float enemySpeed = .75f;
	private Vector2 playerPosition;
	private PlayerScript2 player;
	private Rigidbody2D enemyBody;
	private bool canWalk = true;
	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerScript2> ();
		enemyBody = FindObjectOfType<Rigidbody2D> ();
		enemyBody.freezeRotation = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (canWalk == false) 
		{
			StartCoroutine (walkDelay());
		}
		if (canWalk == true) 
		{
			playerPosition = (player.transform.position);
			transform.position = Vector2.MoveTowards (transform.position, playerPosition, enemySpeed * Time.deltaTime); 
		}

	}
		
	void OnCollisionEnter2D(Collision2D thing)
	{
		if (thing.gameObject.name == "Player") 
		{
			player.playerHP -= 50;
		}

		canWalk = false;
	}

	IEnumerator walkDelay()
	{
		yield return new WaitForSeconds (.5f);
		canWalk = true;
	}
}
