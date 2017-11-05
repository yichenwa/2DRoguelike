//Mike Fortin, this file handles the attackTriggers
//used for melee attacking
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerScript2))]
public class PlayerMeleeAttack : MonoBehaviour 
{
	private bool attacking = false;
	private float attackTimer = 0;
	private float attackDelay = .3f;
	private PlayerScript2 player;
	private Animator animator;

	//Finds the attached hitboxes in all directions
	public Collider2D left;
	public Collider2D right;
	public Collider2D up;
	public Collider2D down;

	//Initializes the boxes to off
	void Start()
	{
		left.enabled = false;
		right.enabled = false;
		up.enabled = false;
		down.enabled = false;
		player = FindObjectOfType<PlayerScript2> ();
        //animator = FindObjectOfType<Animator> ();
        animator = GetComponent<Animator>();
	}

	//This is main function that enables/disables a melee attack depending on direction and keypress
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.T) && !attacking /*&& player.getParrying() == false*/) //Restricts attack while parrying
		{
			attacking = true;
			attackTimer = attackDelay;
			if (player.getDirectionString () == "u") {
				up.enabled = true;
				animator.SetTrigger ("sliceback");
			} else if (player.getDirectionString () == "d") {
				down.enabled = true;
				animator.SetTrigger ("sliceforward");
			} else if (player.getDirectionString () == "l") {
				left.enabled = true;
				animator.SetTrigger ("sliceleft");
			} else if (player.getDirectionString () == "r") {
				right.enabled = true;
				animator.SetTrigger ("sliceright");
			}
		}
		//Limits total attack spamming
		if (attacking) 
		{
			if (attackTimer > 0) 
			{
				attackTimer -= Time.deltaTime;
			} 

			else 
			{
				attacking = false;
				left.enabled = false;
				right.enabled = false;
				up.enabled = false;
				down.enabled = false;
			}
		}
	}
}
