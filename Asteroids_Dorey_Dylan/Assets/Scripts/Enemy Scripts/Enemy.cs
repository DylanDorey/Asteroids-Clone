using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [2/04/2024]
 * [Base class for all enemies that stores point values, speed, death, and explosion particle system]
 */

public class Enemy : MonoBehaviour
{
    //explosion particle system prefab
    public GameObject explosion;

    //int value for the amount of points an enemy will be worth
    public int enemyPoints;

    //the speed that the enemy will move at
    public float enemySpeed;

    public virtual void OnTriggerEnter(Collider other)
    {
        //if an enemy collides with the player call the player OnDeath function to remove a life
        if (other.gameObject.CompareTag("Player"))
        {
            //call player's onDeath method
            PlayerController.Instance.OnDeath();
        }
    }

    /// <summary>
    /// plays the explosion particle system on enemy death
    /// </summary>
    public virtual void OnDeath()
    {
        //call explode method
        Explode();
    }

    /// <summary>
    /// Spawn explosion particles on enemy death
    /// </summary>
    private void Explode()
    {
        //spawn explosion particle system on position
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
