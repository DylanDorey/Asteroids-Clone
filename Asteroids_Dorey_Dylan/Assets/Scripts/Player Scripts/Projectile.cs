using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [1/31/2024]
 * [The projectile that the player will be able to shoot]
 */

public class Projectile : MonoBehaviour
{
    //float for the speed of the lazer
    public float projectileSpeed;

    //The lifetime of the projectile before it is destroyed
    private float lifetime = 1f;

    void Update()
    {
        //give the projectile a speed value
        transform.position += (transform.up * projectileSpeed);

        //if the projectile goes off screen make it screen wrap to the other side
        GameManager.Instance.ScreenWrap(gameObject);

        //destroy the projectile after a certain amount of time
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the other collider is tagged "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            //add the points to the players score
            PlayerData.Instance.playerScore += other.gameObject.GetComponent<Enemy>().enemyPoints;
        }

        //if the other collider is tagged "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy the projectile
            Destroy(gameObject);
        }
    }
}
