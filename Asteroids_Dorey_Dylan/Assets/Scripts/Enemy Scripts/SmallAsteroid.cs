using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/04/2024]
 * [Determines the size of the small asteroid]
 */

public class SmallAsteroid : Asteroid
{
    void Start()
    {
        //set asteroid size to small
        asteroidSize = AsteroidSize.small;
    }
}
