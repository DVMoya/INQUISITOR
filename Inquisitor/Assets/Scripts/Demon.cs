using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Character
{
    public float hitScale = 1.1f;
    public float hitSpeed = 0.5f;
    public int   hitVibra = 2;
    public float hitElast = 1f;

    private void Awake()
    {
        FullHeal();
    }

    public override void TakeDamage(float damageTaken)
    {
        transform.DOPunchScale(Vector3.one * hitScale, hitSpeed, hitVibra, hitElast);
        StartCoroutine(WaitForAnimation(hitSpeed));
        
        base.TakeDamage(damageTaken);       
    }

    IEnumerator WaitForAnimation(float wait)
    {
        //It doesn't matter that the pushed animation plays at a different rate than it receives damage
        yield return new WaitForSeconds(wait);
    }

    public override void DealDamage(Collider col)
    {
        throw new System.NotImplementedException(); // Dummy's cant´deal damage
    }

    public override void Kill()
    {
        Destroy(gameObject);
        Component lvl = GetComponentInParent<Level>();
        lvl.SendMessage("EnemyDead");
    }
}
