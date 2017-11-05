//Clinton Oka, Last Updated 10/22
//Freeze Script Can be invoked on any canvas to freeze frames/ pause.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeze : MonoBehaviour {

    //public bool playerdead;

    public GameObject playerScoreCanvas;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(playerScoreCanvas == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
	}
}
