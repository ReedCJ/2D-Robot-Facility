using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject hook;           // The "hook" in the grappling hook
    [SerializeField] private GameObject shot;           // Our player's projectile
    [SerializeField] private Transform[] shotSpawns;    // The locations our player's projectile can spawn from

    [SerializeField] private float speed = 0.0f;   // Character ground speed
#pragma warning restore 0649

    private float timer;
    
    private bool fire;                      // Attack key input
    public float fireRate;                  // How fast you can shoot
    private float lf;                       //last time you fired
    private float nextFire;                 //counter for fire rate

    public Transform teatherSpawn;
    private bool teather;                   // Teather key input
    [System.NonSerialized] public bool jump;        // Jump key input
    [System.NonSerialized] public bool canDouble;   // bool for being able to double dump
    private bool doubleJump;                // double jump bool
    private bool camFollow;                 // Camera is in follow mode?

    private bool canTeleport;               //Are you on a teleporter?
    private GameObject teleporter;          //What teleporter are you on?
    private bool teleport;                  //Are you teleporting?
    private float lt;                       //When did you last teleport?

    private GameObject GrappleHook;         // Active Grappling Hook Object
    static public Animator animator;
    public CharacterController2D controller;      // The script that processes our movement inputs
    [System.NonSerialized] public TeatherController grappleController;  // Script for swinging player
    [System.NonSerialized] public Rigidbody2D body;
    [System.NonSerialized] public float hMove = 0.0f;                   // Ground movement
    [System.NonSerialized] public float vMove = 0.0f;                   // Vertical Input and climbing
    [System.NonSerialized] public bool teatherOut;                      // Grappling hook deployed?
    [System.NonSerialized] public bool swinging;                        // Currently swinging?
    [System.NonSerialized] public bool teatherSwinging;                 // Currently swinging?
    [System.NonSerialized] public bool facing;                          // True = right, False = left
    [System.NonSerialized] public bool fallThrough;                     // Can the player currently fall through thin platforms?
    [System.NonSerialized] public bool jumpThrough;                     // Can the player currently jump through thin platforms?
    [System.NonSerialized] public bool thinGround;                      // Is the player on top of ground he can fall through?
    [System.NonSerialized] public bool crouch;                          // Is player crouched
    [System.NonSerialized] public bool focusing;                        // Is the player locking his movement to aim?
    [System.NonSerialized] public bool up;                              // Up Input
    [System.NonSerialized] public bool down;                            // Down Input
    [System.NonSerialized] public bool grounded;                        // On the ground as opposed to in the air?

    private IEnumerator coroutine;
    private float waitTime;
    GameObject playerModel;
    private Renderer rend;
    public bool dead;
    public GameOver gameOverUI;
    public PlayerHealth playerHealth;
    public bool confined;
    public bool knockdown;
    public bool contactRight;
    public bool contactTop;

    private AudioManager audio;


    //Run when player is created
    void Start()
    {
        
        focusing = false;
        knockdown = false;
        thinGround = false;
        //Player starts facing right
        facing = true;
        //Player has not double jumped
        doubleJump = false;
        //assign rigidbody to variable
        body = GetComponent<Rigidbody2D>();
        SetInitialState();
        animator = GetComponentInChildren<Animator>();

        //aquires the model to flash for invulnerability effect
        playerModel = this.transform.GetChild(2).GetChild(1).gameObject;
        rend = playerModel.GetComponent<SkinnedMeshRenderer>();

        //resets death bools after restart just in case
        dead = false;
        animator.SetBool("Death", false);
    }

    void SetInitialState()      // Sets variables 
    {
        audio = FindObjectOfType<AudioManager>();
        camFollow = true;
    }

    // Update is called once per frame
    void Update()
    {
        //timer
        timer += Time.deltaTime;

        //Checks for invulnerable to display effect
        if (!dead && playerHealth.invuln)
            StartCoroutine("Blink");
       
        if (!MainMenu.isPaused && !dead && !knockdown)
        {
            #region Keys
            hMove = Input.GetAxisRaw("Horizontal");
            vMove = Input.GetAxisRaw("Vertical");

            //Sets animation parameter for speed (Movement)
            animator.SetFloat("Velocity_x", body.velocity.x);
            animator.SetFloat("Speed", Mathf.Abs(hMove));
            animator.SetFloat("Vertical_f", (vMove));
            animator.SetFloat("Horizontal_f", Mathf.Abs(hMove));

            //Plays audio for character movement
            if (grounded && !crouch && Math.Abs(hMove) > .5)
            {
                if (audio != null)
                {
                    if (!audio.sounds[15].source.isPlaying)
                        audio.Play("Running");
                    if (audio.sounds[16].source.isPlaying)
                        audio.Stop("Walking");
                }
            }
            
            else if (grounded && crouch && (Math.Abs(hMove) >= .1) || Math.Abs(hMove) <= .5 && Math.Abs(hMove) >= .1)
            {
                if (audio != null)
                {
                    if(!audio.sounds[16].source.isPlaying)
                        audio.Play("Walking");
                    if (audio.sounds[15].source.isPlaying)
                        audio.Stop("Running");
                }
                    
            }
            else
            {
                if (audio != null)
                {
                    audio.Stop("Running");
                    audio.Stop("Walking");
                }
                    
            }

                //Checks if falling parameter
                animator.SetFloat("Falling", body.velocity.y);
            if (body.velocity.y < -2)
                animator.SetBool("Jumping", false);


            if (Input.GetButtonDown("Jump"))
            {
                if (grounded && !confined)
                {
                      jump = true;
                      animator.SetBool("Jumping", true);
                      animator.SetBool("Grounded", false);
                }
                //double jump
                else if (!grounded && canDouble || swinging)
                {
                    animator.SetTrigger("DoubleJumping");
                    animator.SetBool("Jumping", true);
                    animator.SetBool("Grounded", false);
                    doubleJump = true;
                    canDouble = false;
                }
            }

            if(Input.GetButtonDown("Interact"))
            {
                if(grounded && canTeleport && timer > lt + 0.5f)
                {
                    teleport = true;
                }
            }

            if (Input.GetButtonUp("Jump") && !grounded)     // Short hop code
            {
                if (body.velocity.y > 0)
                    body.velocity = new Vector2(body.velocity.x, body.velocity.y * .5f);
            }

            // look/aim up
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                up = true;
            }
            else
            {
                up = false;
            }

            // Aim/look down / crouch
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                down = true;
            }
            else
            {
                down = false;
            }


            if (down && grounded)
            {
                crouch = true;
            }
            else
            {
                crouch = false;
            }

            if (Input.GetButtonDown("Teather"))
            {
                if(!confined)
                {
                    teather = true;
                    animator.SetLayerWeight(1, 1);
                    // StartCoroutine("TetherTorso");
                    animator.SetTrigger("SwingStart");
                    if (audio != null)
                    audio.Play("Tether");
                }
               
            }

            animator.SetBool("Swinging", swinging);
            MoveThroughPlatform();

            //Attack button press/release
            if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Fire1"))
            {
                animator.SetBool("Shooting",true);
                fire = true;
            }
            else if (Input.GetButtonUp("Attack") || Input.GetButtonUp("Fire1"))
            {
                animator.SetBool("Shooting", false);
                fire = false;
            }

            if (Input.GetButton("Focus"))
                focusing = true;
            else focusing = false;

            #endregion
        }
    }


    void FixedUpdate()
    {
        //
        // Movement input && grapple input processing block
        //

        if (focusing && !swinging)
        {
            controller.Move(0, crouch, false, false);
        }
        if (!swinging && !fallThrough)
        {
            controller.Move(hMove * speed * Time.fixedDeltaTime, crouch, jump, doubleJump);
        }
        else if (swinging && doubleJump)
        {
            controller.Move(hMove * speed * Time.fixedDeltaTime, crouch, jump, doubleJump);
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
        //Reset double jump if player is grounded, or is swinging 
        if (grounded) { canDouble = true; }

        //Fire if enough time has passed between shots and fire button is pressed
        if (fire && timer > lf + fireRate)
        {
            Attack();
            lf = timer;
        }

        if(teleport)
        {
            if (audio != null)
                audio.Play("Teleport");
            this.gameObject.transform.position = teleporter.GetComponent<TeleporterController>().Destination.transform.GetChild(0).gameObject.transform.position;
            lt = timer;
        }

        //If grapple key is pressed
        if (teather) { CastTeather(); }

        //reset bools at the end of a FixedUpdate
        jump = false;
        doubleJump = false;
        teather = false;
        teleport = false;
        //animator.SetBool("Swinging", false);
    }

    void CastTeather()                  // Deploys grapple
    {
        if (!teatherOut && !swinging)
        {
            teatherOut = true;
            GrappleHook = Instantiate(hook, teatherSpawn.position, Quaternion.identity);
            grappleController = GrappleHook.GetComponent<TeatherController>();
        }
    }

    void Attack()
    {
        Transform shotSpawn = GetShotSpawn();
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }

    public bool CanTeleport
    {
        get { return canTeleport; }
        set { canTeleport = value; }
    }

    public GameObject Teleporter
    {
        get { return teleporter; }
        set { teleporter = value; }
    }

    private Transform GetShotSpawn()
    {
        //if up is held always shoot up
        if (swinging && !focusing)
            return shotSpawns[0];
        else if(up)
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                return shotSpawns[2];
            }
            else
                return shotSpawns[1];
        }
        else if (down)
        {
            //if crouch in air
            if (!grounded || focusing)
            {
                if (Input.GetAxisRaw("Horizontal") == 0)
                    return shotSpawns[4];
                else
                    return shotSpawns[3];
            }
            else
                return shotSpawns[0];
        }
        else
            return shotSpawns[0];
    }


    public void OnLanding ()
    {
        //Debug.Log("Landed");
        animator.SetTrigger("Landing");
        animator.SetBool("Jumping", false);
        animator.SetBool("Grounded", true);
        if (audio != null)
            audio.Play("Landing");

    }

    public void OnCrouching (bool isCrouching)
    {
        animator.SetBool("Crouching", isCrouching);
    }
    
    public void MoveThroughPlatform()       // Updates fallThrough and jumpThrough status.
    {
        //Debug.Log(" " + grounded + " " + crouch + " " + jump + " " + thinGround + " " + (body.velocity.y == 0.0f));
        if ((grounded && crouch && jump && thinGround && body.velocity.y == 0.0f))
        {
            fallThrough = true;
        }
        else if (!grounded && body.velocity.y > 0 || (jump && !thinGround) || doubleJump)
        {
            jumpThrough = true;
            fallThrough = false;
        }
        else
            jumpThrough = false;
    }
    
   
    //plays contact animation - triggered from contactDamage script
    public void contactAnimate()
    {
        // Debug.Log("Contact");
        if (audio != null)
            audio.Play("Damage");
        animator.SetTrigger("Contact");
        if (audio != null && !dead)
            audio.Play("Invulnerable");
    }

    //plays contact animation - triggered from contactDamageCharger script
    //parameter is a bool "right" side = true, false is left side of player.
    public void contactAnimateCharger()
    {
        //knockdown bool for locking input while stunned
        knockdown = true;

        //animation parameter to disable alternate animations
        animator.SetBool("Knockdown", true);
        if (audio != null)
        {
            if (!dead)
            {
                audio.Play("Knockdown");
                audio.Play("Invulnerable");
            }
            
        }
            
        //coroutine to reenable parameters/variables after animation
        StartCoroutine("knockeddown");

        //continues invulnerable for duration of stun
        playerHealth.invuln = true;

        //resets input so it doesn't become locked during animation
        
        fire = false;
        hMove = 0;
        vMove = 0;

        //Plays corresponding animations for direction of enemy contact.
        if (controller.m_FacingRight)
        {
            if(contactRight)
            {
                //Debug.Log("Hit on R facing R");
                animator.SetTrigger("KnockedBack");
                controller.Move(-2 * speed * Time.fixedDeltaTime * 10, crouch, jump, doubleJump);
                controller.Flip();
            }
            else
            {
                //Debug.Log("Hit on L facing R");
                animator.SetTrigger("KnockedForward");
                controller.Move(2 * speed * Time.fixedDeltaTime * 10, crouch, jump, doubleJump);
                //controller.Flip();
            }

        }

        else if (!controller.m_FacingRight)
        {
            if(contactRight)
            {
                //Debug.Log("Hit on R facing L");
                animator.SetTrigger("KnockedForward");
                controller.Move(-2 * speed * Time.fixedDeltaTime * 10, crouch, jump, doubleJump);
                //controller.Flip();

            }
            else
            {
                //Debug.Log("Hit on L facing L");
                animator.SetTrigger("KnockedBack");
                controller.Move(2 * speed * Time.fixedDeltaTime * 10, crouch, jump, doubleJump);
                controller.Flip();
            }
            
        }
    }

    IEnumerator knockeddown()
    {
        yield return new WaitForSeconds(2f);
        //facing = !facing;
        //controller.Flip();
        //controller.m_FacingRight = !controller.m_FacingRight;
        knockdown = false;
        animator.SetBool("Knockdown", false);
        playerHealth.invuln = false;

    }

    IEnumerator Blink()
    {
        while (playerHealth.invuln && !dead)
        {
            if (rend.enabled)
                rend.enabled = false;
            else
                rend.enabled = true;
            yield return new WaitForSeconds(.2f);
        }
        if (audio != null)
            audio.Stop("Invulnerable");
        rend.enabled = true;
    }

    //Plays death animation and starts GameoverUI - Triggered from contactDamage Script
    public void playerDeath()
    {
        //Sets playercontroller bool to dead - disables inputs to character controller
        dead = true;

        //Ensures input is reset to 0 so once player controller is disabled the player doesnt lock in movement

        hMove = 0;
        vMove = 0;
        fire = false;

        //Plays Death animation and disables all other animation events
        animator.SetBool("Death", true);

        //Start GameOverUI
        if (gameOverUI != null)
            gameOverUI.gameOver();
        else Debug.Log("You need to attach inGameUI to PlayerController");
    }

    public void playerDeathFall()
    {
        //Sets playercontroller bool to dead - disables inputs to character controller
        dead = true;

        //Ensures input is reset to 0 so once player controller is disabled the player doesnt lock in movement
        hMove = 0;
        vMove = 0;

        //Plays Death animation and disables all other animation events
        animator.SetBool("DeathFall", true);

        //Start GameOverUI
        if (gameOverUI != null)
            gameOverUI.gameOver();
        else Debug.Log("You need to attach inGameUI to PlayerController");
    }

}
