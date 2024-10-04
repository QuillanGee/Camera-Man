using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDCharacterMovement : MonoBehaviour {

    public float speed = 1f;
    public float JumpHeight;
    public bool InAir = false;
    private Animator animator;

    private Rigidbody2D rb;
    
    private bool facingRight = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        InAir = false;
        // Debug.Log("InAir false");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        InAir = true;
        // Debug.Log("InAir True");
    }

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * -speed, rb.velocity.y);

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


        // Set animation move state (you can adjust this for different animations)
        if (moveHorizontal != 0)
        {
            animator.SetInteger("moveState", 1);  // Walking animation
        }
        else
        {
            animator.SetInteger("moveState", 0);  // Idle animation
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (InAir == false)
            {
                print("jumped");
                rb.AddForce(new Vector2(0, JumpHeight), ForceMode2D.Impulse);
            }
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