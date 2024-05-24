using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [2/03/2024]
 * [Holds all relevent player data, and saves high score]
 */

public class PlayerData : MonoBehaviour
{
    //singelton for PlayerData
    private static PlayerData _instance;
    public static PlayerData Instance { get { return _instance; } }

    //ints that track the players current score, lives remaining, and highest score
    public int playerScore;
    public int playerLives;
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
    /// This sets the highest score of the player
    /// </summary>
    /// <returns> Player's highest score </returns>
    public int SetHighScore()
    {
        //if the score for the game is higher than the highScore
        if (playerScore > highScore)
        {
            //set highScore equal to playerScore
            highScore = playerScore;
        }

        return highScore;
    }

    /// <summary>
    /// Sets all of the player data back to its default values
    /// </summary>
    public void InitializePlayerData()
    {
        //set score and lives back to default
        playerScore = 0;
        playerLives = 3;
    }
}
