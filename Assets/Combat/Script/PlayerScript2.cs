//Mike Fortin, PlayerScript
//Also see PlayerMeleeAttack,
//Shooting, and BulletMove scripts
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(Animator))]
public class PlayerScript2 : MonoBehaviour
{

    public float playerSpeed = 3f; //Used to set the players speed when moving
    public float playerHP = 100; //Used to store adjust and update the players health
    public float maxHP = 100; //Used for the health bar to calc currentHealth/maxHealth
    public float parryTime = 2f; //Sets how long a parry will last for
    private string directionString = ""; //Used for melee attack direction
    public float ammo = 2; //Ammo reserve
    private Rigidbody2D playerBody; //Used to find and adjust the body of the player
    private Animator animator; //Used to set animation triggers off depending on movement
    public bool canTakeDamage = true; //Used with parry
    public bool canParryAgain = true; //Used with parry, prevents spamming
    private Vector2 currentPosition; //Used to adjust position with world switching
    private bool inFantasyWorld = true; //Determines which world the player is in 
	private bool canTeleport = true; //Used to limit teleporting between worlds

    // Used for initialization
    void Start()
    {
        playerBody = FindObjectOfType<Rigidbody2D>(); //Initializes the player body
        playerBody.freezeRotation = true; //IMPORTANT! PLAYER TYPE MUST BE DYNAMIC WITH FREEZE ROTATION SELECTED
        //animator = FindObjectOfType<Animator>();
        animator = GetComponent<Animator>(); //Finds the animator controller
        canTakeDamage = true; 
        directionString = "d";
        currentPosition.Set(transform.position.x, transform.position.y);
    }

    // This is how the player's movement and status will update 
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && canParryAgain == true) //Handles parrying, binds to R key for now
        {
            animator.SetTrigger("parry");
            canTakeDamage = false;
            canParryAgain = false;
            StartCoroutine(parryDelay());
            StartCoroutine(preventUnlimitedParry());
        }

		if (Input.GetKeyDown(KeyCode.F) && canTeleport == true) //Handles teleportation, binds to F key for now
        {
			canTeleport = false;

            if(inFantasyWorld == true)
            {
                transform.position = new Vector3(transform.position.x + 2000, transform.position.y, transform.position.z);
                inFantasyWorld = false;
            }
            else
            {
                transform.position = new Vector3(transform.position.x - 2000, transform.position.y, transform.position.z);
                inFantasyWorld = true;
            }

			StartCoroutine (teleportDelay ());
        }

        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        GetComponent<Rigidbody2D>().velocity = targetVelocity * playerSpeed; //moves the player

        //This series of ifs uses animation triggers according to directional movement
        //Priority animation given to left and right on diagonal movement
        if (targetVelocity.x < 0)
        {
            animator.SetTrigger("left");
            directionString = "l";
        }
        else if (targetVelocity.x > 0)
        {
            animator.SetTrigger("right");
            directionString = "r";
        }
        else if (targetVelocity.y > 0)
        {
            animator.SetTrigger("back");
            directionString = "u";
        }
        else if (targetVelocity.y < 0)
        {
            animator.SetTrigger("forward");
            directionString = "d";
        }

       
        //Kills the player if his health drops below 0
        if (playerHP == 0) {
            animator.SetTrigger("die");
        }

        if (playerHP <0)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("MenuScreen");
        }
        currentPosition.Set(transform.position.x, transform.position.y);
    }
    IEnumerator parryDelay()
    {
        yield return new WaitForSeconds(parryTime); //limits the parrying to a short time
        canTakeDamage = true;
    }

    IEnumerator preventUnlimitedParry()
    {
        yield return new WaitForSeconds(3f); //Can be changed,
        canParryAgain = true;                  //prevents spamming of the parry button for invincibility
    }

	IEnumerator teleportDelay() //Limits how often the player can teleport with the f key
	{
		yield return new WaitForSeconds (3f);
		canTeleport = true;
	}

    public string getDirectionString() //Used for attacking. Determines direction to attack in 
    {
        return directionString;
    }

    public bool getParrying() //Used for UI, attacking to see if the player is parrying or not
    {
        return !canParryAgain;
    }
    public Vector2 getCurrentPosition() //Returns the players current position on the map
    {
        return currentPosition;
    }
}