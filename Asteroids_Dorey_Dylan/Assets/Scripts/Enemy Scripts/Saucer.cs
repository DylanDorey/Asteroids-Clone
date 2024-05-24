using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/14/2024]
 * [Base class for the saucer enemies]
 */

public class Saucer : Enemy
{
    //prefab for the projectile that will be shot
    public GameObject projectilePrefab;

    //the location that the projectile will come from
    public Transform projectileSpawner;

    //an index that, when equals a certain number, makes the UFO switch moving directions
    private int randomMoveIndex;

    //bool to start and stop corutine based on if the UFO is alive or not
    public bool isAlive = true;

    public virtual void Start()
    {
        //begin moving randomly
        StartCoroutine(RandomMoveIndex());
    }

    private void FixedUpdate()
    {
        //call move method based on random index
        Move(randomMoveIndex);

        //Check when the saucer game object goes off screen and screen wrap to the opposite side
        GameManager.Instance.ScreenWrap(gameObject);
    }

    public override void OnTriggerEnter(Collider other)
    {
        //if a saucer collides with a projectile
        if (other.gameObject.CompareTag("Projectile"))
        {
            //remove this game object form the enemies alive list
            EnemySpawner.Instance.enemiesAlive.Remove(gameObject);

            //play the explosion effect on death
            OnDeath();

            //destroy the projectile
            Destroy(other.gameObject);

            //set isAlive to false to stop random index coroutine
            isAlive = false;

            //destroy the saucer
            Destroy(gameObject);
        }

        base.OnTriggerEnter(other);
    }

    /// <summary>
    /// Instantiates a projectile from the saucer
    /// </summary>
    public virtual void Shoot()
    {
        //spawn a projectile
        Instantiate(projectilePrefab, projectileSpawner.position, projectileSpawner.rotation);
    }

    /// <summary>
    /// Allows the Saucers to move in a random direction
    /// </summary>
    public virtual void Move(int randomIndex)
    {
        //switch for the randomIndex
        switch (randomIndex)
        {
            //move up if the randomIndex is 0
            case 0:
                transform.Translate(Vector3.left * (enemySpeed * Time.deltaTime));
                break;
            //move right if the randomIndex is 1
            case 1:
                transform.Translate(Vector3.right * (enemySpeed * Time.deltaTime));
                break;
            //move down if the randomIndex is 2
            case 2:
                transform.Translate(Vector3.down * (enemySpeed * Time.deltaTime));
                break;
            //move left if the randomIndex is 3
            case 3:
                transform.Translate(Vector3.up * (enemySpeed * Time.deltaTime));
                break;
        }

    }

    /// <summary>
    /// sets the random move index to a new number every 2 seconds, changing the saucers movement
    /// </summary>
    /// <returns></returns>
    public IEnumerator RandomMoveIndex()
    {
        //while the saucer is alive
        while (isAlive)
        { 
            //set randomMoveIndex to a random value between 0-3
            randomMoveIndex = Random.Range(0, 4);

            //wait 3 seconds
            yield return new WaitForSeconds(3f);
        }
    }
}
