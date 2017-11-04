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

        //RoomPair is a custom object we created which holds two GameObjects: a fantasy room and its corresponding science room.
        public class RoomPair
        {
            public GameObject _fantasy;             //Fantasy room prefab
            public GameObject _science;             //Science room prefab

            //RoomPair constructor.
            public RoomPair(GameObject fantasy, GameObject science)
            {
                _fantasy = fantasy;
                _science = science;
            }
        }

        public GameObject[] fantasyStartRooms;                                 //Array of fantasy starting room prefabs
        public GameObject[] fantasyNormalRooms;                                //Array of normal fantasy room prefabs
        public GameObject[] fantasyEndRooms;                                   //Array of fantasy end room prefabs
        public GameObject[] fantasyKeyAndLockRooms;                            //Array of fantasy key rooms immediately followed by their corresponding lock rooms

        public GameObject[] scienceStartRooms;                                 //Array of science starting room prefabs
        public GameObject[] scienceNormalRooms;                                //Array of normal science room prefabs
        public GameObject[] scienceEndRooms;                                   //Array of science end room prefabs
        public GameObject[] scienceKeyAndLockRooms;                            //Array of science key rooms immediately followed by their corresponding lock rooms

        public GameObject[] fantasyEnemies;                                    //Array of enemy prefabs

        private GameObject[,] fantasyRooms = new GameObject[7, 7];      //A matrix that holds the fantasy rooms that have been instantiated
        private GameObject[,] scienceRooms = new GameObject[7, 7];      //A matrix that holds the science rooms that have been instantiated
        private List<Vector2> roomCoordinates = new List<Vector2>();    //A list of xy coordinates that have rooms placed in them
        private int roomCoordinatesSize = 0;                            //The size of the list roomCoordinates

        //SetupScene gets called from the GameManager script. 
        //SetupScene first calls a method to perform initial data structure initialization.
        //Next it then calls another method to generate a complete random level layout.
        public void SetupScene()
        {
            //Resets and initializes the data structures used for level generation.
            InitialiseDataStructures();

            //Generates a random level.
            GenerateMap();
        }


        //Clears data structures in preparation for generating level
        void InitialiseDataStructures()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++) //This loop makes sure every position in fantasyRooms and scienceRooms are empty.
                {
                    fantasyRooms[x, y] = null;
                    scienceRooms[x, y] = null;
                }
            }
            roomCoordinates = new List<Vector2>();
            roomCoordinatesSize = 0;
        }



        //This method places starting room, places a certain number of regular rooms in a loop, then places an exit room.
        void GenerateMap()
        {
            //Levels are randomly generated with rooms being placed into a 7 by 7 grid.
            //The starting room is always placed in the middle of the grid at position (3,3).
            //The length of each one of our rooms is 256 units and the height is 176 units.
            //A fantasy version of each level and a science version of each level are generated simultaneously, but offset by 2000 units in the x dimension.

            Vector2 startPos = new Vector2(3, 3);   //Position of the start room in the model of our level grid.
            Vector2 scaledStartPos = new Vector2(3 * 256, 3 * 176); //Actual position of the fantasy room being instantiated in Unity.
            Vector2 offsetStartPos = new Vector2((3 * 256) + 2000, 3 * 176);    //Actual position of the science room being instantiated in Unity (notice 2000 unit offset in x dimension).

            //Instantiates fantasy room at starting position and adds room to our grid model.
            GameObject roomToInstantiate = Instantiate(fantasyStartRooms[0], scaledStartPos, Quaternion.identity);
            fantasyRooms[3, 3] = roomToInstantiate;

            //Instantiates science room at starting position and adds room to our grid model.
            roomToInstantiate = Instantiate(scienceStartRooms[0], offsetStartPos, Quaternion.identity);
            scienceRooms[3, 3] = roomToInstantiate;

            //Adds the starting coordinate position (3,3) to our roomCoordinate list.
            //Coordinates contained in this list are occupied.
            roomCoordinates.Add(startPos);
            roomCoordinatesSize += 1;

            for (int i = 0; i < 8; i++) //This loop places 8 "normal" rooms that are not starting rooms, exit rooms, key rooms, or lock rooms.
            {
                Vector3 coordinate = selectCoordinate();    //Calls function that will return an available coordinate pair for instantiating a new room.
                Vector2 scaledCoordinate = new Vector2((int)coordinate.x * 256, (int)coordinate.y * 176);   //Multiply room coordinate by the dimensions of our rooms.
                Vector2 offsetCoordinate = new Vector2(((int)coordinate.x * 256) + 2000, (int)coordinate.y * 176);  //Applies 2000 unit offset for science room.

                RoomPair fanAndSciRoom = selectRoom();  //Calls method that will return a pair of rooms to be instantiated (one fantasy and one science room).

                GameObject fantasyRoomToInstantiate = Instantiate(fanAndSciRoom._fantasy, scaledCoordinate, Quaternion.identity); //Instantiates fantasy room
                fantasyRooms[(int)coordinate.x, (int)coordinate.y] = fantasyRoomToInstantiate;  //Adds just instantiated fantasy room to fantasyRooms matrix at appropriate coordinates.
                spawnEnemies(scaledCoordinate, fantasyEnemies, fanAndSciRoom._fantasy.tag);     //This method instantiates enemies at the most recently instantiated fantasy room.

                GameObject scienceRoomToInstantiate = Instantiate(fanAndSciRoom._science, offsetCoordinate, Quaternion.identity);   //Instantiates science room
                scienceRooms[(int)coordinate.x, (int)coordinate.y] = scienceRoomToInstantiate;  //Adds just instantiated science room to scienceRooms matrix at appropriate coordinates.
                spawnEnemies(offsetCoordinate, fantasyEnemies, fanAndSciRoom._science.tag);     //This method instantiates enemies at the most recently instantiated science room.

                //Opens the doors between the newly instnatiated room and its neighbor so that the player character can move between rooms.
                openDoors((int)coordinate.z, (int)coordinate.x, (int)coordinate.y, fantasyRoomToInstantiate, scienceRoomToInstantiate);

                //Adds the new coordinates to the list of occupied coordinates.
                roomCoordinates.Add(coordinate);
                roomCoordinatesSize += 1;
            }

            //Places a room containing a key somewhere in the level and stores the coordinates of the key room in variable keyPos.
            Vector2 keyPos = placeKeyRoom();

            Boolean lockAndKeyFarAway = false;    //This variable will be set to true if the locked room is placed at least 3 spaces away from the key room.
            while (lockAndKeyFarAway == false)    //This loop continues until the locked room is placed far enough away from the key room.
            {
                Vector3 lockPos = selectCoordinate();   //Potential coordinates for placing the locked room.
                Vector3 endPos = new Vector3();
                Boolean possibleToPlaceExit = false;    //This variable will be set to true if there is an available space to place the exit room next to the locked room after placing locked room.

                if (isCoordinateEmpty((int)lockPos.x, (int)lockPos.y + 1))  //If true exit room will be placed north of locked room.
                {
                    endPos = new Vector3((int)lockPos.x, (int)lockPos.y + 1, 1);
                    possibleToPlaceExit = true;
                }
                else if (isCoordinateEmpty((int)lockPos.x, (int)lockPos.y - 1))     //If true exit room will be placed south of locked room.
                {
                    endPos = new Vector3((int)lockPos.x, (int)lockPos.y - 1, 2);
                    possibleToPlaceExit = true;
                }
                else if (isCoordinateEmpty((int)lockPos.x + 1, (int)lockPos.y))     //If true exit room will be placed east of locked room.
                {
                    endPos = new Vector3((int)lockPos.x + 1, (int)lockPos.y, 3);
                    possibleToPlaceExit = true;
                }
                else if (isCoordinateEmpty((int)lockPos.x - 1, (int)lockPos.y))     //If true exit room will be placed west of locked room.
                {
                    endPos = new Vector3((int)lockPos.x - 1, (int)lockPos.y, 4);
                    possibleToPlaceExit = true;
                }

                if (possibleToPlaceExit == true)
                {
                    //This condition checks to make sure the key room and locked room are at least 3 spaces apart.
                    if (distanceBetweenPoints((int)keyPos.x, (int)keyPos.y, (int)lockPos.x, (int)lockPos.y) >= 3)
                    {
                        placeLockRoom(lockPos);     //Places the locked room at coordinates in lockPos.
                        Vector2 scaledEndPos = new Vector2((int)endPos.x * 256, (int)endPos.y * 176);
                        Vector2 offsetEndPos = new Vector2(((int)endPos.x * 256) + 2000, (int)endPos.y * 176);

                        //Instantiates fantasy exit room, adds the room to our model, and spawns enemies in the exit room.
                        GameObject fantasyRoomToInstantiate = Instantiate(fantasyEndRooms[0], scaledEndPos, Quaternion.identity);
                        fantasyRooms[(int)endPos.x, (int)endPos.y] = roomToInstantiate;
                        spawnEnemies(scaledEndPos, fantasyEnemies, fantasyEndRooms[0].tag);

                        //Instantiates science exit room, adds the room to our model, and spawns enemies in the exit room.
                        GameObject scienceRoomToInstantiate = Instantiate(scienceEndRooms[0], offsetEndPos, Quaternion.identity);
                        scienceRooms[(int)endPos.x, (int)endPos.y] = roomToInstantiate;
                        spawnEnemies(offsetEndPos, fantasyEnemies, scienceEndRooms[0].tag);

                        lockAndKeyFarAway = true;

                        //Opens the doors between the exit room and the locked room so that the player character can move between rooms.
                        openDoors((int)endPos.z, (int)endPos.x, (int)endPos.y, fantasyRoomToInstantiate, scienceRoomToInstantiate);
                    }
                }
            }

        }


        //openDoors deactivates the closed doors and walls between a newly created room and a previously existing room so that the player can move between the two rooms.
        //fantasyRoomToInstantiate and scienceRoomToInstantiate are GameObjects corresponding to the newly created fantasy and science rooms respectively.
        //fantasyToDestroy and scienceToDestroy are GameObjects corresponding to the previously existing fantasy and science rooms respectively.
        //relativeDirections indicates which side of the previously existing room the newly created room is being instantiated.
        void openDoors(int relativeDirection, int x, int y, GameObject fantasyRoomToInstantiate, GameObject scienceRoomToInstantiate)
        {
            if (relativeDirection == 1)     //Placing a room to the north of a previously existing room
            {
                GameObject fantasyToDestroy = fantasyRooms[x, y-1];
                GameObject scienceToDestroy = scienceRooms[x, y-1];

                //Destroy the north closed door and wall of the previously existing room
                fantasyToDestroy.transform.Find("northDoorClosed").gameObject.SetActive(false);
                fantasyToDestroy.transform.Find("northWall").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("northDoorClosed").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("northWall").gameObject.SetActive(false);

                //Destroy the south closed door and wall of the newly created room
                fantasyRoomToInstantiate.transform.Find("southDoorClosed").gameObject.SetActive(false);
                fantasyRoomToInstantiate.transform.Find("southWall").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("southDoorClosed").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("southWall").gameObject.SetActive(false);

            }
            else if (relativeDirection == 2)    //Placing a room to the south of a previously existing room
            {
                GameObject fantasyToDestroy = fantasyRooms[x, y+1];
                GameObject scienceToDestroy = scienceRooms[x, y+1];

                //Destroy the south closed door and wall of the previously existing room
                fantasyToDestroy.transform.Find("southDoorClosed").gameObject.SetActive(false);
                fantasyToDestroy.transform.Find("southWall").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("southDoorClosed").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("southWall").gameObject.SetActive(false);

                //Destroy the north closed door and wall of the newly created room
                fantasyRoomToInstantiate.transform.Find("northDoorClosed").gameObject.SetActive(false);
                fantasyRoomToInstantiate.transform.Find("northWall").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("northDoorClosed").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("northWall").gameObject.SetActive(false);
            }
            else if (relativeDirection == 3)    //Placing a room to the east of a previously existing room
            {
                GameObject fantasyToDestroy = fantasyRooms[x-1, y];
                GameObject scienceToDestroy = scienceRooms[x-1, y];

                //Destroy the east closed door and wall of the previously existing room
                fantasyToDestroy.transform.Find("eastDoorClosed").gameObject.SetActive(false);
                fantasyToDestroy.transform.Find("eastWall").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("eastDoorClosed").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("eastWall").gameObject.SetActive(false);

                //Destroy the west closed door and wall of the newly created room
                fantasyRoomToInstantiate.transform.Find("westDoorClosed").gameObject.SetActive(false);
                fantasyRoomToInstantiate.transform.Find("westWall").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("westDoorClosed").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("westWall").gameObject.SetActive(false);
            }
            else if (relativeDirection == 4)    //Placing a room to the west of a previously existing room
            {
                GameObject fantasyToDestroy = fantasyRooms[x+1, y];
                GameObject scienceToDestroy = scienceRooms[x+1, y];

                //Destroy the west closed door and wall of the previously existing room
                fantasyToDestroy.transform.Find("westDoorClosed").gameObject.SetActive(false);
                fantasyToDestroy.transform.Find("westWall").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("westDoorClosed").gameObject.SetActive(false);
                scienceToDestroy.transform.Find("westWall").gameObject.SetActive(false);

                //Destroy the east closed door and wall of the newly created room
                fantasyRoomToInstantiate.transform.Find("eastDoorClosed").gameObject.SetActive(false);
                fantasyRoomToInstantiate.transform.Find("eastWall").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("eastDoorClosed").gameObject.SetActive(false);
                scienceRoomToInstantiate.transform.Find("eastWall").gameObject.SetActive(false);
            }
        }



        //This method returns a valid pair of xy coordinates for placing a new room. 
        //The z coordinate is the direction the new room was placed relative to a previously existing room (N=1,S=2,E=3,W=4)
        Vector3 selectCoordinate()
        {
            Boolean coordinateSelected = false;
            while (coordinateSelected == false)
            {
                int randomIndex = Random.Range(0, roomCoordinatesSize - 1); //Draws random index from roomCoordinates
                Vector2 potentialCoordinates = roomCoordinates[randomIndex]; //Grabs the coordinates at random index, this coordinate is occupied by a room.

                int randomDirection = Random.Range(1, 5); //Random number between 1 and 4, will be used for picking a direction.

                if (randomDirection == 1)   //Will check to see if the coordinates north of a previously exiting room are available to place a new room there.
                {
                    potentialCoordinates.y += 1;
                }
                else if (randomDirection == 2)      //Will check to see if the coordinates south of a previously exiting room are available to place a new room there.
                {
                    potentialCoordinates.y -= 1;
                }
                else if (randomDirection == 3)      //Will check to see if the coordinates east of a previously exiting room are available to place a new room there.
                {
                    potentialCoordinates.x += 1;
                }
                else if (randomDirection == 4)      //Will check to see if the coordinates west of a previously exiting room are available to place a new room there.
                {
                    potentialCoordinates.x -= 1;
                }

                if ((int)potentialCoordinates.x >= 7 || (int)potentialCoordinates.x < 0 || (int)potentialCoordinates.y >= 7 || (int)potentialCoordinates.y < 0)
                {
                    //This means the potential coordinate is out of bounds of our 7 by 7 grid model, must select a new potential xy coordinate pair.
                    coordinateSelected = false;
                }
                else if (fantasyRooms[(int)potentialCoordinates.x, (int)potentialCoordinates.y] != null)
                {
                    //This means the potential coordinate is already occupied, must select a new potential xy coordinate pair.
                    coordinateSelected = false;
                }
                else
                {
                    //Successfully found an available coordinate to place a room
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
            return new Vector3(); //This statement should never be reached and therefore never return.
        }



        //Pulls a random room prefab out of the array of rooms
        RoomPair selectRoom()
        {
            int randomRoom = Random.Range(0, fantasyNormalRooms.Length);
            return new RoomPair(fantasyNormalRooms[randomRoom], scienceNormalRooms[randomRoom]);
        }



        //Helper method for calculating the distance between any two rooms given their respective x and y coordinates.
        double distanceBetweenPoints(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }


        //Helper method returns true when xy coordinate pair is available for instantiating a new room, false otherwise.
        Boolean isCoordinateEmpty(int x, int y)
        {
            if(x >= 7 || x < 0 || y >= 7 || y< 0)   //xy coordinate is out of bounds of our 7 by 7 grid model.
            {
                return false;
            }
            else if (fantasyRooms[x, y] == null)    //xy coordinate is available.
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        
        //Method will place a key room and return the xy coordinates where it was instantiated.
        Vector2 placeKeyRoom()
        {
            Vector3 coordinate = selectCoordinate();
            Vector2 scaledCoordinate = new Vector2((int)coordinate.x * 256, (int)coordinate.y * 176);   //Multiply room coordinate by the dimensions of our rooms.
            Vector2 offsetCoordinate = new Vector2(((int)coordinate.x * 256) + 2000, (int)coordinate.y * 176);  //Applies 2000 unit offset for science room.

            GameObject fantasyRoomToInstantiate = Instantiate(fantasyKeyAndLockRooms[0], scaledCoordinate, Quaternion.identity);
            fantasyRooms[(int)coordinate.x, (int)coordinate.y] = fantasyRoomToInstantiate;
            spawnEnemies(scaledCoordinate, fantasyEnemies, fantasyKeyAndLockRooms[0].tag);

            GameObject scienceRoomToInstantiate = Instantiate(scienceKeyAndLockRooms[0], offsetCoordinate, Quaternion.identity);
            scienceRooms[(int)coordinate.x, (int)coordinate.y] = scienceRoomToInstantiate;
            spawnEnemies(offsetCoordinate, fantasyEnemies, scienceKeyAndLockRooms[0].tag);

            openDoors((int)coordinate.z, (int)coordinate.x, (int)coordinate.y, fantasyRoomToInstantiate, scienceRoomToInstantiate);

            roomCoordinates.Add(coordinate);
            roomCoordinatesSize += 1;

            return new Vector2(coordinate.x, coordinate.y);
        }



        //Method will place a locked room at the coordinates specified by Vector3 coordinate.
        void placeLockRoom(Vector3 coordinate)
        {
            Vector2 scaledCoordinate = new Vector2((int)coordinate.x * 256, (int)coordinate.y * 176);   //Multiply room coordinate by the dimensions of our rooms.
            Vector2 offsetCoordinate = new Vector2(((int)coordinate.x * 256) + 2000, (int)coordinate.y * 176);  //Applies 2000 unit offset for science room.

            GameObject fantasyRoomToInstantiate = Instantiate(fantasyKeyAndLockRooms[1], scaledCoordinate, Quaternion.identity);
            fantasyRooms[(int)coordinate.x, (int)coordinate.y] = fantasyRoomToInstantiate;
            spawnEnemies(scaledCoordinate, fantasyEnemies, fantasyKeyAndLockRooms[1].tag);

            GameObject scienceRoomToInstantiate = Instantiate(scienceKeyAndLockRooms[1], offsetCoordinate, Quaternion.identity);
            scienceRooms[(int)coordinate.x, (int)coordinate.y] = scienceRoomToInstantiate;
            spawnEnemies(offsetCoordinate, fantasyEnemies, scienceKeyAndLockRooms[1].tag);

            openDoors((int)coordinate.z, (int)coordinate.x, (int)coordinate.y, fantasyRoomToInstantiate, scienceRoomToInstantiate);

            roomCoordinates.Add(coordinate);
            roomCoordinatesSize += 1;
        }


        //Method will spawn enemies within a room depending on the room's particular ID.
        //Vector2 pos is an xy coordinate pair of the room having enemies spawned within it.
        //enemyTypes is an array of enemy GameObjects that can be instantiated. Currently the enemy at index 0 is a regular enemy and the enemy at index 1 is a boss enemy.
        void spawnEnemies(Vector2 pos, GameObject[] enemyTypes, String roomID)
        {
            //Switch statment based on room ID, this is because different rooms have different enemy placements.
            switch (roomID)
            {
                //Important note: each one of our rooms is 16 blocks long and 11 blocks high.
                //Each one of these "blocks" is a square measuring 16 Unity units by 16 Unity units.
                //Therefore, enemies are being instantiated at coordinates that are some multiple of 16, to keep placement consistent with our room dimensions.
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



    }
}