using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public float maxEnergyShield;
    public float energyShiled;
    public Transform playerTarget;
    public Rigidbody rb;
    public Vector3 velocity;

    [SerializeField] private GameObject deathParticle;

    private Vector3 lastPos;

    private void OnEnable()
    {
        health = maxHealth;
        energyShiled = maxEnergyShield;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        energyShiled = maxEnergyShield;
    }

    private void FixedUpdate()
    {
        // manual calculation of velocity
        velocity = CalculateVelocity();
    }


    // manual calculation of velocity
    public Vector3 CalculateVelocity()
    {
        var trackVelocity = (rb.position - lastPos) / Time.fixedDeltaTime;
        lastPos = rb.position;

        return trackVelocity;
    }

    public void TakeDamage(float amount)
    {
        if(energyShiled > 0)
        {
            energyShiled -= amount;

            if(energyShiled < 0)
            {
                energyShiled = 0;
            }
        }
        else
        {
            health -= amount;
            if (health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
