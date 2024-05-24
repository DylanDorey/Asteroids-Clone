using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/04/2024]
 * [Base class for the asteroid enemies that allows them to break and move]
 */

//enum for the various sizes of the asteroids
public enum AsteroidSize
{
    small,
    medium,
    large
}

public class Asteroid : Enemy
{
    //AsteroidSize enum ref
    public AsteroidSize asteroidSize;

    //medium and small asteroid prefab
    public GameObject mediumAsteroid;
    public GameObject smallAsteroid;

    //the speed that the asteroid will rotate
    public float rotateSpeed = 0.08f;

    //bool to prevent multiple asteroids from spawning
    private bool hasBroken;

    void Update()
    {
        //if the asteroid goes off screen make it screen wrap to the opposite side
        GameManager.Instance.ScreenWrap(gameObject);
    }

    private void FixedUpdate()
    {
        //rotate and move in a constant direction
        Rotate();
        Move();
    }

    public override void OnTriggerEnter(Collider other)
    {
        //if an asteroid collides with a projectile
        if (other.gameObject.CompareTag("Projectile"))
        {
            //play the explosion effect on death
            OnDeath();

            //destroy the larger asteroid
            Destroy(other.gameObject);

            //if the larger asteroid has not broken already
            if (!hasBroken)
            {
                //call break method to spawn in two more smaller asteroids(unless its a small asteroid)
                Break();
            }

            //set has broken back to false
            hasBroken = false;
        }

        base.OnTriggerEnter(other);
    }

    /// <summary>
    /// Allows the asteroid to move forward at a constant speed
    /// </summary>
    public void Move()
    {
        //Make the asteroid move in a continuous forward direction at a particular speed
        transform.Translate(transform.up * enemySpeed);
    }

    /// <summary>
    /// Allows the asteroid to break into the appropriate size after being hit by a projectile
    /// </summary>
    public void Break()
    {
        //switch on the asteroidSize enum
        switch (asteroidSize)
        {
            //if the asteroid is small
            case AsteroidSize.small:
                //remove the small asteroid from the enemiesAlive list and destroy it
                EnemySpawner.Instance.enemiesAlive.Remove(gameObject);
                Destroy(gameObject);
                break;

            //if the asteroid is medium
            case AsteroidSize.medium:
                hasBroken = true;

                //instatiate two more small asteroids at random x and y positions close to the parent medium asteroid, and set a random rotation for each small asteroid
                GameObject sAsteroid1 = Instantiate(smallAsteroid, new Vector3(transform.position.x +- Random.Range(0f, 2f), transform.position.y +- Random.Range(0f, 2f), transform.position.z), transform.rotation);
                GameObject sAsteroid2 = Instantiate(smallAsteroid, new Vector3(transform.position.x +- Random.Range(0f, 2f), transform.position.y +- Random.Range(0f, 2f), transform.position.z), transform.rotation);
                sAsteroid1.transform.Rotate(0f, 0f, Random.Range(0f, 270f));
                sAsteroid2.transform.Rotate(0f, 0f, Random.Range(0f, 270f));

                //add both small asteroids to the enemies alive list, and remove the medium asteroid from said list
                EnemySpawner.Instance.enemiesAlive.Add(sAsteroid1);
                EnemySpawner.Instance.enemiesAlive.Add(sAsteroid2);
                EnemySpawner.Instance.enemiesAlive.Remove(gameObject);

                //destroy the medium asteroid
                Destroy(gameObject);
                break;

            //if the asteroid is large
            case AsteroidSize.large:
                hasBroken = true;

                //instatiate two more medium asteroids at random x and y positions close to the parent large asteroid, and set a random rotation for each medium asteroid
                GameObject mAsteroid1 = Instantiate(mediumAsteroid, new Vector3(transform.position.x +- Random.Range(0f, 2f), transform.position.y +- Random.Range(0f, 2f), transform.position.z), transform.rotation);
                GameObject mAsteroid2 = Instantiate(mediumAsteroid, new Vector3(transform.position.x +- Random.Range(0f, 2f), transform.position.y +- Random.Range(0f, 2f), transform.position.z), transform.rotation);
                mAsteroid1.transform.Rotate(0f, 0f, Random.Range(0f, 270f));
                mAsteroid2.transform.Rotate(0f, 0f, Random.Range(0f, 270f));

                //add both medium asteroids to the enemies alive list, and remove the medium asteroid from said list
                EnemySpawner.Instance.enemiesAlive.Add(mAsteroid1);
                EnemySpawner.Instance.enemiesAlive.Add(mAsteroid2);
                EnemySpawner.Instance.enemiesAlive.Remove(gameObject);

                //destroy the large asteroid
                Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// Makes the asteroid rotate indefinetely
    /// </summary>
    public void Rotate()
    {
        //rotate on the z axis
        transform.Rotate(new Vector3(0f, 0f, rotateSpeed));
    }
}
