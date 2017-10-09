using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemies : MonoBehaviour {

    public Vector2 spawnLocation;
    public GameObject[] enemyTypesPrefabs;
    public GameObject[] enemyTypesClones;


    void spawnNewRoom(Vector2 pos, GameObject[] enemyTypes, int roomID)
    {
        switch (roomID)
        {
            case 0: break;

            case 1:
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (5 * 16), pos.UnitY + (3 * 16)), Quaternion.identity);
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (9 * 16), pos.UnitY + (3 * 16)), Quaternion.identity);
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (13 * 16), pos.UnitY + (3 * 16)), Quaternion.identity);
                break;

            case 2:
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (7 * 16), pos.UnitY + (3 * 16)), Quaternion.identity);
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (10 * 16), pos.UnitY + (3 * 16)), Quaternion.identity);
                break;

            case 3:
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (8 * 16), pos.UnitY + (6 * 16)), Quaternion.identity);
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (8 * 16), pos.UnitY + (10 * 16)), Quaternion.identity);
                break;

            case 4:
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (8 * 16), pos.UnitY + (10 * 16)), Quaternion.identity);
                Instantiate(enemyTypes[0], new Vector2(pos.UnitX + (8 * 16), pos.UnitY + (10 * 16)), Quaternion.identity);
                break;

            case 5: break;

            case 6: break;

            case 7: break;

            case 8:
                Instantiate(enemyTypes[1], new Vector2(pos.UnitX + (8 * 16), pos.UnitY + (10 * 16)), Quaternion.identity);
                break;

            case 9: break;




        }
        enemyTypesClones[0] = Instantiate()
     

    }
}
