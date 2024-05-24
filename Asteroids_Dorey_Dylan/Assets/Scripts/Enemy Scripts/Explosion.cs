using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/04/2024]
 * [Plays the explosion particle system]
 */

public class Explode : MonoBehaviour
{
    private void Start()
    {
        //play the explosion particle system
        GetComponent<ParticleSystem>().Play();
    }
}
