//Mike Fortin, Used For Bullets Movement Mechanics
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove : MonoBehaviour
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
        //bulletBox = FindObjectOfType<BoxCollider2D> ();
        player = FindObjectOfType<PlayerScript2>(); //Finds the players location

        //bulletMoving = false;

		//These next statement determine which direction to shoot the bullet in
        if (player.getDirectionString() == "r")
        {
            shootRight();

        }
        if (player.getDirectionString() == "l")
        {
            shootLeft();
            //transform.rotation()
        }
        if (player.getDirectionString() == "u")
        {
            shootUp();

        }
        if (player.getDirectionString() == "d")
        {
            shootDown();

        }

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

	//This function makes bullets do damages to enemies and destroys them on collision 
    void OnCollisionEnter2D(Collision2D thing) 
    {
        if (thing.gameObject.tag == "Enemy") //Will need changing if there are more boxcolliders introduced
        {
            thing.gameObject.SendMessageUpwards("takeDamage", strength);
            Destroy(gameObject);
        }
        if (thing.gameObject.name != "Player")
        {
            Destroy(gameObject);
        }

    }
}