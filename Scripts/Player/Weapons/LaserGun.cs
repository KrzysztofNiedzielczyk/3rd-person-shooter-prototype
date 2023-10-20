using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon, IShootable
{
    public float maxPositionDeviation = 0.01f;
    public float maxRotationDeviation = 0.5f;

    public void Fire()
    {
        //if statement for rate of fire implementation
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            //play shoot audio
            shootAudio.Play();
            //play muzzle flash
            muzzleFlash.Play();

            // randomize position and angles for spread of fire
            float randomPosX = Random.Range(-maxPositionDeviation, maxPositionDeviation);
            float randomPosZ = Random.Range(-maxPositionDeviation, maxPositionDeviation);
            float randomRotX = Random.Range(-maxRotationDeviation, maxRotationDeviation);
            float randomRotY = Random.Range(-maxRotationDeviation, maxRotationDeviation);
            float randomRotZ = Random.Range(-maxRotationDeviation, maxRotationDeviation);

            //get the gameobject from the pool
            foreach (GameObject projectile in ProjectilePool.Instance.blueProjectilePool)
            {
                if (projectile.activeInHierarchy == false)
                {
                    //set the position, rotation of the gameobject and activate
                    projectile.transform.position = muzzle.position + new Vector3(randomPosX, 0, randomPosZ);
                    projectile.transform.rotation = muzzle.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(randomRotX, randomRotZ, randomRotY);
                    projectile.SetActive(true);
                    break;
                }
            }
        }
    }
}
