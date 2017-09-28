using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove : MonoBehaviour {

	// Use this for initialization
	public float BulletSpeed ;
	public int Team = 0;
	void Start () {

	}

	void Update() {
		transform.Translate(Vector2.right * BulletSpeed * Time.deltaTime);
		if (transform.position.y > 20 || transform.position.y < -20)
		{

			Destroy(gameObject);

		}
	}
}
