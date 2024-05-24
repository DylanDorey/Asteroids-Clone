using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/06/2024]
 * [Manages all UI functions]
 */

//enum for the various states of the game
public enum ScreenState
{
    mainMenu,
    playing,
    gameOver
}

public class UIManager : MonoBehaviour
{
    //singelton for UIManager
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    //reference to the ScreenState enum
    private ScreenState screenState;

    //reference to different UI screens
    public GameObject menuScreen;
    public GameObject playingScreen;
    public GameObject gameOverScreen;

    //reference to life sprites
    public Image life1;
    public Image life2;
    public Image life3;

    //score text that corresponds with the players score
    public TextMeshProUGUI scoreText;

    //high score text that corresponds with the players highest score
    public TextMeshProUGUI highScoreText;

    //round text that will display in the center of the screen
    public TextMeshProUGUI centerText;

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

    void Update()
    {
        //if the game state is set to playing
        if (GameManager.Instance.gameState == GameState.playing)
        {
            //update the lives UI images to current player lives
            CheckLives();

            //update the high score to display the players high score
            highScoreText.text = "High Score " + PlayerData.Instance.highScore.ToString();

            //update the score to display the players current score
            scoreText.text = PlayerData.Instance.playerScore.ToString();
        }

        //based upon the game state, set the screen state to corresponding screen
        CheckScreenState();
    }

    /// <summary>
    /// This will show how many lives the player has left on screen
    /// </summary>
    private void CheckLives()
    {
        //switch for the amount of player lives
        switch (PlayerData.Instance.playerLives)
        {
            //if the player has 3 lives left, display 3 lives
            case 3:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(true);
                break;

            //if the player has 2 lives left, display 2 lives
            case 2:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(true);
                life3.gameObject.SetActive(false);
                break;

            //if the player has 1 life left, display 1 life
            case 1:
                life1.gameObject.SetActive(true);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                break;

            //default to disable all lives
            default:
                life1.gameObject.SetActive(false);
                life2.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                break;
        }
    }


    /// <summary>
    /// Changes the UI screen based upon what the screen state is
    /// </summary>
    private void CheckScreenState()
    {
        //switch for the state of the game
        switch (GameManager.Instance.gameState)
        {
            //if the game state is currently menu, disable all panels but the menuScreen
            case GameState.menu:
                menuScreen.gameObject.SetActive(true);
                playingScreen.gameObject.SetActive(false);
                gameOverScreen.gameObject.SetActive(false);
                Cursor.visible = true;
                break;

            //if the game state is currently playing, disable all panels but the playingScreen
            case GameState.playing:
                menuScreen.gameObject.SetActive(false);
                playingScreen.gameObject.SetActive(true);
                gameOverScreen.gameObject.SetActive(false);
                Cursor.visible = false;
                break;

            //if the game state is currently game over, disable all panels but the gameOverScreen
            case GameState.gameOver:
                menuScreen.gameObject.SetActive(false);
                playingScreen.gameObject.SetActive(false);
                gameOverScreen.gameObject.SetActive(true);
                Cursor.visible = true;
                break;
            
            //default to all screens disabled
            default:
                menuScreen.gameObject.SetActive(false);
                playingScreen.gameObject.SetActive(false);
                gameOverScreen.gameObject.SetActive(false);
                break;
        }

        //if the game state is playing and the player has 0 lives
        if (GameManager.Instance.gameState == GameState.playing && PlayerData.Instance.playerLives <= 0)
        {
            //set player state to dead
            PlayerController.Instance.isDead = true;

            //set screen state to game over
            GameManager.Instance.gameState = GameState.gameOver;
        }
    }

    /// <summary>
    /// displays the round the player is on
    /// </summary>
    /// <returns> the display time of the center text </returns>
    public IEnumerator NextRoundText()
    {
        for (int index = 0; index < 1; index++)
        {
            //set the center text to the next round for 2 seconds
            centerText.text = "Round " + GameManager.Instance.currentLevel;

            yield return new WaitForSeconds(2f);
        }

        //clear the center text field
        centerText.text = "";
    }
}
