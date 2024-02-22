using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del jugador
    public float rotationSpeed = 100f; // Velocidad de rotación del jugador
    public float jumpForce = 10f; // Fuerza del salto
    private bool isGrounded; // Variable para controlar si el jugador está en el suelo

    void Update()
    {
        // Movimiento horizontal y vertical
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);

        // Rotación
        float rotationInput = Input.GetAxis("HorizontalRotation"); // Capturar la entrada de rotación lateral
        transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
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
