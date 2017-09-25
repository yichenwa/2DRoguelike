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
        public class Count
        {
            public int minimum;             //Minimum value for our Count class.
            public int maximum;             //Maximum value for our Count class.


            //Assignment constructor.
            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }


        public int columns = 8;                                         //Number of columns in our game board.
        public int rows = 8;											//Number of rows in our game board.
        public int newRows = 7;
        public int newCols = 7;
        public Count wallCount = new Count(5, 9);                       //Lower and upper limit for our random number of walls per level.
        public Count foodCount = new Count(1, 5);                       //Lower and upper limit for our random number of food items per level.
        public GameObject exit;                                         //Prefab to spawn for exit.
        public GameObject[] floorTiles;                                 //Array of floor prefabs.
        public GameObject[] wallTiles;                                  //Array of wall prefabs.
        public GameObject[] foodTiles;                                  //Array of food prefabs.
        public GameObject[] enemyTiles;                                 //Array of enemy prefabs.
        public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.

        private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
        private List<Vector3> gridPositions = new List<Vector3>();  //A list of possible locations to place tiles.


        private int[,] occupiedSpaces = new int[7, 7];                   //A matrix that takes an xy coordinate and returns the room ID contained there        
        private List<Vector2> roomCoordinates = new List<Vector2>();    //A list of xy coordinates that have rooms placed in them
        private int roomCoordinatesSize = 0;
        private List<int> hasExit = new List<int>();                   //A list of room IDs that stil have available exits
        private int hasExitSize = 0;


        //Clears our list gridPositions and prepares it to generate a new board.
        void InitialiseList()
        {
            //Clear our list gridPositions.
            gridPositions.Clear();

            //Loop through x axis (columns).
            for (int x = 1; x < newCols - 1; x++)
            {
                //Within each column, loop through y axis (rows).
                for (int y = 1; y < newRows - 1; y++)
                {
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }


            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    occupiedSpaces[x, y] = -1;
                }
            }

            roomCoordinates = new List<Vector2>();
            roomCoordinatesSize = 0;
            hasExit = new List<int>();
            hasExitSize = 0;


        }


        //Sets up the outer walls and floor (background) of the game board.
        void BoardSetup()
        {
            //Instantiate Board and set boardHolder to its transform.
            boardHolder = new GameObject("Board").transform;

            //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
            for (int x = -1; x < newCols + 1; x++)
            {
                //Loop along y axis, starting from -1 to place floor or outerwall tiles.
                for (int y = -1; y < newRows + 1; y++)
                {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if (x == -1 || x == newCols || y == -1 || y == newRows)
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance =
                        Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                }
            }
        }


        //RandomPosition returns a random position from our list gridPositions.
        Vector3 RandomPosition()
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = Random.Range(0, gridPositions.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];

            //Remove the entry at randomIndex from the list so that it can't be re-used.
            gridPositions.RemoveAt(randomIndex);

            //Return the randomly selected Vector3 position.
            return randomPosition;
        }


        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            int objectCount = Random.Range(minimum, maximum + 1);

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (int i = 0; i < objectCount; i++)
            {
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                Vector3 randomPosition = RandomPosition();

                //Choose a random tile from tileArray and assign it to tileChoice
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }





        void GenerateMap(GameObject[] start, GameObject[] end, GameObject[] normal)
        {
            Vector2 startPos = new Vector2(3, 3);
            Instantiate(start[0], startPos, Quaternion.identity);

            occupiedSpaces[3, 3] = 1;
            roomCoordinates.Add(startPos);
            roomCoordinatesSize += 1;
            hasExit.Add(1);
            hasExitSize += 1;

            for (int i = 0; i < 8; i++)
            {
                Vector2 coordinate = selectCoordinate();
                Instantiate(normal[0], coordinate, Quaternion.identity);
                occupiedSpaces[(int)coordinate.x, (int)coordinate.y] = 2;
                roomCoordinates.Add(coordinate);
                roomCoordinatesSize += 1;
                hasExit.Add(2);
                hasExitSize += 1;
            }

            Boolean endSpaceFarAway = false;
            while (endSpaceFarAway == false)
            {
                Vector2 endPos = selectCoordinate();
                if (distanceBetweenPoints((int)startPos.x, (int)startPos.y, (int)endPos.x, (int)endPos.y) >= 3)
                {
                    Instantiate(end[0], endPos, Quaternion.identity);
                    endSpaceFarAway = true;
                }
                //print(Math.Abs(endPos.magnitude-startPos.magnitude));
                //print(endPos.x);
                //print(endPos.y);

                //Instantiate(end[0], coordinate, Quaternion.identity);
            }

        }

        double distanceBetweenPoints(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }


        Vector2 selectCoordinate()
        {
            Boolean coordinateSelected = false;
            while (coordinateSelected == false)
            {
                int randomIndex = Random.Range(0, roomCoordinatesSize - 1); //Draws random index from roomCoordinates
                Vector2 randomCoordinates = roomCoordinates[randomIndex]; //Grabs the coordinates at random index
                int randomId = occupiedSpaces[(int)randomCoordinates.x, (int)randomCoordinates.y]; //Finds corresponding Room ID at coordinates

                int randomDirection = Random.Range(1, 4);
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
                    //print("This space is occupied!!!");
                    //print((int)potentialCoordinates.x);
                    //print((int)potentialCoordinates.y);
                    //This means the potential coordinate is already occupied, must select a new potential xy coordinate pair.
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
                    //print((int)potentialCoordinates.x);
                    //print((int)potentialCoordinates.y);
                    coordinateSelected = true;
                    return potentialCoordinates;
                }
            }
            return new Vector2();
        }









        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {
            //Creates the outer walls and floor.
            BoardSetup();

            //Reset our list of gridpositions.
            InitialiseList();

            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
            GenerateMap(foodTiles, enemyTiles, wallTiles);

            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

            //Determine number of enemies based on current level number, based on a logarithmic progression
            int enemyCount = (int)Mathf.Log(level, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

            //Instantiate the exit tile in the upper right hand corner of our game board
            Instantiate(exit, new Vector3(newCols - 1, newRows - 1, 0f), Quaternion.identity);
        }
    }
}