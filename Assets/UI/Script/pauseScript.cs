using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour {

    public GameObject pauseMenuCanvas;
    public GameObject inventoryCanvas;


    public bool isPaused;
    public bool isInventory;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenuCanvas.SetActive(true);
            if(isInventory)
            {
                inventoryCanvas.SetActive(true);
            }
            else
            {
                inventoryCanvas.SetActive(false);
            }

        }
        //}
        else
        {
            Time.timeScale = 1; //resume the game
            pauseMenuCanvas.SetActive(false);
            inventoryCanvas.SetActive(false);
        }
            //time.timescale is speed of the game
            if(Input.GetKeyDown(KeyCode.P)|| (Input.GetKeyDown(KeyCode.Escape)))
            {
                isPaused = !isPaused;
                if(isPaused == false && isInventory == true)
            {
                isInventory = false;
            }
            }
        }
    public void resumeGame()
    {
        isPaused = false;
    }
    public void Inventory()
    {
        isInventory = true;
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
