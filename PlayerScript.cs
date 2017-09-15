using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MovingObject 
{
	public float restartLevelDelay = 1f;

	private Animator animator;


	// Use this for initialization
	protected override void Start () 
	{
		animator = GetComponent<Animator> ();

		base.Start();
	}

	private void OnDisable()
	{
		
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove <T> (xDir, yDir);
		RaycastHit2D hit;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0) 
		{
			vertical = 0; //THIS MAY CHANGE, THIS IS MOVEMENT MECHANICS, PREVENTS DIAGONAL MOVEMENT!!!
		}

		if (horizontal != 0 || vertical != 0)
		{
			AttemptMove<Wall>(horizontal, vertical); //THIS MAY CAUSE PROBLEMS MISSING <>
		}
	}

	private void Restart()
	{
		Application.LoadLevel(Application.loadedLevel); //MAY DIFFER DEPENDING ON IMPLEMENTATION OF LEVELS
	}

	protected override void OnCantMove <T> (T component)
	{
		//This will be revisitied with progress
	}
	private void CheckIfDead()
	{
		
	}
}
