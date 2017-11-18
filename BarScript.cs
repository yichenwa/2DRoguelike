﻿//Clinton Oka, Last Edited 10/22
//Shows Health Bar, Parry Status Bar, and Time Trail
//see PlayerScript2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {
    private PlayerScript2 player;

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image health;
    [SerializeField]
    private Image parry;
    [SerializeField]
    public int timeleft = 300;
    [SerializeField]
    public Text timeCountDown;
    
    //private Image test;

    public float MaxValue { get; set; }


    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, 100, 0, 1);
        }
    }

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerScript2>();
        StartCoroutine("LoseTime"); //Once the player is initialized the time will begin. 

    }
	
	// Update is called once per frame
	void Update () { //THe series of else ifs represents states of the health bar in relation to the player health
        timeCountDown.text = ("" + timeleft);

        if (timeleft <= 0)
        {
            StopCoroutine("LoseTime");
            timeCountDown.text = "Times Up!"; //time up then end game.
        }
        if (player.playerHP <= 0){
            health.fillAmount = 0;
        }
        else if (player.playerHP <= player.maxHP * .1F)
        {
            health.fillAmount = .1F;
        }
        else if (player.playerHP <= player.maxHP * .2F)
        {
            health.fillAmount = .2F;
        }
        else if (player.playerHP <= player.maxHP * .3F)
        {
            health.fillAmount = .3F;
        }
        else if (player.playerHP <= player.maxHP * .4F)
        {
            health.fillAmount = .4F;
        }
        else if (player.playerHP <= player.maxHP * .5F)
        {
            health.fillAmount = .5F;
        }
        else if (player.playerHP <= player.maxHP * .6F)
        {
            health.fillAmount = .6F;
        }
        else if (player.playerHP <= player.maxHP * .7F)
        {
            health.fillAmount = .7F;
        }
        else if (player.playerHP <= player.maxHP * .8F)
        {
            health.fillAmount = .8F;
        }
        else if (player.playerHP <= player.maxHP * .9F)
        {
            health.fillAmount = .9F;
        }
        HandleBar();
	}

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeleft--;
        }
    }

    private void HandleBar()//will update the bar constantly
    {

        if (player.canParryAgain == true)
        {
            parry.fillAmount = 1;
        }
        else if (player.canParryAgain == false)
        {
            parry.fillAmount = 0;

        }
        

            //HandleBar();
    }

    private float Map(float val, float inMin, float inMax, float outMin, float outMax)
    {
        return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //
    }
}
