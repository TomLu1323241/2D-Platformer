using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Configs
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    // States
    bool isAlive = true;
    // Cashed components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    BoxCollider2D feetCollider;
    CapsuleCollider2D BodyCollider;


    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        feetCollider = GetComponent<BoxCollider2D>();
        BodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        flipSprite();
        ClimbLadder();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * speed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        if (Mathf.Abs(myRigidBody.velocity.x) > 0)
        {
            myAnimator.SetBool("Running", true);
        }
        else
        {
            myAnimator.SetBool("Running", false);
        }
    }

    private void ClimbLadder()// TODO: Add velocity to form jumping onto ladder
    {
        if (BodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myRigidBody.gravityScale = 0;
            if (Mathf.Abs(myRigidBody.velocity.y) > 0 && BodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
            {
                myAnimator.SetBool("Climbing", true);
            }
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            myRigidBody.velocity = climbVelocity;
        }
        else
        {
            myRigidBody.gravityScale = 1;
        }
        if (!BodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myAnimator.SetBool("Climbing", false);
        }
        if (Mathf.Abs(myRigidBody.velocity.y) < 1 && BodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myAnimator.speed = 0;
        } else
        {
            myAnimator.speed = 1;
        }
    }

    private void Jump()// TODO: Add autohop
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
            (CrossPlatformInputManager.GetButtonDown("Jump") ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void flipSprite()
    {
        if (myRigidBody.velocity.x > 0)
        {
            mySpriteRenderer.flipX = false;
        }
        else if (myRigidBody.velocity.x < 0)
        {
            mySpriteRenderer.flipX = true;
        }
    }
}
