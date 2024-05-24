using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [2/14/2024]
 * [The projectile that the enemy saucers will be able to shoot]
 */

public class EnemyProjectile : MonoBehaviour
{
    //float for the speed of the lazer
    public float projectileSpeed;

    //the lifetime of the projectile before it is destroyed
    private float lifetime = 1f;

    void Update()
    {
        //give the projectile a speed value
        transform.position += (transform.up * projectileSpeed);

        //allows the enemy projectile to screen wrap
        GameManager.Instance.ScreenWrap(gameObject);

        //destroy the projectile after a certain amount of time
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collider)
    {
        //if the other collider is tagged "Player"
        if (collider.gameObject.CompareTag("Player"))
        {
            //call the players OnDeath method when hit by the projectile
            PlayerController.Instance.OnDeath();

            //destroy the projectile
            Destroy(gameObject);
        }
    }
}
