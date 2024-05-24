using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [1/29/2024]
 * [This will manage all game functions initializing, starting, ending, and navigating]
 */

//enum for the various states of the game
public enum GameState
{
    menu,
    playing,
    roundOver,
    gameOver
}

public class GameManager : MonoBehaviour
{
    //singelton for GameManager
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    //reference to game state
    public GameState gameState;

    //value to determine what level the player is on
    public int currentLevel = 0;

    //bools for restarting the game
    public bool hasRestarted = false;
    public bool allEnemiesRemoved = false;

    //screen bounds for screen wrap
    private float bottomMax = -9.1f;
    private float topMax = 9.1f;
    private float leftMax = -5.28f;
    private float rightMax = 5.28f;

    void Awake()
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

    private void Start()
    {
        //start the game at the menu
        gameState = GameState.menu;

        //spawn the enemies for the main menu background
        EnemySpawner.Instance.DisplayMenuBackground();
    }

    /// <summary>
    /// This will intialize all game elements for Asteroids
    /// </summary>
    private void InitializeGame()
    {
        //clear the list of enemies and enemies present on the level
        EnemySpawner.Instance.ClearList();

        //set ufo status' to false
        EnemySpawner.Instance.hasBigUFO = false;
        EnemySpawner.Instance.hasSmallUFO = false;

        //initialize the player controller and player data
        PlayerController.Instance.InitializePlayerController();
        PlayerData.Instance.InitializePlayerData();

        //set the current level to 0
        currentLevel = 0;
    }

    /// <summary>
    /// This will start the game Asteroids
    /// </summary>
    public void StartGame()
    {
        //initialize background elements
        InitializeGame();

        //start a new round
        StartNextRound();

        //load the high score of the player from JSON file
        Save.Instance.LoadSave();
    }

    /// <summary>
    /// starts new round of enemies
    /// </summary>
    public void StartNextRound()
    {
        //set gamestate to playing
        gameState = GameState.playing;

        //increase the current level by 1
        currentLevel++;

        //display round information
        UIManager.Instance.StartCoroutine(UIManager.Instance.NextRoundText());

        //calculate the next round's amount of enemies to spawn based on the current level
        EnemySpawner.Instance.CalculateSpawnAmount(currentLevel);

        //start spawning the enemies for that round
        EnemySpawner.Instance.StartCoroutine(EnemySpawner.Instance.StartSpawningAsteroids());
    }

    /// <summary>
    /// This will allow the user to close/quit Asteroids
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// This will send the user back to the main menu
    /// </summary>
    public void ReturnToMenu()
    {
        //sets the high score at the end of the game
        PlayerData.Instance.SetHighScore();

        //saves the high score in JSON file
        Save.Instance.SaveGame();

        //set game state to menu
        gameState = GameState.menu;
    }

    /// <summary>
    /// This will make any object that goes off screen wrap to the opposite side
    /// </summary>
    /// <param name="objectToWrap"> the object that will wrap the screen </param>
    public void ScreenWrap(GameObject objectToWrap)
    {
        //switch for the x position of the object that needs to be screen wrapped
        switch (objectToWrap.transform.position.x)
        {
            //if the object's x position is greater than the max screen height, set the x position to the min screen height
            case > 9.1f:
                objectToWrap.transform.position = new Vector3(bottomMax, objectToWrap.transform.position.y, objectToWrap.transform.position.z);
                break;
            //if the object's x position is less than the min screen height, set the x position to the max screen height
            case < -9.1f:
                objectToWrap.transform.position = new Vector3(topMax, objectToWrap.transform.position.y, objectToWrap.transform.position.z);
                break;
        }

        //switch for the y position of the object that needs to be screen wrapped
        switch (objectToWrap.transform.position.y)
        {
            //if the object's y position is greater than the max screen width, set the y position to the min screen height
            case > 5.28f:
                objectToWrap.transform.position = new Vector3(objectToWrap.transform.position.x, leftMax, objectToWrap.transform.position.z);
                break;
            //if the object's y position is less than the min screen width, set the y position to the max screen height
            case < -5.28f:
                objectToWrap.transform.position = new Vector3(objectToWrap.transform.position.x, rightMax, objectToWrap.transform.position.z);
                break;
        }
    }
}
