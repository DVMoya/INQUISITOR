using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del jugador
    public float rotationSpeed = 100f; // Velocidad de rotación del jugador
    public float jumpForce = 10f; // Fuerza del salto
    public GameObject damageBox;

    public Rigidbody m_Rigidbody;
    private bool isGrounded; // Variable para controlar si el jugador está en el suelo

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        damageBox.SetActive(false);
    }

    void Update()
    {
        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Activar el GameObject de daño
            damageBox.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            // Desactivar el GameObject de daño
            damageBox.SetActive(false);
        }
    }

    
    void FixedUpdate()
    {
        /*
            He colocado el movimiento en FixedUpdate porque si no no era fluido.
            Salto y Ataque siguen en update para responder lo más rápido posible a los inputs
        */

        // Movimiento horizontal y vertical
        float moveForward = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(0.0f, 0.0f, moveForward);
        transform.Translate(movement * speed * Time.deltaTime);

        // Rotación
        float rotationInput = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; // Capturar la entrada de rotación lateral
        transform.Rotate(Vector3.up * rotationInput);
    }

    // Verificar si el jugador está en el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Verificar si el jugador deja de tocar el suelo
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
