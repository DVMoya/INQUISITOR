using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public GameObject m_DamageDealer;
    public string _tag;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == _tag) m_DamageDealer.SendMessage("DealDamage", other);
    }
}
