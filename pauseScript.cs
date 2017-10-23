//Clinton Oka Last Updated 10/22
//Pause Menu Script, Resume, Pause, Restart, and Inventory
//Can be invoked pressing P, Inventory is a Child of the Pause Menu
//Cannot Call inventory without pausing the game first.
//See "Inventory.cs, GameManager, Boardmanager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Completed;

public class pauseScript : MonoBehaviour {
    public GameManager manage;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public GameObject gameManager;


    public GameObject pauseMenuCanvas;
    public GameObject inventoryCanvas;


    public bool isPaused;
    public bool isInventory;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) //boolean is the game paused
        {
                Time.timeScale = 0;
                pauseMenuCanvas.SetActive(true);
                if (isInventory)
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
            if (Input.GetKeyDown(KeyCode.P))
            {
            isPaused = !isPaused;
            if (isPaused == false && isInventory == true)
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
            Instantiate(gameManager);
    }
    public void ReturnToMain(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
