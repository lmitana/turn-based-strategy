using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] int health = 100;
    int healthMax;

    void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        if (health == 0)
        {
            Die();
        }        
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
