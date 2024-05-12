using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : Character
{
    private CharacterController m_Controller;
    private Animator m_Animator;
    public GameObject damageBox;

    [SerializeField] private GameObject waterPlane;
    private Vector3 moveDirection;
    private bool isAttacking    = false;
    private bool isRunning      = false;
    private bool isAirborne     = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator   = GetComponentInChildren<Animator>();
        m_Animator.SetFloat("atSpeed", _damageTick * 1.5f); // 1.5f --> base duration of the animation
        // ^ makes the attack animation play at the same rate as the player deals damage
        // (I have to update this whenever the player upgrades its damage tick rate)

        damageBox.GetComponent<DamageTrigger>().SetTick(_damageTick); // Tells the damage trigger the damage tick rate (this was the simplest way I could think of)
        damageBox.SetActive(false);

        FullHeal();
    }

    // Update is called once per frame
    void Update()
    {
        /*
            MOVEMENT
        */
        moveDirection = new Vector3(0f, moveDirection.y, 0f);
        isAirborne = !m_Controller.isGrounded;

        if (!isAirborne){
            moveDirection = moveDirection + transform.forward * Input.GetAxis("Vertical") * _speedM;

            if (Input.GetKey(KeyCode.Space)){
                moveDirection.y = _jumpF;

                m_Animator.SetBool("isAirborne", true);
            } else {
                m_Animator.SetBool("isAirborne", false);
                moveDirection.y = 0;    // For a smoother fall from the cliff
            }
        } else {
            moveDirection = moveDirection + transform.forward * Input.GetAxis("Vertical") * _speedM * 0.75f; //Reduced movement while airborne
        }

        moveDirection.y = moveDirection.y + Physics.gravity.y * _gravity * Time.deltaTime;

        m_Controller.Move(moveDirection * Time.deltaTime);

        /*
            ROTATIONS
        */
        float rotationInput = Input.GetAxis("Horizontal") * _speedR * Time.deltaTime; // Capturar la entrada de rotación lateral
        transform.Rotate(Vector3.up * rotationInput);

        /*
            ANIMATOR CONTROLLER
        */
        
        // ATTACK
        if (Input.GetKeyDown(KeyCode.K))
        {
            isAttacking = true;
            m_Animator.SetBool("isAttacking", isAttacking);

            // Activar el GameObject de daño
            damageBox.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            isAttacking = false;
            m_Animator.SetBool("isAttacking", isAttacking);

            // Desactivar el GameObject de daño
            damageBox.SetActive(false);
        }

        //Prota corriendo
        isRunning = Input.GetAxis("Horizontal") != 0
                 || Input.GetAxis("Vertical")   != 0;

        m_Animator.SetBool("isRunning", isRunning);
    }

    public override void DealDamage(Collider col)
    {
        col.SendMessage("TakeDamage", _damage);
    }

    public override void Kill(){
        Destroy(this);
    } 
}
