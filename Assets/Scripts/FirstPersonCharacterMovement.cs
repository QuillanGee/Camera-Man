using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCharacterMovement : MonoBehaviour
{
    //Player Control Settings
    private float walkSpeed = 50.0f;            // Movement speed
    public float gravity = 20.0f;             // Gravity force
    private float mouseSensitivity = 200.0f;     // Mouse sensitivity for look around
    private float groundDrag = 9.0f;
    
    //for moving foward backward left and right
    [SerializeField] private Transform orientation;
    private float rotationX = 0.0f;           // Pitch rotation (up-down)
    private float rotationY = 0.0f;
    private float moveX = 0.0f;
    private float moveY = 0.0f;
    private Vector3 moveDirection = Vector3.zero; // Direction of player movement

    //Ground Check
    private float sphereRadius = 1.0f;
    public LayerMask groundLayer;
    public LayerMask blockLayer;
    private bool isGrounded = true;

    //Jump Parameters
    private float jumpForce = 10.0f;
    private float jumpCooldown = 0.25f;
    private float airMultiplier = 0.4f;
    bool readyToJump = true;
    
    private Camera fpc;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fpc = GetComponentInChildren<Camera>();

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void FixedUpdate()
    {
        MoveCharacter();
    }
    void Update()
    {
        //CHECK GROUNDED on ground layer and block layer
        if (Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer) || Physics.Raycast(transform.position, Vector3.down, 1f, blockLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        if (isGrounded)
        {
            rb.drag = groundDrag;
            print("Grounded");
        }
        else
        {
            rb.drag = 0;
        }
        
        MyInput();
        SpeedControl();
        
        // --- Mouse look around ---
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        // Rotate the player on the X and Y-axis (up-down-left-right)
        rotationY += mouseX;
        rotationX -= mouseY;
        
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit rotation to 90 degrees up and down
        fpc.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
        
    }


    private void MyInput()
    {
        // Move in the direction the player is facing
        moveX = Input.GetAxis("Horizontal"); // A/D (Left/Right)
        moveY = Input.GetAxis("Vertical");   // W/S (Forward/Backward)

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MoveCharacter()
    {
        if (isGrounded)
        {
            moveDirection = orientation.right * moveX + orientation.forward * moveY;
            rb.AddForce(moveDirection.normalized * walkSpeed, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * walkSpeed * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if (flatVel.magnitude > walkSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * walkSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}