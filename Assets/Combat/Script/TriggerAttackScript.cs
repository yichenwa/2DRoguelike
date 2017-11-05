//Mike Fortin, these triggers are hitboxes around the player for melee attacking
//They are used depending on the direction of the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackScript : MonoBehaviour 
{
	public float strength = 50; //Strength of the melee attack
	private PlayerScript2 player;

	void Start()
	{
		player = FindObjectOfType<PlayerScript2> (); //Finds the player object
	}

	//Deals damage to the player, and main functionlity of gaining ammunition
	void OnTriggerEnter2D(Collider2D thing)
	{
		if (thing.gameObject.tag == "Enemy" && thing.isTrigger == false) //&& thing.gameObject.name == "Enemy") 
		{
			thing.SendMessageUpwards ("takeDamage", strength);
		}
		if (player.ammo < 7) //Every melee hit increases ammunition reserve(up to 7)
		{
			player.ammo += 1;
		}
			
	}
}
