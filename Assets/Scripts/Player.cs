using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

//control + rr to change name to something else across the whole script

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float gravity = -10f, movementSpeed = 10f, jumpHeight = 8f, centreRadius = .5f;

    private CharacterController2D controller;
    private SpriteRenderer rend;
    private Animator anim;

    private Vector3 velocity;
    private bool isClimbing = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }


    //private Vector3 motion; // store the difference in movement


    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //Get Horizontal Input (A/D or Left/Right arrows)
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        //If the controller is touching the ground .. if character is grounded
        if (!controller.isGrounded)
        {
            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
        }
        if(controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //Make the player jump
                Jump();
            }
        }
        //if space is pressed 
       

        // Climb up or down depending on Y value
        Climb(inputV, inputH);

        // Move left or right depending on X value
        Move(inputH);


        //controller.Move(transform.right * inputH * movementSpeed * Time.deltaTime); 
        // Move the controller with modified motion
        controller.Move(velocity * Time.deltaTime);
    }



    public void Move(float inputH)
    {
        velocity.x = inputH * movementSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        rend.flipX = inputH < 0;
    }

    public void Climb(float inputV, float inputH)
    {
        bool isOverLadder = false; // Is the player overlapping the ladder?
        Vector3 inputDir = new Vector3(inputH, inputV, 0);

        #region Part 1 - Detecting Ladders
        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);
        // Loop through all hit objects
        foreach (var hit in hits)
        {
            //  Check if tagged "Ladder"
            if (hit.tag == "Ladder")
            {
                // Player is overlapping a Ladder!
                isOverLadder = true;
                break; // Exit just the foreach loop (works for any loop)
            }
        }
        // If the player is overlapping AND input vertical is made
        if (isOverLadder && inputV != 0)
        {
            anim.SetBool("IsClimbing", true);
            // The player is in Climbing state!
            isClimbing = true;
        }
        #endregion

        #region Part 2 - Translating the Player
        // If player is climbing
        if (isClimbing)
        {
            velocity.y = 0;
            // Move player up and down on the ladder (additionally move left and right)
            transform.Translate(inputDir * movementSpeed * Time.deltaTime);
        }
        #endregion

        if (!isOverLadder)
        {
            anim.SetBool("IsClimbing", false);
            isClimbing = false;
        }

        anim.SetFloat("ClimbSpeed", inputDir.magnitude * movementSpeed);
    }

    public void Hurt()
    {

    }

    public void Jump()
    {
        // 4
        velocity.y = jumpHeight;
    }
}


//void Update()  //MANNYS CODE DOUBLE CHECK AGAINST MINE
//  {
//    float inputH = Input.GetAxis("Horizontal");
//float inputV = Input.GetAxis("Vertical");
//    // If character is:
//    if (!controller.isGrounded && // NOT grounded
//        !isClimbing) // NOT climbing
//    {
//      // Apply gravity
//      velocity.y += gravity* Time.deltaTime;
//    }
//    // If space is pressed
//    if (Input.GetButtonDown("Jump"))
//    {
//      // Make the player jump
//      Jump();
//    }
//    // Climb up or down depending on Y value
//    Climb(inputV);
//// Move left or right depending on X value
//Move(inputH);
//// Move the controller with modified motion
//controller.move(velocity* Time.deltaTime);
//  }