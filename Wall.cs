using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour 
{
	public int hp = 0;
	private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void awake () 
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
