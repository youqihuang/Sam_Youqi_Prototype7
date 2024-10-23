using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;            // Speed of player movement
    public float jumpForce = 7f;        // Jump force
    public float dashSpeed = 10f;       // Speed of dash movement
    public float dashCooldown = 3f;     // Dash cooldown in seconds

    private Rigidbody rb;
    private bool canDash = true;        // To track if dash is allowed
    private Vector3 movement;
    private bool isGrounded = true;     // To check if the player is on the ground

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        movement = new Vector3(horizontalInput * speed, rb.velocity.y, 0);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && horizontalInput < 0 && canDash)
            {
                StartCoroutine(Dash(Vector3.left));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && horizontalInput > 0 && canDash)
            {
                StartCoroutine(Dash(Vector3.right));
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, 0);
    }

    private IEnumerator Dash(Vector3 direction)
    {
        rb.AddForce(direction * dashSpeed, ForceMode.Impulse);
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
