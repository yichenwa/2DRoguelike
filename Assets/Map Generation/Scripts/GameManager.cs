﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelstartDelay = 3f;
    public static GameManager instance = null;
    public BoardManager boardScript;

    //level and player information handling
    private int lvl = 1;
    public int playerCoins;
    // Use this for initialization
    void Awake()
    {
        //This if/else-if statement ensures that only one instance of the gameManager exists at a time.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();

        //InitGame();     //Calls InitGame() in boardManager.

    }

    void InitGame()
    {
        //Calls the SetupScene method found in the BoardManager script.
        //This is where the entirety of level generation takes place.
        boardScript.SetupScene(LevelData.currentLevel);
    }

    void OnLevelWasLoaded(int index)
    {
        //Call InitGame to initialize our level.
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
    }
}