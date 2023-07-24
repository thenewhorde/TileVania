using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 30f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float deathSeconds = 1f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isJumpingOffLadder;
    private float lastJumpTime;

    bool isAlive = true;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }


    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    
    void OnFire (InputValue value)
    {
        if (!isAlive) { return; }
        //Spawn (what we spawn, where we spawn, rotation)
        Instantiate(bullet, gun.position, transform.rotation);
        Debug.Log("Is the bullet firing?");
    }


    void OnJump(InputValue value) 
    {
        if (!isAlive) { return; }

        bool jumpTouchClimb = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        bool jumpTouchGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));



        if (jumpTouchClimb || jumpTouchGround)
        {
            if (value.isPressed)
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);

                myRigidbody.gravityScale = gravityScaleAtStart;
                myAnimator.SetBool("isClimbing", false);

                //set jump integer
                lastJumpTime = Time.time;

                //matt

                if (jumpTouchClimb)
                {
                    isJumpingOffLadder = true;

                    Debug.Log("Did click jump on ladder");
                }

            }
        }


    }

    void ClimbLadder()
    {
        //Set jump cooldown
        if (Time.time - lastJumpTime <= 0.2f) return;


        //matt: when isJumpingOffLadder is true, skip the ladder logic until the player touches ground again
        if (isJumpingOffLadder) 
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
                isJumpingOffLadder = false;

            Debug.Log("if JumpingOffLadder condition is met");

            if (moveInput.y !=0 || Input.GetButtonDown("OnJump") && !myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))

            {
                Vector2 catchVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
                myRigidbody.velocity = catchVelocity;
                myRigidbody.gravityScale = 0f;
                myAnimator.SetBool("isClimbing", true);
                isJumpingOffLadder = false;
            }

            return;
        }


        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            //set gravity at default amount

            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);

            Debug.Log("if JumpingOffLadder condition is not met");
            return;
        }


        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

        //set default x - axis speed and y - axis climb speed
        
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);

        myRigidbody.velocity = climbVelocity;

        myRigidbody.gravityScale = 0f;

        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);

        Debug.Log("is the non-if statements being ran?");

    }


    //how to block an area of code (alt > mouse drag > ctrl + k > ctrl + c / ctrl + u for uncomment)


    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playHasHorizontalSpeed);

    }

    void FlipSprite()
    {
        bool playHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);

        }

    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        //wait for this amount of time
        yield return new WaitForSeconds(deathSeconds);


        //After waiting, call ProcessPlayerDeath
        //calls on the public void on the game session script to reduce lives
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

}
