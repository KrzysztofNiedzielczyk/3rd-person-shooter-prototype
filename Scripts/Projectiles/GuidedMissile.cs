using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuidedMissile : Projectile
{
    public float rotationSpeed;
    public Transform targetPosition;
    public float activationDelay = 0.5f;
    private bool activated = false;
    private Coroutine initialActivationCoroutine;

    private void OnEnable()
    {
        initialActivationCoroutine = StartCoroutine(InitialActivationDelay());
        rb.velocity = Vector3.zero;
        InitialThrust();
    }

    private void OnDisable()
    {
        activated = false;
        StopCoroutine(initialActivationCoroutine);
    }

    public override void FixedUpdate()
    {
        if (activated)
        {
            Rotate();
            rb.velocity = transform.forward * speed;
        }
    }

    IEnumerator InitialActivationDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        activated = true;
    }

    void Rotate()
    {
        //find the vector pointing from our position to the target
        Vector3 _direction = (targetPosition.transform.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void AquireTarget(Transform target)
    {
        targetPosition = target;
    }

    public void InitialThrust()
    {
        rb.AddForce(BattleManager.Instance.playerComponent.velocity + transform.forward * 5f, ForceMode.VelocityChange);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 7) { return; }

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
}
