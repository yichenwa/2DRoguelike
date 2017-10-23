using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackScript : MonoBehaviour 
{
	public float strength = 50;
	private PlayerScript2 player;

	void Start()
	{
		player = FindObjectOfType<PlayerScript2> ();
	}


	void OnTriggerEnter2D(Collider2D thing)
	{
		if (thing.isTrigger == false) //&& thing.gameObject.name == "Enemy") 
		{
			thing.SendMessageUpwards ("takeDamage", strength);
		}
		if (player.ammo < 7) 
		{
			player.ammo += 1;
		}
			
	}
}
