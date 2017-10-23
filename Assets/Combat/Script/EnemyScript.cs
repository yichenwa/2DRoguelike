//Mike Fortin, EnemyScript
//Handles the basic enemy mechanics
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyScript : MonoBehaviour
{

    public float enemySpeed = .75f; //Used to set the enemies walking speed
    public float enemyHP = 150;
    private Vector2 playerPosition; //Will be used to locate the player's location
    private PlayerScript2 player; //Finds the player object by finding the PlayerScript2 object
    private Rigidbody2D enemyBody; //Used to control the enemy body 
    private bool canWalk = true; //Will be used to make the enemy pause if a condition is met
    private bool canDamage = true;
	private bool playerInRange;
	private CircleCollider2D enemyRange;
    private Animator animator;
	private Vector3 savedPosition;

    // Used for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerScript2>();
        enemyBody = FindObjectOfType<Rigidbody2D>();
        animator = FindObjectOfType<Animator>();
        enemyBody.freezeRotation = true; //IMPORTANT! ENEMY MUST BE KINEMATIC
		playerInRange = false;
		//enemyRange.enabled = true;

    }

    //Main function for the enemy to update
    void FixedUpdate()
    {
		savedPosition = transform.position;

        if (canWalk == false)
        {
            StartCoroutine(walkDelay()); //After he's been stopped, call the function to restart his walking
        }
		if (canWalk == true && playerInRange == true)  //Move towards the player creepily
        {
            playerPosition = (player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, enemySpeed * Time.deltaTime);
            /*if (transform.position.x < 0) 
			{
				animator.SetTrigger ("enemy1 left");
			}*/
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
            player.playerHP -= 50; //Damage done, can be adjusted
            canDamage = false;
            StartCoroutine(damageDelay());
			transform.position = savedPosition;
        }
        canWalk = false;
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

	void OnTriggerEnter2D(Collider2D thing) //Used to see if the player is close, to initialize movement
	{
		if (thing.gameObject.tag == "Player") 
		{
			playerInRange = true;
		}
	}
	void OnTriggerExit2D(Collider2D thing) //Used to stop the enemy from moving if th player is not in attack range
	{
		if (thing.gameObject.tag == "Player") 
		{
			playerInRange = false;
		}
	}
}