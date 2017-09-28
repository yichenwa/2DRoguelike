using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackScript : MonoBehaviour 
{
	public float strength = 50;


	void OnTriggerEnter2D(Collider2D thing)
	{
		if (thing.isTrigger == false) //&& thing.gameObject.name == "Enemy") 
		{
			thing.SendMessageUpwards ("takeDamage", strength);
		}
			
	}
}
