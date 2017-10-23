using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour
{
    public GameObject gameManager;          //GameManager prefab to instantiate.

    //Loader script is attached to the Main Camera (which in turn is attached to the Player prefab).
    //When the PlayerMove scene begins Awake() is called from the Loader script.
    //The Awake() method instantiates an instance of the gameManager script.
    void Awake()
    {
        Instantiate(gameManager);
    }
}