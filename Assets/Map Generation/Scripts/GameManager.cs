using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public BoardManager boardScript;

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

        InitGame();     //Calls InitGame() in boardManager.
    }

    void InitGame()
    {
        //Calls the SetupScene method found in the BoardManager script.
        //This is where the entirety of level generation takes place.
        boardScript.SetupScene();
    }

    // Update is called once per frame
    void Update()
    {

    }
}