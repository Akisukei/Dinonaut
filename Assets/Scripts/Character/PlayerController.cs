using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement2D))]
public class PlayerController : MonoBehaviour
{
    public CharacterMovement2D movementController;

    private Animator animator;
    private float moveHorizontal = 0f;
    private bool jumpPressed = false;
    //private bool isKicking = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Updates every frame
    void Update()
    {
        // Get user button inputs
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        //if (Input.GetButtonDown("Interact"))
        //    isKicking = true;
        //else
        //    isKicking = false;
        animator.SetBool("isKicking", Input.GetButtonDown("Interact"));
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // add force relative to the time its been since last frame (i.e. add more force if 1 frame = 1 sec)
        movementController.Move(moveHorizontal * Time.fixedDeltaTime, jumpPressed);
        jumpPressed = false;
    }
}
