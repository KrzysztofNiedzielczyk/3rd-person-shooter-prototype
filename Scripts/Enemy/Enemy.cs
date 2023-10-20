using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public float speed;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Dying();
        }
    }

    public virtual void Dying()
    {
        Destroy(gameObject);
    }
}
