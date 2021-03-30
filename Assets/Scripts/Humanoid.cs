using System.Collections;
using UnityEngine;

public abstract class Humanoid : LiveBeing
{
    private void OnCollisionEnter(Collision collision)
    {
        EnvironmentDanger danger = collision.gameObject.GetComponent<EnvironmentDanger>();
        if (danger != null)
            TakeDamage(danger.DamageToDeal);

        CollidedWithObject(collision);
    }

    public virtual void CollidedWithObject(Collision collision)
    {
    }

    public abstract void PlayIdle();

    public abstract void PlayMove();

    protected IEnumerator PlayAnimationIdleToRun()
    {
        float speedIncrease = .1f;
        float currentMoveSpeed = 0;
        float secondsStep = .03f;

        while (currentMoveSpeed < 1)
        {
            Animator.SetFloat("MoveSpeed", currentMoveSpeed);
            currentMoveSpeed += speedIncrease;
            yield return new WaitForSeconds(secondsStep);
        }
    }
}