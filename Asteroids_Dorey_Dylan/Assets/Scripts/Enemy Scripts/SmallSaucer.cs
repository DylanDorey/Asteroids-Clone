using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [02/14/2024]
 * [Handles shooting for the small saucer]
 */

public class SmallSaucer : Saucer
{
    public int playersStartingScore;

    public int newScore;

    private float firingAngle;

    private float rotationModifier = 90f;

    private float turnSpeed = 10f;

    private int angleAdjustmentIndex = 3;

    public Transform playerTransform;

    public Transform target;

    public override void Start()
    {
        //set the starting score of the player when the small UFO first spawns in
        playersStartingScore = PlayerData.Instance.playerScore;

        //start shooting when spawned in
        StartCoroutine(StartShooting());

        //base start function from Saucer class
        base.Start();
    }

    private void Update()
    {
        FollowPlayer();
        AdjustFiringAngle();

        //if the player is not dead
        if (PlayerController.Instance.isDead == false)
        {
            //set the transform of the player equal to the player gameobject transform
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        //update the target position to the players transform plus angle offset
        target.position = new Vector3(playerTransform.position.x + angleAdjustmentIndex, playerTransform.position.y + angleAdjustmentIndex, playerTransform.position.z);
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

    /// <summary>
    /// adjusts the target that the small UFO fires/aims at based upon the players score
    /// </summary>
    private void AdjustFiringAngle()
    {
        //if the score of the player is greater than 100 of the initialized score
        if (playersStartingScore + 100 < PlayerData.Instance.playerScore)
        {
            //set the new intitialized score
            playersStartingScore = PlayerData.Instance.playerScore;

            //if the adjustment angle is not already maxed out
            if (angleAdjustmentIndex > -1)
            {
                //bring the angle in tighter to the player
                angleAdjustmentIndex--;
            }
        }
    }

    /// <summary>
    /// makes the projectile spawner point towards the player whereever it is at on the level
    /// </summary>
    private void FollowPlayer()
    {
        //subtract the projectile spawners position from the players position
        Vector3 lookDirection = target.position - projectileSpawner.position;

        //set angle equal to the angle tangent of lookDirection.y and .x then convert that radian to degrees
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - rotationModifier;

        //store the rotation value to angle at the player
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //lerp the rotation of the projectile spawner to the lookDirection
        projectileSpawner.rotation = Quaternion.Slerp(projectileSpawner.rotation, rotation, Time.deltaTime * turnSpeed);
    }
}
