using System;
using UnityEngine;

public abstract class LiveBeing : MonoBehaviour
{
    public event EventHandler<LiveBeing> Died;

    public int Health = 10;
    public Animator Animator;

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Died(null, this);
    }
}