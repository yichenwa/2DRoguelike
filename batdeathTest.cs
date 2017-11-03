// author by Jerry 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batdeathTest : MonoBehaviour {
	public int hp =3;
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetTrigger ("hurt");
		hp -= 1;

		if (hp <= 0) {
			Destroy (this.gameObject);//If the bat's hp=0, it will disappeared
		}
	}

}
