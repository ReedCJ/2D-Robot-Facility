﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject hook;           // The "hook" in the grappling hook
    [SerializeField] private GameObject shot;           // Our player's projectile
    [SerializeField] private GameObject attack;         // A hitbox representing a melee weapon
    [SerializeField] private GameObject cam;            // The primary virtual camera

    [SerializeField] private CharacterController2D controller;      // The script that processes our movement inputs

    [SerializeField] private float speed = 0.0f;   // Character ground speed

    private float timer;
    
    private bool fire;                      // Attack key input
    public float fireRate;                  // How fast you can shoot
    private float nextFire;                 //counter for fire rate
    private GameObject settingshot;

    public bool enabledDouble;              // public bool for enabling/disabling double jumps

    private bool teather;                   // Teather key input
    private bool crouch;                    // Is player crouched
    private bool jump;                      // Jump key input
    private bool canDouble;                 // bool for being able to double dump
    private bool doubleJump;                // double jump bool
    private bool grounded;                  // On the ground as opposed to in the air?
    private bool camFollow;                 // Camera is in follow mode?
    private GameObject GrappleHook;         // Active Grappling Hook Object
    private TeatherController grappleController;     // Script for swinging player
    static public Animator animator;
    private Rigidbody2D body;
    [System.NonSerialized] public float hMove = 0.0f;               // Ground movement
    [System.NonSerialized] public float vMove = 0.0f;               // Vertical Input and climbing
    [System.NonSerialized] public bool teatherOut;                  // Grappling hook deployed?
    [System.NonSerialized] public bool swinging;                    // Currently swinging?
    [System.NonSerialized] public bool teatherSwinging;             // Currently swinging?
    [System.NonSerialized] public bool facing;                      // True = right, False = left
    public bool up;                                                 // Up Input
    public bool down;                                               // Down Input


    //Run when p;ayer is created
    void Start()
    {
        //Player starts facing right
        facing = true;
        //Player has not double jumped
        doubleJump = false;
        //assign rigidbody to variable
        body = GetComponent<Rigidbody2D>();
        SetInitialState();
        animator = transform.GetChild(2).GetComponent<Animator>();
    }

    void SetInitialState()      // Sets variables 
    {
        camFollow = true;
    }

    // Update is called once per frame
    void Update()
    {
        //timer
        timer += Time.deltaTime;

        if (!MainMenu.isPaused)
        {
            hMove = Input.GetAxisRaw("Horizontal");
            //Sets animation parameter for speed (Movement)
            animator.SetFloat("Speed", Mathf.Abs(hMove));

            //Checks if falling parameter
            animator.SetFloat("Falling", Mathf.Abs(body.velocity.y));
            
            /*
            if (hMove > .01 || hMove < -.01 && grounded && !down)
                animator.SetBool("Running", true);
            else animator.SetBool("Running", false);


            if (hMove > .01 || hMove < -.01 && grounded && down)
                animator.SetBool("Walking", true);
            else animator.SetBool("Walking", false);

            */

            vMove = Input.GetAxisRaw("Vertical");
            #region Keys
            if (Input.GetButtonDown("Jump"))
            {
//                if (hMove != 0)
//                    animator.SetTrigger("Jumping");
//                else animator.SetTrigger("IdleJump");
                if (grounded)
                {
                    jump = true;
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Grounded", false);
                }
                //double jump
                else if (!grounded && canDouble) { doubleJump = true; canDouble = false; }
            }

            if (Input.GetButtonUp("Jump") && !grounded)     // Short hop code
            {
                if (body.velocity.y > 0)
                    body.velocity = new Vector2(body.velocity.x, body.velocity.y * .5f);
            }

            // look/aim up
            if (Input.GetAxisRaw("Vertical") > 0) { up = true; }
            else { up = false; }

            // Aim/look down / crouch
            if (Input.GetAxisRaw("Vertical") < 0) { down = true; }
            else { down = false; }

            if (down && grounded) { crouch = true; }
            else { crouch = false; }

 //           if (crouch)
 //               animator.SetBool("Crouched", crouch);
 //           else if (!crouch)
 //               animator.SetBool("Crouched", crouch);

            //Attack button press/release
            if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Fire1")) { fire = true; }
            else if (Input.GetButtonUp("Attack") || Input.GetButtonUp("Fire1")) { fire = false; }

            if (Input.GetButtonDown("Teather")) { teather = true; }

 //           if (teather == true)
 //               animator.SetBool("Swinging", true);
 //           else if (teather == false)
 //               animator.SetBool("Swinging", false);
        }
      
       
        #endregion
    }

    void FixedUpdate()
    {
        if (!swinging)
            controller.Move(hMove * speed * Time.fixedDeltaTime, crouch, jump, doubleJump);
        else if (swinging && jump)
        {
            if (teatherSwinging)
                grappleController.StartRetracting();
            // Perform swing jump
        }
        else if (swinging)
        {
            if (teatherSwinging)
                grappleController.Swing();
        }

        //direction facing
        if (hMove > 0) { facing = true; }
        else if (hMove < 0) { facing = false; }

        //Assign grounded
        grounded = controller.m_Grounded;
        //Reset double jump if player is grounded
        if (grounded) { canDouble = true; }

        //Fire if enough time has passed between shots and fire button is pressed
        if (fire && timer > fireRate)
        {
            Attack();
            timer = 0.0f;
        }

        //If grapple key is pressed
        if (teather) { CastTeather(); }

        //reset bools at the end of a FixedUpdate
        jump = false;
        doubleJump = false;
        teather = false;
    }

    void CastTeather()           // Currently non functional
    {
        if (!teatherOut && !swinging)
        {
            teatherOut = true;
            GrappleHook = Instantiate(hook, new Vector3(transform.position.x + .2f, transform.position.y + .2f, transform.position.z), Quaternion.identity);
            grappleController = GrappleHook.GetComponent<TeatherController>();
        }
    }

    void Attack()
    {
        //place where the shot spawns
        Vector3 attackSpawn = GetShotSpawn();
        //placeholder rotation
        Quaternion placeholderRotation = new Quaternion();
        //shoot the shot
        settingshot = Instantiate(shot, attackSpawn, placeholderRotation);
        //set the shot direction
        SetShotDirection();
    }

    private Vector3 GetShotSpawn()
    {
        Vector3 attackSpawn = body.position;

        //if up is held always shoot up
        if(up)
        {
            attackSpawn.y++;
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                attackSpawn.x++;
            }
            else if(Input.GetAxisRaw("Horizontal") < 0)
            {
                attackSpawn.x--;
            }
        }
        else
        {
            //if crouch in air
            if(Input.GetAxisRaw("Vertical") < 0 && !grounded)
            {
                attackSpawn.y--;
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    attackSpawn.x++;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    attackSpawn.x--;
                }
            }
            else
            {
                //if facing right
                if(facing)
                {
                    attackSpawn.x++;
                }
                //if facing left
                else
                {
                    attackSpawn.x--;
                }
                //if crouched on ground
                if(crouch && grounded)
                {
                    attackSpawn.y -= 0.5f;
                }
            }
        }

        return attackSpawn;
    }

    private void SetShotDirection()
    {
        //if shooting up
        if (up)
        {
            settingshot.GetComponent<ShotController>().shootVertical = true;

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                settingshot.GetComponent<ShotController>().diagonal = true;
                settingshot.GetComponent<ShotController>().diagonal = true;
                settingshot.GetComponent<ShotController>().shootDiagonal = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                settingshot.GetComponent<ShotController>().diagonal = true;
                settingshot.GetComponent<ShotController>().diagonal = true;
                settingshot.GetComponent<ShotController>().shootDiagonal = false;
            }
        }
        else
        {
            if (!grounded && Input.GetAxisRaw("Vertical") < 0)
            {
                    settingshot.GetComponent<ShotController>().shootVertical = false;
                    //if crouch in air and holding horizontal
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        settingshot.GetComponent<ShotController>().diagonal = true;
                        settingshot.GetComponent<ShotController>().shootDiagonal = true;
                    }
                    else if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        settingshot.GetComponent<ShotController>().diagonal = true;
                        settingshot.GetComponent<ShotController>().shootDiagonal = false;
                    }
            }
            else
            {
                //not a vertical shot
                settingshot.GetComponent<ShotController>().vertical = false;

                //if facing right
                if (facing)
                {
                    settingshot.GetComponent<ShotController>().shootHorizontal = true;
                }
                //if facing left
                else
                {
                    settingshot.GetComponent<ShotController>().shootHorizontal = false;
                }
            }
        }
    }


    public void OnLanding ()
    {
        animator.SetTrigger("Landing");
      //  Debug.Log("Landed Event");
        animator.SetBool("Jumping", false);
        animator.SetBool("Grounded", true);

    }

    public void OnCrouching (bool isCrouching)
    {
        animator.SetBool("Crouching", isCrouching);
    }
}
