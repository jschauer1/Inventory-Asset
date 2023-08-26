using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // Speed at which the player moves

    private Rigidbody2D rb;
    private SpriteRenderer sR;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveHorizontal = 0;

        // Check for A, D or arrow key input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveHorizontal = -1; // Move left
            sR.flipX = true;
            
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveHorizontal = 1; // Move right
            sR.flipX = false;

        }
        if(rb.velocity.magnitude == 0)
        {
            animator.SetBool("Running", false);
        }
        else
        {
            animator.SetBool("Running", true);

        }

        // Apply movement to the Rigidbody2D
        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }
}
