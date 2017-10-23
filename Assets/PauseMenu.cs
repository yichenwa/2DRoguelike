using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenCanvas;
    public bool gamePaused;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {            //check is button is pressed 
                                //if pressed activate the canvas
        if (gamePaused)
        {
            pauseMenCanvas.SetActive(true);

        }
        else
        {
            pauseMenCanvas.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))    //for now spacebar can be used to pause and unpause as well
        {
            gamePaused = !gamePaused;           
        }
	}

    void resume()
    {
        gamePaused = false;
    }
}
