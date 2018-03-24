using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	private LayerMask ground;
	private bool grounded;
	private const float jumpForce = 15;
	private const float moveSpeed = 5;

	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;
	private Animator myAnimator;

	// Use this for initialization
	void Start () {
		ground = LayerMask.GetMask ("Ground");
		grounded = false;

		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D> ();
		myAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.IsTouchingLayers (myCollider, ground);

		float upVel = myRigidbody.velocity.y;
		if (grounded && (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0))) {
			upVel = jumpForce;
		}
		myRigidbody.velocity = new Vector2 (moveSpeed, upVel);

		myAnimator.SetFloat ("Speed", myRigidbody.velocity.x);
		myAnimator.SetBool ("Grounded", grounded);
	}

   public float GetJumpForce()
   {
      return jumpForce;
   }
}
