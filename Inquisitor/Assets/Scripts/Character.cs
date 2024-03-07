using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable<float>
{
    public float _healthM;
    public float _healthC;
    public float _damage;
    public float _damageTick;
    public float _speedM;
    public float _speedR;
    public float _jumpF;
    public float _gravity;

    public float HealthM    { get { return _healthM; } }
    public float HealthC    { get { return _healthC; }      set { _healthC = value; } }
    public float Damage     { get { return _damage; }       set { _damage = value; } }
    public float DamageTick { get { return _damageTick; }   set { _damageTick = value; } }
    public float SpeedM     { get { return _speedM; }       set { _speedM = value; } }
    public float SpeedR     { get { return _speedR; }       set { _speedR = value; } }
    public float JumpF      { get { return _jumpF; }        set { _jumpF = value; } }
    public float Gravity    { get { return _gravity; }      set { _gravity = value; } }

    /*
        IDamageable INTERFACE

        Controls all objects that can be dealt damage
    */

    public void Heal(float healStrength)
    {
        _healthC += healStrength;
        if (_healthC > _healthM) _healthC = _healthM;
    }

    public void FullHeal()
    {
        _healthC = _healthM;
    }

    public virtual void TakeDamage(float damageTaken)
    {
        _healthC -= damageTaken;
        if (_healthC <= 0f)
        {
            _healthC = 0f;
            Kill();
        }
    }

    public abstract void DealDamage(Collider col);

    public abstract void Kill();
}
