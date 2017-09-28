using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {
	private Animator animator;
	// Use this for initialization
	public GameObject Bullet; 
	void Start () {
		animator = GetComponent<Animator> ();

	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))
		{   
			animator.SetTrigger ("attack");
			Instantiate(Bullet, transform.position, new Quaternion(0, 0, 0, 0));
			//Bullet.GetComponent<Bulletmove>().Team = GetComponent<unit>().Team;

		}
	}
}
