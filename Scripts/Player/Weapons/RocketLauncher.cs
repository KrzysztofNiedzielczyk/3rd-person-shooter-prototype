using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon, IShootable
{
    private Transform aimPosition;

    private void Update()
    {
        aimPosition = BattleManager.Instance.aimPositionTransform;
    }

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

            //get the gameobject from the pool
            foreach (GameObject projectile in ProjectilePool.Instance.guidedMissilePool)
            {
                if (projectile.activeInHierarchy == false)
                {
                    //set the position, rotation of the gameobject and activate
                    projectile.transform.position = muzzle.position;
                    projectile.transform.rotation = muzzle.rotation;
                    projectile.SetActive(true);
                    projectile.GetComponent<GuidedMissile>().AquireTarget(aimPosition);
                    break;
                }
            }
        }
    }
}
