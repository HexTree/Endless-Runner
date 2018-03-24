using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
	private LayerMask ground;
	private LayerMask bound;
	private bool grounded;
	private const float jumpForce = 15;
	private const float moveSpeed = 5;

	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;
	private Animator myAnimator;

	// Use this for initialization
	void Start () 
   {
		ground = LayerMask.GetMask ("Ground");
		bound = LayerMask.GetMask ("Bound");
		grounded = false;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D> ();
		myAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
   {
		grounded = Physics2D.IsTouchingLayers (myCollider, ground);

		float upVel = myRigidbody.velocity.y;
		if (grounded && (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0))) 
      {
			upVel = jumpForce;
		} 
		else if (Physics2D.IsTouchingLayers (myCollider, bound)) 
      {
			if (Input.GetKeyDown (KeyCode.R)) {
				RespawnPlayerDebug();
			}
		}
		myRigidbody.velocity = new Vector2 (moveSpeed, upVel);

		myAnimator.SetFloat ("Speed", myRigidbody.velocity.x);
		myAnimator.SetBool ("Grounded", grounded);
	}

   public float GetJumpForce()
   {
      return jumpForce;
   }
		
	//"Respawn" the player, for debugging purposes only
	private void RespawnPlayerDebug() 
   {
		Vector2 respawnPosition = new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.y + myRigidbody.GetComponent<Renderer>().bounds.size.y);
		myRigidbody.position = respawnPosition;
	}
}
