using UnityEngine;

public class PlayerControl : MonoBehaviour 
{
    // Jump Mechanic Constants
    private const float JUMP_FORCE = 10f; // Affects min jump, speed of continuous jump, and inevitably height.
    private const float MOVE_SPEED = 5f;
    private const float JUMP_TIME = .25f; // Affects max airtime of jump and max jump height.
    private const float FALL_MULTIPLIER = 2f; // Affects fall speed after full jump.
    private const float LOW_JUMP_MULTIPLIER = 1.5f; // Affects fall speed after all other jumps. (should be lower than above)

    // Ground Detection Variables
	private LayerMask ground;
    private bool grounded;
	private LayerMask bound;
	
    // Jump Mechanic Variables
    private bool initialJump;
    private bool stoppedJumping;
    private bool jumpButtonHeld;
    private bool jumpButtonReleased;
    private float jumpTimeCounter = 0f;
    private float normalGravity;

    // Player Variables
	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;
	private Animator myAnimator;

	// Use this for initialization
	void Start () 
    {
		ground = LayerMask.GetMask ("Ground");
		bound = LayerMask.GetMask ("Bound");
		grounded = false;
        initialJump = false;
        stoppedJumping = true;
        jumpButtonHeld = false;
        jumpButtonReleased = false;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D> ();
		myAnimator = GetComponent<Animator> ();

        normalGravity = myRigidbody.gravityScale;
	}
	
	// Update is called once per frame
    // All input detection and anything not directly related to physics here.
	void Update () 
    {
	    grounded = Physics2D.IsTouchingLayers (myCollider, ground);

		if (Physics2D.IsTouchingLayers (myCollider, bound)) 
        {
			if (Input.GetKeyDown (KeyCode.R))
            {	
				RespawnPlayerDebug();
			}
		}

		if (grounded)
		{
		    jumpTimeCounter = JUMP_TIME;
		}

		if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
		{
		    initialJump = true;
		}

		jumpButtonHeld = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
		{
		    jumpButtonReleased = true;
		}

		myAnimator.SetBool("Grounded", grounded);
    }

    // Alterations to velocity and other physics here. Input Booleans determined in Update().
    void FixedUpdate()
    {

   		myRigidbody.velocity = new Vector2(MOVE_SPEED, myRigidbody.velocity.y);

        // Does initial min jump if grounded and jump button pressed.
        if (initialJump)
        {
            myRigidbody.velocity += Vector2.up * JUMP_FORCE;
            initialJump = false;
            stoppedJumping = false;
        }

        // If the Jump button is held after initial jump, continues jump until the timer (jumpTimeCounter) <= 0.
        if (jumpButtonHeld && !stoppedJumping)
        {
            if (jumpTimeCounter > 0)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, JUMP_FORCE);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        // If the jump button is released then the jump immediately ends.
        if (jumpButtonReleased)
        {
            jumpButtonReleased = false;
            stoppedJumping = true;
            jumpTimeCounter = 0;
        }

        /*Where the Mario magic happens, if/else statements below create a gravity shift when 
         jumping, increasing fall speed after jump and an even faster fall if a full jump or outright falling.
         This makes jumping feel a little tighter and less floaty. The settings for this can be adjusted
         with FALL_MULTIPLER for full jump and LOW_JUMP_MULTIPLIER for every other jump.*/
        if (myRigidbody.velocity.y < 0)
        {
            myRigidbody.gravityScale = normalGravity * FALL_MULTIPLIER;
        }
        else if (myRigidbody.velocity.y > 0 && !jumpButtonHeld)
        {
            myRigidbody.gravityScale = normalGravity * LOW_JUMP_MULTIPLIER;
        }
        else
        {
            myRigidbody.gravityScale = normalGravity;
        }

        myAnimator.SetFloat("Speed", myRigidbody.velocity.x);
    }

    //TODO: Adjust this getter for Obstacle Contstuctor
    public float GetJumpForce()
    {
        return JUMP_FORCE;
    }
		
	//"Respawn" the player, for debugging purposes only
	private void RespawnPlayerDebug() 
    {
		Vector2 respawnPosition = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.y + myRigidbody.GetComponent<Renderer>().bounds.size.y);
		myRigidbody.position = respawnPosition;
	}
}
