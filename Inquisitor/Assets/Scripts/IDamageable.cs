using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<T>
{
    void Heal(T healStrength);
    void FullHeal();
    void TakeDamage(T damageTaken);
    void DealDamage(Collider colliderHit);
    void Kill();
}