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
    public float timeleft = 301.5f;
    [SerializeField]
    public Text timeCountDown;
    private int leveldelay;
    public GameObject levelLoadCanvas;
    public GameObject barCanvas;


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
        StartCoroutine("LoseTime");
        levelLoadCanvas.SetActive(true);
        barCanvas.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
        //level screen delay
        if(timeleft == 299.5)
        {
            timeleft = 300;
            barCanvas.SetActive(true);
            levelLoadCanvas.SetActive(false);
        }

        timeCountDown.text = ("" + timeleft);
        if(timeleft <= 0)
        {
            StopCoroutine("LoseTime");
            timeCountDown.text = "Times Up!"; //time up then end game
            player.playerdead = true;
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
