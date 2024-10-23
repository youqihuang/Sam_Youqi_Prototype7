using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
        public float moveSpeed = 100f;         // Player's movement speed
    public float jumpForce = 5f;           // Force applied when the player jumps
    public bool isGrounded;                // To check if the player is grounded
    public Transform groundCheck;          // Transform of the object to check if grounded
    public float groundCheckRadius = 0.4f; // Radius of the ground check
    public LayerMask groundLayer;          // Layer to check against (ground layer)

    public GameObject boundingBox;         // Reference to the bounding box object

    private Rigidbody rb;
    private Renderer boundingBoxRenderer;  // Renderer of the bounding box

    void Start()
    {
        rb = GetComponent<Rigidbody>();    // Get the Rigidbody component of the player
        boundingBoxRenderer = boundingBox.GetComponent<Renderer>(); // Get the Renderer of the bounding box
    }

    void Update()
    {
        Move();
        Jump();
        ClampPositionWithinBounds();       // Ensure player stays within bounds
    }

    void Move()
    {
        // Get horizontal input (A/D or Left/Right arrow keys)
        float moveInput = Input.GetAxis("Horizontal");

        // Set player's velocity for horizontal movement (moving along X-axis in 3D space)
        Vector3 move = transform.right * moveInput * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, 0);  // Movement along X axis, keep Y velocity for jumping
    }

    void Jump()
    {
        // Check if the player is grounded (using a small sphere overlap)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // If the player presses Space and is grounded, apply a vertical force for jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);  // Apply jump on Y-axis
        }
    }

    void ClampPositionWithinBounds()
    {
        // Get the bounds of the bounding box from the Renderer
        Bounds bounds = boundingBoxRenderer.bounds;

        // Clamp the player's position within the bounding box's extents
        float clampedX = Mathf.Clamp(transform.position.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y);
        float clampedZ = Mathf.Clamp(transform.position.z, bounds.min.z, bounds.max.z);

        // Update the player's position with the clamped values
        transform.position = new Vector3(clampedX, clampedY, clampedZ);
    }
}
