using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour {

	private GameObject [] musicPlayer;
	// Use this for initialization
	void Start () 
	{
		musicPlayer  = GameObject.FindGameObjectsWithTag ("Music");
		if (musicPlayer.Length > 1)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
