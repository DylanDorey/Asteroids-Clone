using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/04/2024]
 * [Determines the size of the large asteroid]
 */

public class LargeAsteroid : Asteroid
{
    void Start()
    {
        //set asteroid size to large
        asteroidSize = AsteroidSize.large;
    }
}
