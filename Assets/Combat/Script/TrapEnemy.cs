//Mike Fortin, EnemyScript
//Handles the basic enemy mechanics
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
public class TrapEnemy : MonoBehaviour
{

    public float enemySpeed = 65f; //Used to set the enemies walking speed
    public float enemyHP = 100;
	public float strength = 50;
    private Vector2 playerPosition; //Will be used to locate the player's location
    private PlayerScript2 player; //Finds the player object by finding the PlayerScript2 object
    private Rigidbody2D enemyBody; //Used to control the enemy body 
    private bool canWalk = true; //Will be used to make the enemy pause if a condition is met
    private bool canDamage = true;
	private bool playerInRange;
	private CircleCollider2D enemyRange;
    private Animator animator;
	private Vector3 savedPosition;

    //Modify the below parameters to change the behavior of the trap enemy
    private float triggerDistance = 65;   // How close the player must be to be triggered
    private float lungeDistance = 65; // How far to go when chasing the player

    // Used for trap mechanics
    private Vector2 spawnSpot, goalSpot; //Where the enemy spawned and where it will try to go to when triggered
    private bool triggered = false; // Set to true when the 'trap' is triggered

    // Used for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerScript2>();
        enemyBody = FindObjectOfType<Rigidbody2D>();
        animator = FindObjectOfType<Animator>();
        enemyBody.freezeRotation = true; //IMPORTANT! ENEMY MUST BE KINEMATIC
		playerInRange = false;
        spawnSpot = transform.position;
        goalSpot = spawnSpot;
    }

    //Main function for the enemy to update
    void FixedUpdate()
    {
		
		savedPosition = transform.position;
        playerPosition = player.transform.position;

        if (canWalk == false)
        {
            StartCoroutine(walkDelay()); //After he's been stopped, call the function to restart his walking
        }
        if (canWalk == true)
        {
            // Always "move" towards the goal spot. By default this location is the spawn position (so no movement)
            transform.position = Vector2.MoveTowards(transform.position, goalSpot, enemySpeed * Time.deltaTime);

            // Player has stepped on our line and is within the trigger distance, we are not triggered and we are in the spawn position
            if ((Mathf.Abs(transform.position.x - playerPosition.x) < 5 || Mathf.Abs(transform.position.y - playerPosition.y) < 5)
                && !triggered && 
                transform.position.x == spawnSpot.x && transform.position.y == spawnSpot.y && 
                Vector3.Distance(transform.position,playerPosition)<triggerDistance)
            {
                triggered = true;
                goalSpot = spawnSpot;
                if (Mathf.Abs(transform.position.x - playerPosition.x) < 5)
                    goalSpot.y -= lungeDistance * ((transform.position.y - playerPosition.y) / Mathf.Abs(transform.position.y - playerPosition.y));
                if (Mathf.Abs(transform.position.y - playerPosition.y) < 5)
                    goalSpot.x -= lungeDistance * ((transform.position.x - playerPosition.x) / Mathf.Abs(transform.position.x - playerPosition.x));
            }
        }
		if (triggered == true) //If we are triggered and at the goal spot, reset and go back to spawn spot
        {
            if (transform.position.x==goalSpot.x && transform.position.y==goalSpot.y)
            {
                triggered = false;
                goalSpot = spawnSpot;
            }
        }
        if (enemyHP <= 0) //Kills the enemy if his health drops to 0 or les
        {
            this.gameObject.SetActive(false);
        }

    }

    void OnCollisionStay2D(Collision2D thing) //If the enemy collides with the player, inflict damage to him
    {
        if (thing.gameObject.name == "Player" && player.canTakeDamage == true && canDamage == true)
        { //The new canTakeDamage works // with parrying
            player.playerHP -= strength; //Damage done, can be adjusted
            canDamage = false;
            StartCoroutine(damageDelay());
			transform.position = savedPosition;
        }
			canWalk = false;
			triggered = false;
			goalSpot = spawnSpot;
    }

    public void takeDamage(float damage) //Used as a reference in player scripts, to hurt the enemy
    {
        enemyHP -= damage;
    }

    IEnumerator walkDelay() //Used to get the enemy walking after a collision, with delay of course
    {
        yield return new WaitForSeconds(.5f);
        canWalk = true;
    }

    IEnumerator damageDelay() //Used to limit how quick the enemy can damage the player if standing adjacent to him
    {
        yield return new WaitForSeconds(.5f);
        canDamage = true;
    }
}