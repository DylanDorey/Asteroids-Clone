using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/04/2024]
 * [Determines the size of the medium asteroid]
 */

public class MediumAsteroid : Asteroid
{
    void Start()
    {
        //set asteroid size to medium
        asteroidSize = AsteroidSize.medium;
    }
}
