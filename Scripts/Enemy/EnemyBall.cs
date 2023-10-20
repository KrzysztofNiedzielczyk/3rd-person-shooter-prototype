using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : Enemy
{
    [SerializeField] private Transform gun;
    [SerializeField] private Transform gunSocket;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float gunRotationSpeed;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private EnemyProjectile projectile;
    private float nextTimeToFire = 0f;
    private BattleManager battleManager;

    private void Awake()
    {
        battleManager = BattleManager.Instance;
    }

    private void Update()
    {
        if (battleManager.player != null)
        {
            Quaternion toRotation = Quaternion.LookRotation(battleManager.playerComponent.playerTarget.position - gunSocket.position, Vector3.up);
            gunSocket.rotation = Quaternion.RotateTowards(gunSocket.rotation, toRotation, gunRotationSpeed * Time.deltaTime);
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            //get the gameobject from the pool
            foreach (GameObject projectile in ProjectilePool.Instance.redProjectilePool)
            {
                if (projectile.activeInHierarchy == false)
                {
                    //set the position, rotation of the gameobject and activate
                    projectile.transform.position = muzzle.position;
                    projectile.transform.rotation = muzzle.rotation * Quaternion.Euler(90, 0, 0);
                    projectile.SetActive(true);
                    break;
                }
            }
        }
    }
}
