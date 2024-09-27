using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterMovement : MonoBehaviour
{
    public float walkSpeed = 2.0f;            // Movement speed
    public float jumpForce = 8.0f;            // Jump force
    public float gravity = 20.0f;             // Gravity force
    public float mouseSensitivity = 2.0f;     // Mouse sensitivity for look around

    private CharacterController controller;   // Reference to the CharacterController
    private Vector3 moveDirection = Vector3.zero; // Direction of player movement
    private float verticalVelocity = 0.0f;    // Velocity for jumping and falling
    private float rotationX = 0.0f;           // Pitch rotation (up-down)

    public Camera fpc;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- Mouse look around ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player on the Y-axis (left-right)
        transform.Rotate(0, mouseX, 0);

        // Rotate the camera (pitch) for looking up and down
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit rotation to 90 degrees up and down
        fpc.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // --- Movement ---
        if (controller.isGrounded)
        {
            // Move in the direction the player is facing
            float moveX = Input.GetAxis("Horizontal"); // A/D (Left/Right)
            float moveZ = Input.GetAxis("Vertical");   // W/S (Forward/Backward)

            moveDirection = transform.right * moveX + transform.forward * moveZ;
            moveDirection *= walkSpeed;

            // Jumping
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }

        // Apply gravity (even when grounded to maintain smoothness)
        verticalVelocity -= gravity * Time.deltaTime;

        // Add vertical velocity to the movement direction (jump/fall)
        moveDirection.y = verticalVelocity;

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);

        // --- Exit Lock Mode ---
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}