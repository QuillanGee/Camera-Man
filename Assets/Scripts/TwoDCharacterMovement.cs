using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDCharacterMovement : MonoBehaviour {

    public float speed = 1f;
    public float JumpHeight;
    public bool InAir = false;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        InAir = false;
        // Debug.Log("InAir false");
    }
    private void OnCollisionExit(Collision collision)
    {
        InAir = true;
        // Debug.Log("InAir True");
    }

    void FixedUpdate() {
        Vector2 NoMovement = new Vector2(0f, 0f);

        float moveHorizontal = Input.GetAxis("Horizontal");
            if (moveHorizontal > 0)
            {
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);

                }
            }
            if (moveHorizontal < 0)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        // if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow))) {
        //     if (InAir == false)
        //     {
        //         print("jumped");
        //         rb.AddForce(new Vector2(0, JumpHeight), ForceMode.Impulse);
        //     }
        // }
    }

}