using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement2D : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 350f;
    [SerializeField] private float movementSmoothing = 0.01f;       // time that takes to move character (read docs on SmoothDamp)
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private LayerMask groundLayer;                 // what is considered ground for character
    [SerializeField] private LayerMask wallLayer;                   // what is considered a wall for character
    [SerializeField] private Transform layerDetector;               // empty object that's positioned center of an area to detect (i.e. feet)

    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool onGround;
    private bool touchingWall;
    private bool isJumping;
    private float jumpFallMultiplier = 2.5f;
    private float layerDetectorRadius = 0.5f;
    private Vector2 curVelocity = Vector2.zero;     // a reference for SmoothDamp method to use
    private bool facingRight = true;


    // Called to init variables and set up components before Start()
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // Check if character is grounded
        onGround = false;
        touchingWall = false;

        //onGround = rigidBody.IsTouchingLayers(groundLayer);
        // note: players that press jump 1 pixel before touching ground cannot jump until engine detects exact collision
        // using ground detection below improves smoother jumping flow for players

        // Within a radius of the ground detector, find all layers of type ground within that area
        Collider2D[] colliders = Physics2D.OverlapCircleAll(layerDetector.position, layerDetectorRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)  // ensure collider ISNT its own character collider
                onGround = true;
        }
        // Find of type wall as well
        colliders = Physics2D.OverlapCircleAll(layerDetector.position, layerDetectorRadius, wallLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)  // ensure collider ISNT its own character collider
                touchingWall = true;
        }

        // Update animation
        animator.SetFloat("runningSpeed", Mathf.Abs(rigidBody.velocity.x));
        animator.SetFloat("airborneSpeed", Mathf.Abs(rigidBody.velocity.y));
        animator.SetBool("onGround", onGround);
    }

    // Move player's x position when moving right/left and y if player jumps
    public void Move(float movement, bool jumpPressed)
    {
        // Move player's x position with SmoothDamp 
        if (!touchingWall)
        {
            // Params: current position, target position, current velocity (modified by func), time to reach target (smaller = faster)
            Vector2 targetPosition = new Vector2(movement * movementSpeed, rigidBody.velocity.y);
            rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, targetPosition, ref curVelocity, movementSmoothing);
        }

        // Make sure player can't jump again until they're on ground and let go of jump button
        if (onGround && !Input.GetButton("Jump"))
            isJumping = false;

        // Move player's y position if player jumps OR does a wall jump
        if (jumpPressed && onGround && !isJumping)
        {
            isJumping = true;
            rigidBody.velocity = Vector2.up * jumpForce;
        }
        // Modify player's falling speed by modifying gravity scale
        else if (isJumping && rigidBody.velocity.y < 0)
        {
            // note we substract -1 with multiplier because engine already apply 1 multiple of gravity 
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (jumpFallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // flip character when moving left or right
        if (movement < 0 && facingRight)
            flipCharacter();
        else if (movement > 0 && !facingRight)
            flipCharacter();
    }

    // Flip character sprite
    private void flipCharacter()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
