using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*
    This is a horrible way to check if the player is grounded.
    I aim to fix the problems that character controller has checking isGrounded with a trigger
    instead of using multiple raycasts (that's too complicated for my monkey brain ~( ·-·)~ ).
 */

public class GroundTrigger : MonoBehaviour
{
    public GameObject m_Character;
    public string _tag = "Ground";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tag))
        {
            //m_Character.GetComponent<PlayerController>().SetIsAirborne(true);
            Debug.Log("grounded");
        }
    }
}
