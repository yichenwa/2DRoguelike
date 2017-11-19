//Clinton Oka
//Player Shop Code, Still in the works.
//Will take in the players currency and allow them to buy items
//Will take in the inventory, locking and unlocking items
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerShop : MonoBehaviour {
    public Inventory inventory;
    public PlayerScript2 player;

    public bool Dag;
    public Text dagText;

    public bool Arrow;
    public Text arrowText;



	// Use this for initialization
	void Start () {
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<PlayerScript2>();
    }
    public void buyArrow()
    {
        if(player.coins-150 >= 0)
        {
            arrowText.text =  "Bought";
            Arrow = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
