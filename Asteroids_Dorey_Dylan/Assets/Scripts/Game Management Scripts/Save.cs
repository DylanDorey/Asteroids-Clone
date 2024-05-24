using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/19/2024]
 * [Saves specified items to JSON file]
 */

public class Save : MonoBehaviour
{
    //singelton for UIManager
    private static Save _instance;
    public static Save Instance { get { return _instance; } }

    //the high score that will be saved
    public int highScore;

    private void Awake()
    {
        //if _instance contains something and it isn't this
        if (_instance != null && _instance != this)
        {
            //Destroy it
            Destroy(this.gameObject);
        }
        else
        {
            //otherwise set this to _instance
            _instance = this;
        }
    }

    /// <summary>
    /// saves the players high score to a JSON file
    /// </summary>
    public void SaveGame()
    {
        //Create a save instance with all the data for the current session saved into it
        ItemsToSave itemToSave = CreateSaveGameObject();

        //create a binary formatter and filestream object by passing a path for the save instance to be saved into
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");

        //serialize the data into bytes and write it to the disk and close the filestream
        bf.Serialize(file, itemToSave);
        file.Close();

        //Reset the highScore after save
        highScore = 0;

        Debug.Log("Game Saved");
    }

    /// <summary>
    /// loads and sets the players highest score from the saved JSON file
    /// </summary>
    public void LoadSave()
    {
        // check to see if a save file exists. If not log saying no current save file
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            //create a binary formatter to deserialize the byte file into a ItemToSave(object name) file
            //open a file and store the File Stream, then use that file as a the file to deserialize
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            ItemsToSave itemToSave = (ItemsToSave)bf.Deserialize(file);
            file.Close();

            //Convert values into game state
            PlayerData.Instance.highScore = itemToSave.playerHighScore;

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No Game Saved");
        }
    }


    /// <summary>
    /// creates a gameobject of type ItemsToSave and sets the highScore int to the players high score
    /// </summary>
    /// <returns> the players highscore </returns>
    private ItemsToSave CreateSaveGameObject()
    {
        //reference to a new highScore save
        ItemsToSave highScore = new ItemsToSave();

        //set the high score object to the players high score
        highScore.playerHighScore = PlayerData.Instance.highScore;

        //return the high score to save
        return highScore;
    }
}
