using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/06/2024]
 * [Spawns new enemies every round and handles all round based spawning]
 */

public class EnemySpawner : MonoBehaviour
{
    //singelton for EnemySpawner
    private static EnemySpawner _instance;
    public static EnemySpawner Instance { get { return _instance; } }

    //list of enemies still alive on current level
    public List<GameObject> enemiesAlive = new List<GameObject>();

    //the amount of enemies to spawn each round
    public int enemiesToSpawn;

    //the time between each enemy spawning
    public float timeBetweenSpawns = 2f;

    //enemy prefabs
    public GameObject largeAsteroidPrefab;
    public GameObject bigUFOPrefab;
    public GameObject smallUFOPrefab;

    //the spawning location for the enemies
    public Transform spawnLoc;

    //bool to determine if there is already a big/small UFO present for the current level
    public bool hasBigUFO = false;
    public bool hasSmallUFO = false;

    //a random index to spawn in a big/small UFO
    private int randomNumIndex;

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

    private void Update()
    {
        //if the game is in the playing gamestate
        if (GameManager.Instance.gameState == GameState.playing)
        {
            //check the status of the current round and when to spawn in a big UFO
            CheckRoundStatus();
            SpawnUFO();
        }
    }

    /// <summary>
    /// spawns an enemey asteroid at a random x position
    /// </summary>
    private void SpawnEnemy()
    {
        //Initialize a new vector3 variable for the random x axis position of the asteroid
        Vector3 randomXLoc = new Vector3(Random.Range(-8f, 8f), spawnLoc.position.y, spawnLoc.position.z);

        //spawn a large asteroid at a random x pos
        GameObject enemy = Instantiate(largeAsteroidPrefab, randomXLoc, Quaternion.identity);

        //set a random rotation on spawn
        enemy.transform.Rotate(0f, 0f, Random.Range(0f, 270f));

        //Add enemy to list of alive enemies
        enemiesAlive.Add(enemy);
    }

    /// <summary>
    /// spawns the big UFO when the randomNumIndex reaches a certain condition
    /// </summary>
    private void SpawnUFO()
    {
        //if the level has not had a big UFO yet
        if (!hasBigUFO)
        {
            //if the randomNumIndex is equal to 1
            if (randomNumIndex == 1)
            {
                //remove conditional statement
                randomNumIndex = 0;

                //if a small UFO has already been spawned in
                if (hasSmallUFO)
                {
                    //stop calling random numbers for conditional statement
                    StopCoroutine(CallRandomNum());
                }

                //instantiate the Big UFO
                GameObject UFO = Instantiate(bigUFOPrefab, spawnLoc.position, Quaternion.identity);

                //Add the UFO to the list of enemies alive
                enemiesAlive.Add(UFO);

                //set hasBigUFO to true for the remainder of the current round
                hasBigUFO = true;
            }
        }

        //if the level has not had a small UFO yet
        if (!hasSmallUFO)
        {
            //if the randomNumIndex is equal to 1
            if (randomNumIndex == 2)
            {
                //remove conditional statement
                randomNumIndex = 0;

                //if a big UFO has already been spawned in
                if (hasBigUFO)
                {
                    //stop calling random numbers for conditional statement
                    StopCoroutine(CallRandomNum());
                }

                //instantiate the Big UFO
                GameObject UFO = Instantiate(smallUFOPrefab, spawnLoc.position, Quaternion.identity);

                //Add the UFO to the list of enemies alive
                enemiesAlive.Add(UFO);

                //set hasBigUFO to true for the remainder of the current round
                hasSmallUFO = true;
            }
        }
    }

    /// <summary>
    /// Starts spawning asteroids and UFO based upon level
    /// </summary>
    /// <returns> time between each enemy spawn </returns>
    public IEnumerator StartSpawningAsteroids()
    {
        //start calling random numbers to randomly spawn a UFO
        StartCoroutine(CallRandomNum());

        //spawn the proper amount of enemies given the enemiesToSpawn value
        for (int index = 0; index < enemiesToSpawn; index++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    /// <summary>
    /// calculates the proper amount of enemies to spawn based upon the current level
    /// </summary>
    /// <param name="currentLevel"> the level the player is on </param>
    /// <returns> the amount of enemies to spawn </returns>
    public int CalculateSpawnAmount(int currentLevel)
    {
        //set spawn amount to the current level times a random multiplier of 1 or 2 plus 2 enemies
        enemiesToSpawn = (currentLevel * Random.Range(1, 2)) + 2;

        return enemiesToSpawn;
    }

    /// <summary>
    /// Checks if the current round is over
    /// </summary>
    public void CheckRoundStatus()
    {
        //if all of the enemies are eliminated for the round
        if (enemiesAlive.Count == 0)
        {
            //stop calling numbers at the end of the round
            StopCoroutine(CallRandomNum());

            //set the gameState to gameOver
            GameManager.Instance.gameState = GameState.roundOver;

            hasBigUFO = false;
            hasSmallUFO = false;

            //start the next round
            GameManager.Instance.StartNextRound();
        }
    }

    /// <summary>
    /// Calls a random num every 3 seconds while there is not a Big UFO in the current level
    /// </summary>
    /// <returns> time to wait before calling another random number </returns>
    private IEnumerator CallRandomNum()
    {
        //if the game state is set to playing
        if (GameManager.Instance.gameState == GameState.playing)
        {
            //while there is not a big UFO present during the round
            while (!hasBigUFO || !hasSmallUFO)
            {
                //set the random index equal to a random number 0-9 every 3 seconds
                randomNumIndex = Random.Range(0, 10);

                yield return new WaitForSeconds(3f);
            }
        }
    }

    /// <summary>
    /// displays enemies floating on screen
    /// </summary>
    public void DisplayMenuBackground()
    {
        //spawn a random amount of asteroids (between 5-10)
        for (int index = 0; index < Random.Range(5, 10); index++)
        {
            //Initialize a new vector3 variable for the random x and y axis position of the asteroid
            Vector3 randomXLoc = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), spawnLoc.position.z);

            //spawn a large asteroid at a random x position
            GameObject enemy = Instantiate(largeAsteroidPrefab, randomXLoc, spawnLoc.rotation);

            //set a random rotation on spawn
            enemy.transform.Rotate(0f, 0f, Random.Range(0f, 270f));

            //add the asteroid to the list of enemies
            enemiesAlive.Add(enemy);
        }

        //spawn in a UFO at a random x and y location
        GameObject UFO = Instantiate(bigUFOPrefab, new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), spawnLoc.position.z), spawnLoc.rotation);

        //add the UFO to the list of enemies
        enemiesAlive.Add(UFO);
    }

    /// <summary>
    /// clears the enemies alive list
    /// </summary>
    public void ClearList()
    {
        //for each of the enemies in the enemies alive list
        foreach (GameObject enemy in enemiesAlive)
        {
            //destroy them
            Destroy(enemy);
        }

        //clear the list after destroying them
        enemiesAlive.Clear();
    }
}
