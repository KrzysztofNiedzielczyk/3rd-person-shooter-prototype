using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    [SerializeField] private float releaseTime;
    public Rigidbody rb;
    public GameObject impactEffect;

    public Coroutine releaseCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        releaseCoroutine = StartCoroutine(ReleaseAfterTime());
    }

    private void OnDisable()
    {
        if (releaseCoroutine != null)
        {
            StopCoroutine(releaseCoroutine);
        }
    }

    public virtual void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    IEnumerator ReleaseAfterTime()
    {
        yield return new WaitForSeconds(releaseTime);

        gameObject.SetActive(false);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable hit))
        {
            hit.TakeDamage(damage);

            if (releaseCoroutine != null)
            {
                StopCoroutine(releaseCoroutine);
            }

            //leave hit impact effect
            LeaveImpactEffect(collision.contacts[0]);

            gameObject.SetActive(false);
        }
        else
        {
            if(releaseCoroutine != null)
            {
                StopCoroutine(releaseCoroutine);
            }

            //leave hit impact effect
            LeaveImpactEffect(collision.contacts[0]);

            gameObject.SetActive(false);
        }
    }

    public void LeaveImpactEffect(ContactPoint hit)
    {
        if (impactEffect != null)
        {
            GameObject impactGamObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGamObj, 2f);
        }
    }
}
