// Mike Fortin, Basic Enemy Script
//All code in this file written by Mike Fortin thru 9/21
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyScript1 : MonoBehaviour
{

    public float enemySpeed = .75f; //Used to set the enemies walking speed
    private Vector2 playerPosition; //Will be used to locate the player's location
    private PlayerScript2 player; //Finds the player object by finding the PlayerScript2 object
    private Rigidbody2D enemyBody; //Used to control the enemy body 
    private bool canWalk = true; //Will be used to make the enemy pause if a condition is met 

    // Used for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerScript2>();
        enemyBody = FindObjectOfType<Rigidbody2D>();
        enemyBody.freezeRotation = true;
    }

    //Main function for the enemy to update
    void FixedUpdate()
    {
        enemyBody.freezeRotation = true;

        if (canWalk == false)
        {
            StartCoroutine(walkDelay()); //After he's been stopped, call the function to restart his walking
        }
        if (canWalk == true)  //Move towards the player creepily
        {
            playerPosition = (player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, enemySpeed * Time.deltaTime);
        }

    }

    void OnCollisionEnter2D(Collision2D thing) //If the enemy collides with the player, inflict damage to him
    {
        if (thing.gameObject.name == "Player")
        {
            player.playerHP -= 50; //Damage done, can be adjusted
        }

        canWalk = false;
    }

    IEnumerator walkDelay() //Used to get the enemy walking after a collision
    {
        yield return new WaitForSeconds(.5f);
        canWalk = true;
    }
}