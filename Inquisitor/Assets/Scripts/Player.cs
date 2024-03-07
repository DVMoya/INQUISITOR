using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Character
{
    private CharacterController m_Controller;
    private Animator m_Animator;
    public GameObject m_Player;
    //public GameObject dust;
    public GameObject damageBox;

    private Vector3 moveDirection;
    private Vector3 rotation;
    private bool isAttacking = false;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator   = GetComponentInChildren<Animator>();

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

        if(m_Controller.isGrounded){
            moveDirection = moveDirection + transform.forward * Input.GetAxis("Vertical") * _speedM;

            if (Input.GetButtonDown("Jump")){
                moveDirection.y = _jumpF;
            } else {
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
        isRunning = Input.GetKey(KeyCode.W) ||
                    Input.GetKey(KeyCode.A) ||
                    Input.GetKey(KeyCode.S) ||
                    Input.GetKey(KeyCode.D);

        m_Animator.SetBool("isRunning", isRunning);

        //if(isRunning && m_Controller.isGrounded) dust.SetActive(true);
        //else dust.SetActive(false);





        /*
            TESTING SCIPTS
        */
        if(Input.GetKeyDown(KeyCode.Alpha1)) TakeDamage(2f);
        if(Input.GetKeyDown(KeyCode.Alpha2)) Heal(2f);
        if(Input.GetKeyDown(KeyCode.Alpha3)) FullHeal();

    }

    public override void DealDamage(Collider col)
    {
        col.SendMessage("TakeDamage", _damage);
    }

    public override void Kill(){
        Destroy(m_Player);
    } 
}
