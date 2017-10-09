//author Yichen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextScene : MonoBehaviour {
	private Rigidbody2D myRigidbody;
	private bool NextSence;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		NextSence = false;

		if (Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel ("2");
		}
	}

}
