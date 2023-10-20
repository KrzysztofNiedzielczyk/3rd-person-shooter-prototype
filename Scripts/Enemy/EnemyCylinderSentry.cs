using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCylinderSentry : Enemy
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
            Quaternion toRotation = Quaternion.LookRotation(PredictedPosition(battleManager.playerComponent.playerTarget.position, muzzle.position, battleManager.playerComponent.velocity, projectile.speed) - gunSocket.position, Vector3.up);
            gunSocket.rotation = Quaternion.RotateTowards(gunSocket.rotation, toRotation, gunRotationSpeed * Time.deltaTime);
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            //get the gameobject from the pool
            foreach (GameObject projectile in ProjectilePool.Instance.redBigProjectilePool)
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

    private Vector3 PredictedPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - shooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            Debug.Log("Position prediction is not feasible.");
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }
}
