using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [1/29/2024]
 * [This will allow the user to rotate, thrust, shoot, and enter hyperspace]
 */

public class PlayerController : MonoBehaviour
{
    //singelton for PlayerController
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }

    //reference to scriptable object PlayerInput
    public PlayerInput playerActions;

    //the prefab for the projectile the player will shoot and the projectile spawner
    public GameObject projectilePrefab;
    public Transform projectileSpawner;

    //explosion prefab
    public GameObject explosionPrefab;

    //thruster gameobject prefab
    public GameObject thrusterPrefab;

    //value for how fast the player will rotate
    private float rotateSpeed = 150f;

    //speed value for how fast the player can thrust
    private float thrustSpeed = 150f;

    //bool to prevent multiple thrusts at once
    private bool hasThrusted = false;

    //bool to switch if the player dies
    public bool isDead = false;

    //player starting position
    private Vector3 startPos = new Vector3(0f, 0f, 0f);

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

        //on startup, disable the player object
        gameObject.SetActive(false);
    }

    void Update()
    {
        //Check when the player controller game object goes off screen and screen wrap to the opposite side
        GameManager.Instance.ScreenWrap(gameObject);

        //if the game state is set to menu or game over
        if (GameManager.Instance.gameState == GameState.menu || GameManager.Instance.gameState == GameState.gameOver)
        {
            //disable the player object
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        //if the game state is set to playing
        if (GameManager.Instance.gameState == GameState.playing)
        {
            //reads the Vector2 value from the playerActions components and from the rotate action (AD) in our actions scriptable object
            Vector2 moveVec = playerActions.Ship.Rotate.ReadValue<Vector2>();
            transform.Rotate(new Vector3(0f, 0f, moveVec.x) * -rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// allows the player to rotate when an input action is detected from the action map
    /// </summary>
    public void OnRotate(InputAction.CallbackContext context)
    {
        //On rotate is only going to fire when the event is called. It doesn't continualy get called when held
        Vector2 moveVec = context.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0f, 0f, moveVec.x) * -rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// allows the player to thrust forward when an input action is detected from the action map
    /// </summary>
    public void OnThrust(InputAction.CallbackContext context)
    {
        //If the player has not thrusted already, On thrust is only going to fire when the event is called. It doesn't continualy get called when held
        if (!hasThrusted)
        {
            if (context.performed)
            {
                //start the thrust coroutine that lasts 2 seconds
                StartCoroutine(Thrust());
            }
        }
    }

    /// <summary>
    /// allows the player to enter hyperspace mode when an input action is detected from the action map
    /// </summary>
    public void OnHyperspace(InputAction.CallbackContext context)
    {
        //On hyperspace is only going to fire when the event is called. It doesn't continualy get called when held
        if (context.performed)
        {
            StartCoroutine(EnterHyperSpace());
        }
    }

    /// <summary>
    /// allows the player to shoot projectiles when an input action is detected from the action map
    /// </summary>
    public void OnShoot(InputAction.CallbackContext context)
    {
        //if the spacebar action was performed
        if (context.performed)
        {
            //fire projectiles
            Shoot();
        }
    }

    /// <summary>
    /// allows the player to fire a projectile
    /// </summary>
    private void Shoot()
    {
        //spawn a projectile from the players projectile spawner
        Instantiate(projectilePrefab, projectileSpawner.position, projectileSpawner.rotation);
    }

    /// <summary>
    /// allows the player to thrust in any direction for a given time
    /// </summary>
    private IEnumerator Thrust()
    {
        for (int index = 0; index < 1; index++)
        {
            //the player has thrusted so disable thrusting for 2 seconds and apply force to rigid body, and display thruster on ship
            hasThrusted = true;
            GetComponent<Rigidbody>().AddForce(transform.up * thrustSpeed);
            thrusterPrefab.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);
        }

        //enable thrusting again and remove the thruster on ship
        hasThrusted = false;
        thrusterPrefab.gameObject.SetActive(false);
    }

    /// <summary>
    /// makes the player disappear and reappear in a new location after a certain duration
    /// </summary>
    private IEnumerator EnterHyperSpace()
    {
        //a list that will contain all of the parts that make up the players ship
        List<GameObject> shipParts = new List<GameObject>();

        //loop through the player game objects children
        for (int index = 0; index < gameObject.transform.childCount; index++)
        {
            //if the child's name is "Cube"
            if(gameObject.transform.GetChild(index).gameObject.name == "Cube")
            {
                //add that game object to the list of ship parts
                shipParts.Add(gameObject.transform.GetChild(index).gameObject);
            }
        }

        //disappear for 1 second
        for (int index2 = 0; index2 < 1; index2++)
        {
            //disable the box collider once the player disappears
            GetComponent<BoxCollider>().enabled = false;

            //for each game object in the shipParts list, disable the mesh renderer
            foreach (GameObject shipPart in shipParts)
            {
                shipPart.GetComponent<MeshRenderer>().enabled = false;
            }

            //wait 1 second
            yield return new WaitForSeconds(1f);
        }

        //move the player to a new location
        transform.position = Relocate(transform.position);

        //for each game object in the shipParts list, enable the mesh renderer
        foreach (GameObject shipPart in shipParts)
        {
            shipPart.GetComponent<MeshRenderer>().enabled = true;
        }

        //enable the box collider when the player reappears
        GetComponent<BoxCollider>().enabled = true;
    }

    /// <summary>
    /// sets the players position to a new x and y axis position on the level
    /// </summary>
    /// <param name="newLoc"> the new, random location the player will move to </param>
    /// <returns> the new position </returns>
    private Vector3 Relocate(Vector3 newLoc)
    {
        newLoc = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0f);

        return newLoc;
    }

    public void InitializePlayerController()
    {
        //activate the player on screen
        gameObject.SetActive(true);

        //reference for the PlayerInput scriptable object
        playerActions = new PlayerInput(); //constructor

        //turn playerActions on
        playerActions.Enable();

        //set the rotate speed
        rotateSpeed = 150f;

        //set speed value for how fast the player can thrust
        thrustSpeed = 150f;

        //initialize the players starting position
        startPos = new Vector3(0f, 0f, 0f);

        //diable the thruster effect
        thrusterPrefab.gameObject.SetActive(false);

        //set isDead to false
        isDead = false;

        //set hasThrusted to false
        hasThrusted = false;

        //make player invincible at game start
        StartCoroutine(Invincibility());
    }

    /// <summary>
    /// This will reset all player controller values back to default on restart
    /// </summary>
    public void ResetPlayerController()
    {
        //set player back to the center of the screen
        transform.position = startPos;

        //remove all motion from prior life
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
    }

    /// <summary>
    /// Makes the player exploded and removes a life when they die
    /// </summary>
    public void OnDeath()
    {
        //play explosion particle effect
        Explode();

        //remove a life
        PlayerData.Instance.playerLives--;

        //reset the player controller back to the center of the screen/startPos
        ResetPlayerController();

        //if the player is on their last life
        if (PlayerData.Instance.playerLives > 0)
        {
            //make the player invincible for a short duration
            StartCoroutine(Invincibility());
        }
    }

    /// <summary>
    /// Spawn explosion particles on player death
    /// </summary>
    private void Explode()
    {
        //spawn explosion particle system on position
        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    /// <summary>
    /// makes the player invincible for a short duration
    /// </summary>
    /// <returns> invincibility duration </returns>
    private IEnumerator Invincibility()
    {
        for (int index = 0; index < 1; index++)
        {
            //disable the box collider on the player for 2 seconds
            GetComponent<BoxCollider>().enabled = false;

            yield return new WaitForSeconds(3f);
        }

        //enable the box collider on the player
        GetComponent<BoxCollider>().enabled = true;
    }
}
