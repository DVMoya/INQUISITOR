using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [HideInInspector] CharacterController m_Controller;
    [HideInInspector] Animator m_Animator;
    public GameObject damageBox;

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
    }
}
