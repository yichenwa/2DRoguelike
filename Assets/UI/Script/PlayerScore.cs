//Clinton Oka
//See BarScript, EnemyScript & PlayerScript2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScore : MonoBehaviour {
    private PlayerScript2 player;
    private BarScript playerbar;
    private EnemyScript enemy;

    public Text timeMult;
    public Text enemMult;
    public Text lvlMult;
    public Text total;
    public Text current;
    public int timeleft;
    public int enemies;
    public double totalsc;

    public GameObject playerScoreCanvas;

    public bool calcScore;


    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerScript2>();
        playerbar = FindObjectOfType<BarScript>();
        enemy = FindObjectOfType<EnemyScript>();
    }
	
	// Update is called once per frame
	void Update () {
        enemtot(); //tally up enemies slain, *See Enemy Script*
        //create an if game is finished funct
        //then grab the time
        if (player.playerdead)
        {
                deadScore();
                print("player is dead");
                playerScoreCanvas.SetActive(true);
        }
        else //round won statement placed here
        {
            Time.timeScale = 1; //resume the game
            playerScoreCanvas.SetActive(false);
        }
        //totScore();


    }

    public void enemtot()
    {
        enemMult.text = "" + enemies;
    }

    public void totScore()
    {
        lvlMult.text = "100";
        timeleft = playerbar.timeleft;
        timeMult.text = "" + timeleft;
        totalsc = (enemies * 20) + 100 + (timeleft*1.5); // 100 is for completeing lvl 1 make a value that is global
        total.text = "Survived +" + totalsc;
    }
    public void deadScore()
    {
        lvlMult.text = "0";
        timeleft = playerbar.timeleft;
        timeMult.text = "" + timeleft;
        totalsc = (enemies * 20) + (timeleft * .10);
        total.text = "You Died +" + totalsc;
    }

   


}
