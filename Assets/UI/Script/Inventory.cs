//Clinton Oka
//Inventory Script, Will have all weapon types developed.
//For now can only equip one of each type. 1Melee, 1 Ranged, 1 Clothing
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public PlayerScript2 player;
    public playerShop shop;
    public shooting shooting;
    public float strength = 50;
    public float rstrength = 75;
    public float mstrength = 50;

    private bool mFull;     
    private bool rFull;
    private int melee;          //keep track of melees equipped
    private int ranged;         //keep track of ranged equipped
    public Text oneMelee;       
    public bool equipDag;
    public Text Dag;
    public bool equipSlingShot;
    public Text slingshot;
    public bool equipSw;
    public Text sword;
    public bool equipArrow;
    public Text arrow;

    public bool equipb1;
    public Text boot1;

    public bool equipb2;
    public Text boot2;

    public bool rangedb;




    // Use this for initialization
    void Start () {

        player = FindObjectOfType<PlayerScript2>();
        shop = FindObjectOfType<playerShop>();
       // GameObject player = GameObject.Find("Player")
       // shooting shootsc = FindObjectOfType<shooting>();
        equipDag = true ;

    }

    // each void method equips respective members of the melee, ranged, and clothing feilds. 
    //For now equiping ones de-equipps the others in the same feild. We wish to have the ability to
    //equip more than one in a future time, and incorporate playyer leveling up system
    public void equipBoot1()
    {
        equipb1 = !equipb1;
        player.playerSpeed = 60;
    }
    public void equipDagger()
    {
            equipDag = !equipDag;
            mstrength = 50;
            equipSw = false;
    }
    public void equipslingShot()
    {
            equipSlingShot = !equipSlingShot;
            rstrength = 75;
            equipArrow = false;
    }
    public void equipSword()
    {
        
           // equipSw = !equipSw; //till shop implemented keep other items locked

            mstrength = 125;
            equipDag = false;
    }
    public void equipinArrow()
    {
           // equipArrow = !equipArrow;
            rstrength = 175;
            equipSlingShot = false;
            //arrow.text = "Locked";
    }

    // Update is called once per frame
    void Update () {
        if ( (Input.GetKeyDown(KeyCode.Y)))
        {
            print("Shooting");
            strength = rstrength;

        }
        if((Input.GetKeyDown(KeyCode.T)))
        {
            strength = mstrength;
        }

        if (equipb1 == true)
        {
            boot1.text = "Equipped";
        }
        else
        {
            boot1.text = "Equip?";
        }

        if (equipDag == true)
        {
            Dag.text = "Equipped";
        }
        else
        {
            Dag.text = "Equip?";
        }
        if (equipSw == true)
        {
            sword.text = "Equipped";
        }
        else
        {
           // sword.text = "Equip?";
        }
        if (equipSlingShot == true)
        {         
            slingshot.text = "Equipped";
        }
        else
        {
            slingshot.text = "Equip?";
        }
        if (equipArrow == true)
        {
            arrow.text = "Equipped";
        }
        else
        {
           // arrow.text = "Equip?";
        }
      

    }
}
