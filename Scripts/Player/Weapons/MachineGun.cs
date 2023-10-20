using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon, IShootable
{
    public ParticleSystem muzzleFlash2;
    public GameObject impactEffect;
    public float range = 100f;
    public float maxSpread = 2f;
    public float damage = 10f;

    public void Fire()
    {
        //if statement for rate of fire implementation
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            //play shoot audio
            shootAudio.Play();
            //play muzzle flash
            if (muzzleFlash.isPlaying)
            {
                muzzleFlash2.Play();
            }
            else
            {
                muzzleFlash.Play();
            }

            Vector3 deviation3D = Random.insideUnitCircle * maxSpread;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * range + deviation3D);
            Vector3 forwardVector = muzzle.transform.rotation * rot * Vector3.forward;

            RaycastHit hit;
            if (Physics.Raycast(muzzle.transform.position, forwardVector, out hit, range))
            {
                if (hit.collider != null)
                {
                    GameObject impactGamObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGamObj, 2f);

                    if (hit.collider.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
