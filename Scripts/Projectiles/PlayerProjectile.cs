using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 7) { return; }

        if (collision.gameObject.TryGetComponent(out IDamageable hit))
        {
            hit.TakeDamage(damage);

            StopCoroutine(releaseCoroutine);

            //leave hit impact effect
            LeaveImpactEffect(collision.contacts[0]);

            gameObject.SetActive(false);
        }
        else
        {
            StopCoroutine(releaseCoroutine);

            //leave hit impact effect
            LeaveImpactEffect(collision.contacts[0]);

            gameObject.SetActive(false);
        }
    }
}
