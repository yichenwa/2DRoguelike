//Work on rotation of the bullet and dealing damage with the bullets
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {
	private Animator animator;
	// Use this for initialization
	public GameObject Bullet; 
	//public float shotDelay = 5f; 
	//private bool shotFired = false;
		
	private PlayerScript2 player;
	void Start () 
	{
		animator = GetComponent<Animator> ();
		player = FindObjectOfType<PlayerScript2> ();
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Y) && player.ammo > 0)
		{   
			animator.SetTrigger ("attack"); //We will need left, right, up, down attacking animations
			Instantiate(Bullet, transform.position, new Quaternion(0, 0, 0, 0));
			//Bullet.GetComponent<Bulletmove>().Team = GetComponent<unit>().Team;
			player.ammo -= 1;
			//shotFired = true;

		}
	}

}
