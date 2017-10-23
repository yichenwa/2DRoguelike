//author: yichen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatSkull : MonoBehaviour {
	float times = 1f;
	public GameObject skull;
	GameObject targert = null; 
	private Rigidbody2D wormRigidbody;
	void Start () {
		wormRigidbody = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		int count = 0;
		Movement ();
		times -= Time.deltaTime; 

		 if (times < 0)  
		{
			GameObject obj = (GameObject)Instantiate(skull);

			int rx = Random.Range(-12, 12);
			int ry = Random.Range(-10, 10);
			obj.transform.position = new Vector3(rx,ry,0);

			times = Random.Range(0, 5);

			count++;
		}

		if (count >= 6) {
			
		}
	}

	private void Movement(){
		wormRigidbody.velocity = Vector2.left;
	}
}