using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour {

    public GameObject pauseMenuCanvas;


    public bool isPaused;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isPaused)
        {
            //if (Time.timeScale == 1) //means if not paused
            //{
            Time.timeScale = 0;
            pauseMenuCanvas.SetActive(true);
        }
        //}
        else
        {
            Time.timeScale = 1; //resume the game
            pauseMenuCanvas.SetActive(false);
        }
            //time.timescale is speed of the game
            if(Input.GetKeyDown(KeyCode.P))
            {
                isPaused = !isPaused;
            }
        }
    public void resumeGame()
    {
        isPaused = false;
    }
    public void restartGame()
    {
        Application.LoadLevel(Application.loadedLevel);

    }
    public void ReturnToMain(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
