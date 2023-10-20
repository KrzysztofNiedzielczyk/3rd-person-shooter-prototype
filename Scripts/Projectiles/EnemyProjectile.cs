using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7) { return; }

        if (collision.gameObject.tag != "Enemy" && collision.gameObject.TryGetComponent(out IDamageable hit))
        {
            hit.TakeDamage(damage);

            StopCoroutine(releaseCoroutine);

            gameObject.SetActive(false);
        }
        else
        {
            StopCoroutine(releaseCoroutine);

            gameObject.SetActive(false);
        }
    }
}
