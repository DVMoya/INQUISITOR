using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IDamageable<float>
{
    private CharacterController m_Controller;
    private Animator m_Animator;
    public GameObject m_Player;
    public GameObject dust;
    public GameObject damageBox;

    public float maxHealth = 10f;
    public float currentHealth;
    public float damage = 1f;
    public float moveSpeed;
    [SerializeField, Range(0f, 200f)] float rotaSpeed;
    public float jumpForce;
    public float gravityScale;

    private Vector3 moveDirection;
    private Vector3 rotation;
    private bool isAttacking = false;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator   = GetComponentInChildren<Animator>();

        damageBox.SetActive(false);

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        /*
            MOVEMENT
        */
        moveDirection = new Vector3(0f, moveDirection.y, 0f);

        if(m_Controller.isGrounded){
            moveDirection = moveDirection + transform.forward * Input.GetAxis("Vertical") * moveSpeed;

            if (Input.GetButtonDown("Jump")){
                moveDirection.y = jumpForce;
            } else {
                moveDirection.y = 0;    // For a smoother fall from the cliff
            }
        } else {
            moveDirection = moveDirection + transform.forward * Input.GetAxis("Vertical") * moveSpeed * 0.75f; //Reduced movement while airborne
        }

        moveDirection.y = moveDirection.y + Physics.gravity.y * gravityScale * Time.deltaTime;

        m_Controller.Move(moveDirection * Time.deltaTime);

        /*
            ROTATIONS
        */
        float rotationInput = Input.GetAxis("Horizontal") * rotaSpeed * Time.deltaTime; // Capturar la entrada de rotación lateral
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

        if(isRunning && m_Controller.isGrounded) dust.SetActive(true);
        else dust.SetActive(false);





        /*
            TESTING SCIPTS
        */
        if(Input.GetKeyDown(KeyCode.Alpha1)) TakeDamage(2f);
        if(Input.GetKeyDown(KeyCode.Alpha2)) Heal(2f);
        if(Input.GetKeyDown(KeyCode.Alpha3)) FullHeal();

    }


    /*
        IDamageable INTERFACE

        Controls all objects that can be dealt damage
    */

    public void Heal(float healStrength){
        currentHealth += healStrength;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void FullHeal(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken){
        currentHealth -= damageTaken;
        if(currentHealth <= 0f){
            currentHealth = 0f;
            Kill();
        }
    }

    public void Kill(){
        Destroy(m_Player);
    }
}
