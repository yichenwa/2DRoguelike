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

	public Collider2D left;
	public Collider2D right;
	public Collider2D up;
	public Collider2D down;

	void Start()
	{
		left.enabled = false;
		right.enabled = false;
		up.enabled = false;
		down.enabled = false;
		player = FindObjectOfType<PlayerScript2> ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.T) && !attacking && player.getParrying() == false) //Restricts attack while parrying
		{
			attacking = true;
			attackTimer = attackDelay;
			if (player.getDirectionString () == "u") 
			{
				up.enabled = true;
			} 
			else if (player.getDirectionString () == "d") 
			{
				down.enabled = true;
			} 
			else if (player.getDirectionString () == "l") 
			{
				left.enabled = true;
			} 
			else if (player.getDirectionString () == "r") 
			{
				right.enabled = true;
			}
		}
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
