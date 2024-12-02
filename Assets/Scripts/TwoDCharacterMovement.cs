using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Need this for scene management

public class TwoDCharacterMovement : MonoBehaviour {

    public float speed = 3f;
    private float JumpHeight = 5f;
    private bool isGrounded = true;
    [SerializeField] private Transform groundCheckPosition;
    public LayerMask groundLayer;
    

    private Rigidbody2D rb;
    
    private Animator animator;
    private bool facingRight = false;
    
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            //to make sure rb doesn't go to sleep when character is staying still
            rb.WakeUp();
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneManager.LoadScene(1);  // Loads scene at build index 1
            }
        }
    }

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * -speed, rb.velocity.y);
        
        isGrounded = Physics2D.OverlapPoint(groundCheckPosition.position, groundLayer);

        // Set animation move state (you can adjust this for different animations)
        if (moveHorizontal != 0)
        {
            animator.SetInteger("moveState", 1);  // Walking animation
            // Check if the player is moving right and is not already facing right
            if (moveHorizontal > 0 && !facingRight)
            {
                // Walk right
                Flip();  // Flip the character to face right
            }
            // Check if the player is moving left and is not already facing left
            else if (moveHorizontal < 0 && facingRight)
            {
                // Walk left
                Flip();  // Flip the character to face left
            }
        }
        else
        {
            animator.SetInteger("moveState", 0);  // Idle animation
        }
        
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        //allows us to press jump before hitting the ground
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        //allows us to press jump after leaving the groud
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    void Flip()
    {
        // Toggle the direction the character is facing
        facingRight = !facingRight;

        // Flip the character by changing its x-scale
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}