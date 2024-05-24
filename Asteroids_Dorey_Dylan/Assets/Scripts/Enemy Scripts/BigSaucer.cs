using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/14/2024]
 * [Handles shooting for the big saucer]
 */

public class BigSaucer : Saucer
{
    public override void Start()
    {
        //start shooting when spawned in
        StartCoroutine(StartShooting());

        //base start function from Saucer class
        base.Start();
    }

    private void Update()
    {
        //rotate the shooting point for the UFO
        projectileSpawner.Rotate(new Vector3(0f, 0f, -0.2f));
    }

    /// <summary>
    /// shoots a projectile every 2 seconds
    /// </summary>
    /// <returns> time between shots</returns>
    private IEnumerator StartShooting()
    {
        //while the UFO is alive
        while (isAlive)
        {
            //call shoot method from Saucer class
            Shoot();

            //wait 2 seconds before firing another projectile
            yield return new WaitForSeconds(2f);
        }
    }
}
