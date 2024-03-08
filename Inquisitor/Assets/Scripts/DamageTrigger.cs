using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public GameObject m_DamageDealer;
    public string _tag;

    private float _tick;

    public void SetTick(float tick) {  _tick = tick; }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WaitForTick(other));
    }

    void OnTriggerExit(Collider other)
    {
        StopCoroutine(WaitForTick(other));   
    }

    IEnumerator WaitForTick(Collider other)
    {
        if (other.CompareTag(_tag)) m_DamageDealer.SendMessage("DealDamage", other);
        yield return new WaitForSeconds(1/_tick); //I want to deal damage _tick times per second

        StartCoroutine(WaitForTick(other));
    }
}
