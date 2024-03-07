using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{

    private void Awake()
    {
        FullHeal();
    }

    public override void DealDamage(Collider col)
    {
        throw new System.NotImplementedException(); // Dummy's cant´deal damage
    }

    public override void Kill()
    {
        FullHeal(); // Dummy recovers after being defeated
    }
}
