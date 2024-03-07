using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    public float _animSpeed;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.speed = _animSpeed;

        FullHeal();
    }

    public override void TakeDamage(float damageTaken)
    {
        m_Animator.SetBool("isHit", true);
        StartCoroutine(WaitForAnimation(_animSpeed));

        base.TakeDamage(damageTaken);
    }

    IEnumerator WaitForAnimation(float wait)
    {
        yield return new WaitForSeconds(wait);
        m_Animator.SetBool("isHit", false);
    }

    public override void DealDamage(Collider col)
    {
        throw new System.NotImplementedException(); // Dummy's cant´deal damage
    }

    public override void Kill()
    {
        m_Animator.SetBool("isDead", true);
    }
}
