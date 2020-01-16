using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(RaycastController))]
public class Player : MonoBehaviour
{
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////
    /* PLEASE PLACE FIELDS IN ORDER (i.e. put members related to jumping with other used jumping fields) */
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    /* EDITABLE IN INSPECTOR */
    [SerializeField] private float moveSpeed = 350f;
    [SerializeField] private float moveSmoothing = 0.01f;           // time that takes to move character (read docs on SmoothDamp)
    [SerializeField] private float jumpForce = 8;
    [SerializeField] private float lowJumpGravityMultiplier = 2f;   // make jump low by increasing gravity on character

    /* FIELDS PUBLIC FOR OTHER SCRIPTS */
    [HideInInspector] public bool onGround;

    // COMPONENTS, SCRIPTS, AND OBJECT REFERENCES
    private RaycastController raycastController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterSoundPlayer charSfxPlayer;
    // INTERNAL INSTANCE MEMBERS
    private Vector2 bodyVelocity;
    private float gravity;                          // general gravity on body
    private float moveDirection = 0f;               // direction in which character is moving
    private Vector2 curVelocity = Vector2.zero;     // a reference for SmoothDamp method to use
    private bool jumpPressed = false;
    private bool isJumping = false;            
    private float fallGravityMultiplier = 2.5f;     // 2.5 means gravity increased by 2.5x when falling after jumping
    private bool isKicking = false;


    // TODO re-evaluate fields to use?
    /* STATS */
    private int health;
    private bool invincible;
    private bool hurt;
    private int coins; //Not sure which should keep track of coins for now.
	private float invulnerableTime = 2f;

    // Called before Start
    private void Awake()
    {
        raycastController = GetComponent<RaycastController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start ()
	{
		//Kinematic formula, solve for acceleration going down
		gravity = -(2 * 2.5f) / Mathf.Pow (0.36f, 2);
	}
	
	// Update is called once per frame
	void Update ()
	{
        GetPlayerInput();

        // update animator
        animator.SetFloat("runningSpeed", Mathf.Abs(moveDirection));
        animator.SetBool("isKicking", isKicking);
        isKicking = false;

		//Checks current state of game obj and makes adjustment to velocity if necessary
		CheckState ();
    }

    // FixedUpdate is called at a fixed interval, all physics code should be in here only
    void FixedUpdate()
    {
        if (jumpPressed) OnJumpDown();
        jumpPressed = false;

        // gravity makes game object fall at all times
        bodyVelocity.y += gravity * Time.deltaTime;
        // move player's x axis with smoothdamp
        Move(moveDirection * moveSpeed * Time.fixedDeltaTime);

        // modify player's falling gravity if jumping
        if (isJumping)
        {
            // do a low jump by raising gravity even when ascending if player performs a low jump
            // note we substract -1 with multiplier because engine already apply 1 multiple of gravity 

            // low jump
            if (bodyVelocity.y > 0 && !Input.GetButton("Jump"))
                bodyVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpGravityMultiplier - 1) * Time.fixedDeltaTime;
            // high jump
            else if (bodyVelocity.y < 0)
                bodyVelocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // Get player's input to determine action states
    private void GetPlayerInput()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");     // 1 = moving right, -1 = moving left, 0 = idle
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
        if (Input.GetButtonDown("Interact"))
            isKicking = true;
    }

    // Moves the player. Raycast checks for walls and floors collision.
    public void Move (float moveAmount)
	{
        // Updates raycast position as game object moves.
        raycastController.UpdateRayOrigins();
        raycastController.collision.Reset();

        // Flips player sprite and raycast if character turns direction
        if (spriteRenderer.flipX ? (moveDirection > 0) : (moveDirection < 0))       // 1 = moving right, -1 = moving left, 0 = idle
            FlipFacingDirection();

        // Check collisions - if found, moveAmount velocity will be reduced appropriately
        Vector2 targetPosition = new Vector2(moveAmount, bodyVelocity.y);
        raycastController.checkCollisions(ref targetPosition);

        // Params: current position, target position, current velocity (modified by func), time to reach target (smaller = faster)
        //Vector2 targetPosition = new Vector2(moveAmount, bodyVelocity.y);
        bodyVelocity = Vector2.SmoothDamp(bodyVelocity, targetPosition, ref curVelocity, moveSmoothing);

		// Actually changing the velocity of game object
		//transform.Translate (moveAmount);
	}

    private void OnJumpDown()
    {
        //onGround = raycastController.collision.below
        if (onGround)
        {
            // check if player lets go of jump btn before enabling to jump again
            if (!Input.GetButton("Jump"))
                isJumping = false;
            if (!isJumping)
            {
                bodyVelocity = Vector2.up * jumpForce;
                isJumping = true;
                //soundPlayer.playJumpSfx();
            }
        }
    }

	// Sets the facing direction of player
	private void FlipFacingDirection()
    {
        // flip raycast
        raycastController.collision.collDirection = (int)Mathf.Sign(moveDirection);
        // flip sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    // This method checks the state of the player game object every frame
    private void CheckState ()
	{
        onGround = raycastController.collision.below;

        // If grounded, reset falling velocity
        // If hit ceiling, set velocity.y to 0 to prevent accumulating
        if (onGround || raycastController.collision.above)
            bodyVelocity.y = 0f;

		//Apparantly, Color isn't something you can modify like transform.position
		//Reduce transparency by half when hurt.
		Color c = spriteRenderer.color;
		if (invincible) 
			c.a = 0.5f;
		else 
			c.a = 1f;
		
		spriteRenderer.color = c;
	}

	

	/// <summary>
	/// Resets the invincble boolean. Used by OnTriggerEnter2D, to return player to vulnerable state 
	/// after slight moment of invincibility.
	/// </summary>
	private void resetInvincible ()
	{
		invincible = false;
	}

	/// <summary>
	/// reset hurt boolean. Used in update(), to allow player to move again.
	/// </summary>
	private void resetHurt ()
	{
		hurt = false;
	}


    /// <summary>
    /// This trigger will check for collision with traps. Not the level.
    /// If collided with traps, player's health reduces and becomes invulnerable
    /// for a short while.
    /// if item, then pick up
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
		if (!invincible) {
			if (other.tag == "Trap") {
				ReceiveDamage ();
			}
		}*/

        if (other.tag == "Coin")
        {
            coins++;
        }
    }

    public void ReceiveDamage()
    {
        if (!invincible)
        {
            bodyVelocity.y = 0;
            animator.Play("g_dino_damaged");
            //Receive damage
            health--;
            Debug.Log("HEALTH: " + health);

            //Makes slight pause and prevent player from moving when hit
            hurt = true;
            Invoke("resetHurt", 0.2f);

            //Become invulnerable for 2 seconds
            invincible = true;
            Invoke("resetInvincible", invulnerableTime);
        }
    }
}
