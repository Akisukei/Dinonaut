using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement2D : MonoBehaviour
{
    // Editible fields in inspector
    [SerializeField] private float movementSpeed = 350f;
    [SerializeField] private float movementSmoothing = 0.01f;       // time that takes to move character (read docs on SmoothDamp)
    [SerializeField] private float jumpForce = 8;
    [SerializeField] private float lowJumpGravityMultiplier = 2f;   // make jump low by increasing gravity on character
    [SerializeField] private LayerMask groundLayer;                 // what is considered ground for character
    [SerializeField] private LayerMask wallLayer;                   // what is considered a wall for character
    [SerializeField] private Collider2D rightCollider;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D leftCollider;

    // Fields used in other scripts
    [HideInInspector] public bool onGround;

    // Components, scripts, and object references
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SoundPlayer soundPlayer;
    private Transform groundDetector;               // empty object that's positioned center of an area to detect (i.e. feet)
    private Transform leftWallDetector;
    private Transform rightWallDetector;
    // Internal instance members
    private bool isJumping = false;
    private float jumpFallMultiplier = 2.5f;        // increased gravity when falling
    private bool touchingRightWall;
    private bool touchingLeftWall;
    private float layerDetectorRadius = 0.1f;
    private Vector2 curVelocity = Vector2.zero;     // a reference for SmoothDamp method to use
    private bool facingRight = true;
    private ContactFilter2D d = new ContactFilter2D();



    // Called to init variables and set up components before Start
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundPlayer = GetComponent<SoundPlayer>();
        groundDetector = GameObject.Find("GroundDetector").transform;
        leftWallDetector = GameObject.Find("LeftWallDetector").transform;
        rightWallDetector = GameObject.Find("RightWallDetector").transform;
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // Check if character is grounded
        onGround = false;
        touchingRightWall = false;
        touchingLeftWall = false;

        //onGround = rigidBody.IsTouchingLayers(groundLayer);
        // note: players that press jump 1 pixel before touching ground cannot jump until engine detects exact collision
        // using ground detection below improves smoother jumping flow for players

        // Within a radius of the ground detector, find all layers of type ground within that area
        /*Collider2D[] colliders = Physics2D.OverlapCircleAll(groundDetector.position, layerDetectorRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)  // ensure collider ISNT its own character collider
                onGround = true;
        }*/
        Collider2D[] colliders = new Collider2D[2];

        d.SetLayerMask(groundLayer);
        groundCollider.OverlapCollider(d, colliders);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null)
            {
                Debug.Log("colliders1: " + colliders[i].name);
                onGround = true;
            }
        }
        // Check if wall on left side of character
        /*Physics2D.OverlapCollider(groundCollider, d, colliders);  //OverlapCircleAll(leftWallDetector.position, layerDetectorRadius, wallLayer);
        for (int i = 0; i < colliders.Length; i++) { 
            //Debug.Log("colliders: " + colliders[i].name);
            if (colliders[i].IsTouchingLayers(groundLayer)) onGround = true;
        }*/
        /*if(!onGround)
        {
            if (rightCollider.IsTouchingLayers(wallLayer) || rightCollider.IsTouchingLayers(groundLayer))
                touchingRightWall = true;
            if (leftCollider.IsTouchingLayers(wallLayer) || leftCollider.IsTouchingLayers(groundLayer))
                touchingLeftWall = true;
        }*/
        if (!onGround) {
            d.ClearLayerMask();
            rightCollider.OverlapCollider(d, colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null && colliders[i].gameObject != gameObject)
                {
                    Debug.Log("colliders2: " + colliders[i].name);
                    if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Ground") ||
                    colliders[i].gameObject.layer == LayerMask.NameToLayer("Wall"))
                        touchingRightWall = true;
                }
            }
            leftCollider.OverlapCollider(d, colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null && colliders[i].gameObject != gameObject)
                {
                    Debug.Log("colliders3: " + colliders[i].name);
                    if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Ground") ||
                    colliders[i].gameObject.layer == LayerMask.NameToLayer("Wall"))
                    touchingLeftWall = true;
                }
            } }
        // Check if wall on left side of character
        /*Physics2D.OverlapCollider(rightCollider, d, colliders);  //OverlapCircleAll(leftWallDetector.position, layerDetectorRadius, wallLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log("colliders: " + colliders[i].name);
            if (colliders[i].IsTouchingLayers(wallLayer)) touchingRightWall = true;
        }
        //if (colliders.Length > 0) Debug.Log("touching//touchingLeftWall = true;
        /*for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                touchingLeftWall = true;
        }*/
        // Check if wall on right side of character
        /*colliders = Physics2D.OverlapCircleAll(rightWallDetector.position, layerDetectorRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Ground") ||
                    colliders[i].gameObject.layer == LayerMask.NameToLayer("Wall"))
                touchingRightWall = true;
        }
        colliders = Physics2D.OverlapCircleAll(leftWallDetector.position, layerDetectorRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer == LayerMask.NameToLayer("Ground") ||
                    colliders[i].gameObject.layer == LayerMask.NameToLayer("Wall"))
                touchingLeftWall = true;
        }
        /*Physics2D.OverlapCollider(leftCollider, d, colliders);  //OverlapCircleAll(leftWallDetector.position, layerDetectorRadius, wallLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log("colliders: " + colliders[i].name);
            if (colliders[i].IsTouchingLayers(wallLayer)) touchingLeftWall = true;
        }

        // Update animation
        animator.SetFloat("runningSpeed", Mathf.Abs(rigidBody.velocity.x));
        /*if((walkingGrassSfx.isPlaying || walkingStoneSfx.isPlaying) && Mathf.Abs(rigidBody.velocity.x) <= 0.01)
        {
            walkingGrassSfx.Stop();
            walkingStoneSfx.Stop();
        }*/
    }

    // Move player's x position when moving right/left and y if player jumps
    public void Move(float movement, bool jumpPressed)
    {
        // Move player's x position with SmoothDamp 
        if (!(movement > 0 && touchingRightWall) && !(movement < 0 && touchingLeftWall))
        {
            // Params: current position, target position, current velocity (modified by func), time to reach target (smaller = faster)
            Vector2 targetPosition = new Vector2(movement * movementSpeed, rigidBody.velocity.y);
            rigidBody.velocity = Vector2.SmoothDamp(rigidBody.velocity, targetPosition, ref curVelocity, movementSmoothing);

            /*if (SceneManager.GetActiveScene().name == "LevelOneScene" && onGround && !walkingGrassSfx.isPlaying)
                walkingGrassSfx.Play();
            else if(onGround && !walkingStoneSfx.isPlaying)
                walkingStoneSfx.Stop();*/
        }

        // Make sure player can't jump again until they're on ground and let go of jump button
        if (onGround && !Input.GetButton("Jump"))
            isJumping = false;

        // Move player's y position if player jumps
        if (jumpPressed && onGround && !isJumping)
        {
            isJumping = true;
            rigidBody.velocity = Vector2.up * jumpForce;
            soundPlayer.playJumpSfx();
        }
        // Modify player's falling speed by modifying gravity scale
        else if (isJumping)
        {
            // when player wants to do low jumping, increase gravity before falling
            // note we substract -1 with multiplier because engine already apply 1 multiple of gravity 
            if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpGravityMultiplier - 1) * Time.fixedDeltaTime;
            else if (rigidBody.velocity.y < 0)
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
