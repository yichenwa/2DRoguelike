using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class {
 
    public static List<Game> savedGames = new List<Game>();
             
    //it's static so we can call it from anywhere
    public static void Save() {
        SaveLoad.savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    } 
}	