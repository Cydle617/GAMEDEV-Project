using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    // [SerializeField] 
    // PlayerStats stats;
    [SerializeField] 
    float moveSpeed = 5f;
    [SerializeField] 
    float jumpForce = 5f;
    [SerializeField] 
    float groundCheckDistance = 1.1f;

    [Header("Jump Settings")]
    [SerializeField] private bool canJump = false;

    CharacterController controller;
    Vector2 moveInput;
    Rigidbody rb;
    bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();

        // Freezes the rotations from the Rigidbody to prevent the player from spinning randomly
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame   
    void Update()
    {
        // Checks if the player is on the ground or not using raycasting.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    void FixedUpdate()
    {
        // Checks and calculates the player's movement based on input and applies it to the Rigidbody.
        // Preserve vertical velocity (gravity) while modifying horizontal movement
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveInput.x * moveSpeed, currentVelocity.y, moveInput.y * moveSpeed);
        rb.linearVelocity = targetVelocity;
    }

    public void OnMove(InputValue value)
    {
        // Gets the movement input
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && canJump)
        {
            // Adds a vertical force to make the player jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Call this method from a trigger to ENABLE jumping
    public void EnableJumping()
    {
        canJump = true;
    }

    // Call this method from a trigger to DISABLE jumping
    public void DisableJumping()
    {
        canJump = false;
    }
}
