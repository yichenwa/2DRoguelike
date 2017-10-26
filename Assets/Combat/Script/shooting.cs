//Mike Fortin, Used to set shooting mechanics
//Player can only shoot left or right
//Bullet needs to rotate
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {
	private Animator animator;
	// Use this for initialization
	public GameObject Bullet; //Finds the bullet prefab
	private Vector3 leftPositionFix; //Fixes issue when shooting to the left
	private float leftOffset = 7.5f; //Offset needed to handle this issue
	//public float shotDelay = 5f; 
	//private bool shotFired = false;
	private PlayerScript2 player;
	private SpriteRenderer bulletOrientation;

	void Start () 
	{
        //animator = FindObjectOfType<Animator> ();
        animator = GetComponent<Animator>(); //Finds the animator component
		player = FindObjectOfType<PlayerScript2> (); //Finds the players location 
		bulletOrientation = Bullet.GetComponent<SpriteRenderer>();

	}

	//Handles the instantiation of bullets based on the players direction, ammo, and time limitations
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Y) && player.ammo > 0)
		{   
			//animator.SetTrigger ("attack"); //We will need left, right, up, down attacking animations
			if (player.getDirectionString () == "l") {
				leftPositionFix.Set (transform.position.x - leftOffset, transform.position.y, -1f); //Used to fix shooting left issues
				bulletOrientation.flipX = true;
				Instantiate (Bullet, leftPositionFix,  new Quaternion(0, 0, 0, 0));
				animator.SetTrigger ("shootleft");
			} 
			else if (player.getDirectionString() == "r")
			{
				bulletOrientation.flipX = false;
				Instantiate (Bullet, transform.position, new Quaternion (0, 0, 0, 0));
				animator.SetTrigger ("shootright");
			}
			player.ammo -= 1;
			//shotFired = true;

		}
	}

}
