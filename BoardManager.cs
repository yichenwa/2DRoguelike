//Please Note: this code was originally a script found in an official Unity 2D Roguelike tutorial. Chris Elman has heavily edited the script to adapt it to fit our project's requirements.
//The original script can be at this address: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-board-manager?playlist=17150

using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

namespace Completed

{

    public class BoardManager : MonoBehaviour
    {
        // Using Serializable allows us to embed a class with sub properties in the inspector.
        [Serializable]
        public class RoomPair
        {
            public GameObject _fantasy;             //Fantasy room prefab
            public GameObject _science;             //Science room prefab

            //Assignment constructor.
            public RoomPair(GameObject fantasy, GameObject science)
            {
                _fantasy = fantasy;
                _science = science;
            }
        }

        public GameObject[] fantasyStartRooms;                                 //Array of starting room prefabs
        public GameObject[] fantasyEndRooms;                                   //Array of end room prefabs
        public GameObject[] fantasyNorthRooms;                                 //Array of room prefabs with exits to the north
        public GameObject[] fantasySouthRooms;                                 //Array of room prefabs with exits to the south
        public GameObject[] fantasyEastRooms;                                  //Array of room prefabs with exits to the east
        public GameObject[] fantasyWestRooms;                                  //Array of room prefabs with exits to the west
        public GameObject[] fantasyEnemies;                                    //Array of enemy prefabs
        public GameObject[] scienceStartRooms;                                 //Array of starting room prefabs
        public GameObject[] scienceEndRooms;                                   //Array of end room prefabs
        public GameObject[] scienceNorthRooms;                                 //Array of room prefabs with exits to the north
        public GameObject[] scienceSouthRooms;                                 //Array of room prefabs with exits to the south
        public GameObject[] scienceEastRooms;                                  //Array of room prefabs with exits to the east
        public GameObject[] scienceWestRooms;                                  //Array of room prefabs with exits to the west

        private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.


        private int[,] occupiedSpaces = new int[7, 7];                   //A matrix that takes an xy coordinate and returns the room ID contained there
        private GameObject[,] fantasyRooms = new GameObject[7, 7];     //A matrix that holds the fantasy rooms that have been instantiated
        private GameObject[,] scienceRooms = new GameObject[7, 7];     //A matrix that holds the science rooms that have been instantiated
        private List<Vector2> roomCoordinates = new List<Vector2>();    //A list of xy coordinates that have rooms placed in them
        private int roomCoordinatesSize = 0;
        private List<int> hasExit = new List<int>();                   //A list of room IDs that stil have available exits
        private int hasExitSize = 0;


        //Clears data structures in preparation for generating level
        void InitialiseDataStructures()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    occupiedSpaces[x, y] = -1;
                    fantasyRooms[x, y] = null;
                    scienceRooms[x, y] = null;
                }
            }

            roomCoordinates = new List<Vector2>();
            roomCoordinatesSize = 0;
            hasExit = new List<int>();
            hasExitSize = 0;
        }

        //This method places starting room, places a certain number of regular rooms in a loop, then places an exit room.
        void GenerateMap(GameObject[] fantasyStartRooms, GameObject[] fantasyEndRooms, GameObject[] fantasyNorthRooms, GameObject[] fantasySouthRooms, GameObject[] fantasyEastRooms, GameObject[] fantasyWestRooms, GameObject[] scienceStartRooms, GameObject[] scienceEndRooms, GameObject[] scienceNorthRooms, GameObject[] scienceSouthRooms, GameObject[] scienceEastRooms, GameObject[] scienceWestRooms)
        {
            Vector2 startPos = new Vector2(3, 3);
            Vector2 scaledStartPos = new Vector2(3 * 256, 3 * 176);
            Vector2 offsetStartPos = new Vector2((3 * 256) + 2000, 3 * 176);

            GameObject roomToInstantiate = Instantiate(fantasyStartRooms[0], scaledStartPos, Quaternion.identity);
            fantasyRooms[3, 3] = roomToInstantiate;
            roomToInstantiate = Instantiate(scienceStartRooms[0], offsetStartPos, Quaternion.identity);
            scienceRooms[3, 3] = roomToInstantiate;

            occupiedSpaces[3, 3] = 1;
            roomCoordinates.Add(startPos);
            roomCoordinatesSize += 1;
            hasExit.Add(1);
            hasExitSize += 1;

            for (int i = 0; i < 8; i++)
            {
                Vector3 coordinate = selectCoordinate();
                Vector2 scaledCoordinate = new Vector2((int)coordinate.x * 256, (int)coordinate.y * 176);
                Vector2 offsetCoordinate = new Vector2(((int)coordinate.x * 256) + 2000, (int)coordinate.y * 176);

                RoomPair fanAndSciRoom = selectRoom();

                GameObject fantasyRoomToInstantiate = Instantiate(fanAndSciRoom._fantasy, scaledCoordinate, Quaternion.identity);
                fantasyRooms[(int)coordinate.x, (int)coordinate.y] = fantasyRoomToInstantiate;
                spawnEnemies(scaledCoordinate, fantasyEnemies, fanAndSciRoom._fantasy.tag);
                GameObject scienceRoomToInstantiate = Instantiate(fanAndSciRoom._science, offsetCoordinate, Quaternion.identity);
                scienceRooms[(int)coordinate.x, (int)coordinate.y] = scienceRoomToInstantiate;
                spawnEnemies(offsetCoordinate, fantasyEnemies, fanAndSciRoom._science.tag);

                if ((int)coordinate.z == 1)
                {
                    GameObject fantasyToDestroy = fantasyRooms[(int)coordinate.x, (int)coordinate.y - 1];
                    GameObject scienceToDestroy = scienceRooms[(int)coordinate.x, (int)coordinate.y - 1];
                    Destroy(fantasyToDestroy.transform.Find("northDoorClosed").gameObject);
                    Destroy(fantasyToDestroy.transform.Find("northWall").gameObject);
                    Destroy(scienceToDestroy.transform.Find("northDoorClosed").gameObject);
                    Destroy(scienceToDestroy.transform.Find("northWall").gameObject);

                    Destroy(fantasyRoomToInstantiate.transform.Find("southDoorClosed").gameObject);
                    Destroy(fantasyRoomToInstantiate.transform.Find("southWall").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("southDoorClosed").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("southWall").gameObject);
                }
                else if ((int)coordinate.z == 2)
                {
                    GameObject fantasyToDestroy = fantasyRooms[(int)coordinate.x, (int)coordinate.y + 1];
                    GameObject scienceToDestroy = scienceRooms[(int)coordinate.x, (int)coordinate.y + 1];
                    Destroy(fantasyToDestroy.transform.Find("southDoorClosed").gameObject);
                    Destroy(fantasyToDestroy.transform.Find("southWall").gameObject);
                    Destroy(scienceToDestroy.transform.Find("southDoorClosed").gameObject);
                    Destroy(scienceToDestroy.transform.Find("southWall").gameObject);

                    Destroy(fantasyRoomToInstantiate.transform.Find("northDoorClosed").gameObject);
                    Destroy(fantasyRoomToInstantiate.transform.Find("northWall").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("northDoorClosed").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("northWall").gameObject);
                }
                else if ((int)coordinate.z == 3)
                {
                    GameObject fantasyToDestroy = fantasyRooms[(int)coordinate.x - 1, (int)coordinate.y];
                    GameObject scienceToDestroy = scienceRooms[(int)coordinate.x - 1, (int)coordinate.y];
                    Destroy(fantasyToDestroy.transform.Find("eastDoorClosed").gameObject);
                    Destroy(fantasyToDestroy.transform.Find("eastWall").gameObject);
                    Destroy(scienceToDestroy.transform.Find("eastDoorClosed").gameObject);
                    Destroy(scienceToDestroy.transform.Find("eastWall").gameObject);

                    Destroy(fantasyRoomToInstantiate.transform.Find("westDoorClosed").gameObject);
                    Destroy(fantasyRoomToInstantiate.transform.Find("westWall").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("westDoorClosed").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("westWall").gameObject);
                }
                else if ((int)coordinate.z == 4)
                {
                    GameObject fantasyToDestroy = fantasyRooms[(int)coordinate.x + 1, (int)coordinate.y];
                    GameObject scienceToDestroy = scienceRooms[(int)coordinate.x + 1, (int)coordinate.y];
                    Destroy(fantasyToDestroy.transform.Find("westDoorClosed").gameObject);
                    Destroy(fantasyToDestroy.transform.Find("westWall").gameObject);
                    Destroy(scienceToDestroy.transform.Find("westDoorClosed").gameObject);
                    Destroy(scienceToDestroy.transform.Find("westWall").gameObject);

                    Destroy(fantasyRoomToInstantiate.transform.Find("eastDoorClosed").gameObject);
                    Destroy(fantasyRoomToInstantiate.transform.Find("eastWall").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("eastDoorClosed").gameObject);
                    Destroy(scienceRoomToInstantiate.transform.Find("eastWall").gameObject);
                }

                occupiedSpaces[(int)coordinate.x, (int)coordinate.y] = 2;
                roomCoordinates.Add(coordinate);
                roomCoordinatesSize += 1;
                hasExit.Add(2);
                hasExitSize += 1;
            }

            Boolean endSpaceFarAway = false;
            while (endSpaceFarAway == false)
            {
                Vector3 endPos = selectCoordinate();
                Vector2 scaledEndPos = new Vector2((int)endPos.x * 256, (int)endPos.y * 176);
                Vector2 offsetEndPos = new Vector2(((int)endPos.x * 256) + 2000, (int)endPos.y * 176);
                if (distanceBetweenPoints((int)startPos.x, (int)startPos.y, (int)endPos.x, (int)endPos.y) >= 3)
                {
                    GameObject fantasyRoomToInstantiate = Instantiate(fantasyEndRooms[0], scaledEndPos, Quaternion.identity);
                    fantasyRooms[(int)endPos.x, (int)endPos.y] = roomToInstantiate;
                    spawnEnemies(scaledEndPos, fantasyEnemies, fantasyEndRooms[0].tag);
                    GameObject scienceRoomToInstantiate = Instantiate(scienceEndRooms[0], offsetEndPos, Quaternion.identity);
                    scienceRooms[(int)endPos.x, (int)endPos.y] = roomToInstantiate;
                    spawnEnemies(offsetEndPos, fantasyEnemies, scienceEndRooms[0].tag);
                    endSpaceFarAway = true;

                    if ((int)endPos.z == 1)
                    {
                        GameObject fantasyToDestroy = fantasyRooms[(int)endPos.x, (int)endPos.y - 1];
                        GameObject scienceToDestroy = scienceRooms[(int)endPos.x, (int)endPos.y - 1];
                        Destroy(fantasyToDestroy.transform.Find("northDoorClosed").gameObject);
                        Destroy(fantasyToDestroy.transform.Find("northWall").gameObject);
                        Destroy(scienceToDestroy.transform.Find("northDoorClosed").gameObject);
                        Destroy(scienceToDestroy.transform.Find("northWall").gameObject);

                        Destroy(fantasyRoomToInstantiate.transform.Find("southDoorClosed").gameObject);
                        Destroy(fantasyRoomToInstantiate.transform.Find("southWall").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("southDoorClosed").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("southWall").gameObject);
                    }
                    else if ((int)endPos.z == 2)
                    {
                        GameObject fantasyToDestroy = fantasyRooms[(int)endPos.x, (int)endPos.y + 1];
                        GameObject scienceToDestroy = scienceRooms[(int)endPos.x, (int)endPos.y + 1];
                        Destroy(fantasyToDestroy.transform.Find("southDoorClosed").gameObject);
                        Destroy(fantasyToDestroy.transform.Find("southWall").gameObject);
                        Destroy(scienceToDestroy.transform.Find("southDoorClosed").gameObject);
                        Destroy(scienceToDestroy.transform.Find("southWall").gameObject);

                        Destroy(fantasyRoomToInstantiate.transform.Find("northDoorClosed").gameObject);
                        Destroy(fantasyRoomToInstantiate.transform.Find("northWall").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("northDoorClosed").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("northWall").gameObject);
                    }
                    else if ((int)endPos.z == 3)
                    {
                        GameObject fantasyToDestroy = fantasyRooms[(int)endPos.x - 1, (int)endPos.y];
                        GameObject scienceToDestroy = scienceRooms[(int)endPos.x - 1, (int)endPos.y];
                        Destroy(fantasyToDestroy.transform.Find("eastDoorClosed").gameObject);
                        Destroy(fantasyToDestroy.transform.Find("eastWall").gameObject);
                        Destroy(scienceToDestroy.transform.Find("eastDoorClosed").gameObject);
                        Destroy(scienceToDestroy.transform.Find("eastWall").gameObject);

                        Destroy(fantasyRoomToInstantiate.transform.Find("westDoorClosed").gameObject);
                        Destroy(fantasyRoomToInstantiate.transform.Find("westWall").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("westDoorClosed").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("westWall").gameObject);
                    }
                    else if ((int)endPos.z == 4)
                    {
                        GameObject fantasyToDestroy = fantasyRooms[(int)endPos.x + 1, (int)endPos.y];
                        GameObject scienceToDestroy = scienceRooms[(int)endPos.x + 1, (int)endPos.y];
                        Destroy(fantasyToDestroy.transform.Find("westDoorClosed").gameObject);
                        Destroy(fantasyToDestroy.transform.Find("westWall").gameObject);
                        Destroy(scienceToDestroy.transform.Find("westDoorClosed").gameObject);
                        Destroy(scienceToDestroy.transform.Find("westWall").gameObject);

                        Destroy(fantasyRoomToInstantiate.transform.Find("eastDoorClosed").gameObject);
                        Destroy(fantasyRoomToInstantiate.transform.Find("eastWall").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("eastDoorClosed").gameObject);
                        Destroy(scienceRoomToInstantiate.transform.Find("eastWall").gameObject);
                    }
                }
            }

        }

        //Helper method for calculating distance between starting room position and potential exit room position
        double distanceBetweenPoints(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }


        //This method returns a valid pair of xy coordinates for placing a new room. The third piece of data is the direction the new room was placed in (N=1,S=2,E=3,W=4)
        Vector3 selectCoordinate()
        {
            Boolean coordinateSelected = false;
            while (coordinateSelected == false)
            {
                int randomIndex = Random.Range(0, roomCoordinatesSize - 1); //Draws random index from roomCoordinates
                Vector2 randomCoordinates = roomCoordinates[randomIndex]; //Grabs the coordinates at random index
                int randomId = occupiedSpaces[(int)randomCoordinates.x, (int)randomCoordinates.y]; //Finds corresponding Room ID at coordinates

                int randomDirection = Random.Range(1, 5);
                Vector2 potentialCoordinates = randomCoordinates;
                //================================
                //Insert direction checking here!!!
                //================================
                if (randomDirection == 1)
                {
                    //if(randomId.northPossible && northExists == false){
                    //potentialCoordinates.y += 1;
                    //else if(south...
                    potentialCoordinates.y += 1;
                }
                else if (randomDirection == 2)
                {
                    potentialCoordinates.y -= 1;
                }
                else if (randomDirection == 3)
                {
                    potentialCoordinates.x += 1;
                }
                else if (randomDirection == 4)
                {
                    potentialCoordinates.x -= 1;
                }

                if ((int)potentialCoordinates.x >= 7 || (int)potentialCoordinates.x < 0 || (int)potentialCoordinates.y >= 7 || (int)potentialCoordinates.y < 0)
                {
                    //This means the potential coordinate is out of bounds
                    coordinateSelected = false;
                }
                else if (occupiedSpaces[(int)potentialCoordinates.x, (int)potentialCoordinates.y] != -1)
                {
                    //This means the potential coordinate is already occupied, must select a new potential xy coordinate pair.
                    coordinateSelected = false;
                }
                else
                {
                    //Successfully found a coordinate to place a room
                    if (randomDirection == 1)    //This case occurs when placing a room to the north
                    {
                        coordinateSelected = true;
                        return new Vector3((int)potentialCoordinates.x, (int)potentialCoordinates.y, 1);
                    }
                    else if (randomDirection == 2)    //This case occurs when placing a room to the south
                    {
                        coordinateSelected = true;
                        return new Vector3((int)potentialCoordinates.x, (int)potentialCoordinates.y, 2);
                    }
                    else if (randomDirection == 3)    //This case occurs when placing a room to the east
                    {
                        coordinateSelected = true;
                        return new Vector3((int)potentialCoordinates.x, (int)potentialCoordinates.y, 3);
                    }
                    else if (randomDirection == 4)    //This case occurs when placing a room to the west
                    {
                        coordinateSelected = true;
                        return new Vector3((int)potentialCoordinates.x, (int)potentialCoordinates.y, 4);
                    }
                }
            }
            return new Vector3();
        }

        //Pulls a random room prefab out of the array of rooms
        RoomPair selectRoom()
        {
            int randomRoom = Random.Range(0, fantasyNorthRooms.Length - 1);
            return new RoomPair(fantasyNorthRooms[randomRoom], scienceNorthRooms[randomRoom]);
        }


        void spawnEnemies(Vector2 pos, GameObject[] enemyTypes, String roomID)
        {
            switch (roomID)
            {
                case "0": break;

                case "1":
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (5 * 16), pos.y - (3 * 16)), Quaternion.identity);
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (9 * 16), pos.y - (3 * 16)), Quaternion.identity);
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (13 * 16), pos.y - (3 * 16)), Quaternion.identity);
                    break;

                case "2":
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (7 * 16), pos.y - (3 * 16)), Quaternion.identity);
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (10 * 16), pos.y - (3 * 16)), Quaternion.identity);
                    break;

                case "3":
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (8 * 16), pos.y - (6 * 16)), Quaternion.identity);
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (8 * 16), pos.y - (10 * 16)), Quaternion.identity);
                    break;

                case "4":
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (8 * 16), pos.y - (10 * 16)), Quaternion.identity);
                    Instantiate(enemyTypes[0], new Vector2(pos.x + (8 * 16), pos.y - (10 * 16)), Quaternion.identity);
                    break;

                case "5": break;

                case "6": break;

                case "7": break;

                case "8":
                    Instantiate(enemyTypes[1], new Vector2(pos.x + (8 * 16), pos.y - (10 * 16)), Quaternion.identity);
                    break;

                case "9":
                    Instantiate(enemyTypes[1], new Vector2(pos.x + (8 * 16), pos.y - (10 * 16)), Quaternion.identity);
                    break;

                default: break;

            }

        }



        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene()
        {
            //Reset our list of gridpositions.
            InitialiseDataStructures();

            //Generate the random level
            GenerateMap(fantasyStartRooms, fantasyEndRooms, fantasyNorthRooms, fantasySouthRooms, fantasyEastRooms, fantasyWestRooms, scienceStartRooms, scienceEndRooms, scienceNorthRooms, scienceSouthRooms, scienceEastRooms, scienceWestRooms);

        }
    }
}