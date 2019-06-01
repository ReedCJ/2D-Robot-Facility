using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeatherController : MonoBehaviour
{
    private DistanceJoint2D joint;              //  Used to keep the player at a specific distance from the "hook"
    private Rigidbody2D body;                   // Used to move the hook to different places.
    private Rigidbody2D playerBody;             // Reference to player body to apply swinging physics
    private PlayerController player;            // Script reference for the player
    [SerializeField] private GameObject teatherPrefab;  // The teather object prefab
    private GameObject teather;                 // The teather object

    private bool retracting;                    // Is the teather currently retracting?
    private bool extending;                     // Is the teather currently extending?
    private bool contact;                       // Is the hook currently touching a grappable surface?
    private bool belowAnchor;                   // Is the player below the anchor?
    private float distance;                     // Current distance of teather from player
    private float angle;                        // Current angle of the player relative to downwards line from the grapple
    private bool m_FacingRight;                 // Used for flipping the player, true == right
    private float currentSwing;                 // Current swing range, is steadily reduced to max when above max
    private float currentSpeed;                 // Current speed of swing
    private Vector3 teatherVelocity;            // Base direction of grapple


    public float gravUp;                        // Rate of gravity increase.
    public float deployAngle;                   // Tether deploy angle relative to the character's front in degrees
    public float speed;                         // Teather movement speed
    public float teatherRange;                  // Max travel distance
    public float maxSwing;                      // Max swing range
    public float minSwing;                      // Min swing range
    public float accel;                         // Speed of swinging
    public float maxSwingSpeed;                 // Maximum Swinging speed
    public float pushRange;                     // Max Swing Height
    public float climbSpeed;                    // Speed of retracting grapple
    public float pullRate;                      // Rate that the grapple will pull the player into range in


    void Start()                                // Setup variables, set initial teather velocity
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerBody = player.GetComponent<Rigidbody2D>();
        joint = player.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        contact = false;
        retracting = false;
        extending = true;

        float facing;

        if (player.facing)
            facing = 1;
        else
            facing = -1;
        transform.Rotate(0.0f, 0.0f, deployAngle, Space.Self);
        teatherVelocity = new Vector3(Mathf.Cos(deployAngle / 180 * Mathf.PI) * facing, Mathf.Sin(deployAngle / 180 * Mathf.PI), 0.0f) * speed;

        teather = Instantiate(teatherPrefab, player.teatherSpawn.transform.position, transform.rotation);
        Shift();
        Debug.Log("Starting");
    }

    // Update is called once per frame
    void Update()                   // Track the current distance, the current angle, and the player's current speed
    {
        DistanceAngleSpeed();

        if (Mathf.Abs(angle) < pushRange && playerBody.gravityScale > 4)        // Reset gravity after the player swings back down to a normal range.
        {
            playerBody.gravityScale = 4;
        }

        if (Input.GetButtonDown("Teather") || (Input.GetButtonDown("Jump") && contact) || player.knockdown || player.dead)                 // Input to retract teather
            retracting = true;
        else if (distance > teatherRange && !contact)       // Start retracting after reaching the maximum teatherRange
            retracting = true;
        if (transform.position.y < player.teatherSpawn.transform.position.y)
            belowAnchor = false;
        else belowAnchor = true;

        if (playerBody.velocity.x > 0 && !m_FacingRight && contact && belowAnchor)      // Flip the player if the hook is attached, and he is moving right, but facing left
            Flip();
        else if (playerBody.velocity.x < 0 && m_FacingRight && contact && belowAnchor)  // Flip the player if the hook is attached, and he is moving left, but facing right
            Flip();

        // Position, scale, and rotate the teather into position
        SetRopePositionRotation();
    }

    void FixedUpdate()
    {
        if (extending)
            Shift();
        if (retracting)
            Retract();
    }

    void OnTriggerEnter2D(Collider2D other)     // What happens when the grappling hook collides with a valid surface
    {
        if (other.gameObject.tag == "Terrain" && other.GetComponent<Collider2D>().gameObject.GetComponent<Rigidbody2D>()
            && other.gameObject.GetComponent<PlatformController>() != null && other.gameObject.GetComponent<PlatformController>().grappable && !contact && !retracting)
        {
            m_FacingRight = player.facing;
            if ((player.teatherSpawn.transform.position.x > transform.position.x && m_FacingRight)           // Face the player towards the grapple
                || player.teatherSpawn. transform.position.x < transform.position.x && !m_FacingRight)
                Flip();

            body.velocity = Vector2.zero;       // Stop moving the grappling hook

            joint.distance = distance;          // Setup the joint with current distance, shrinks to maximize size later
            currentSwing = distance;
            joint.connectedBody = body;
            joint.enabled = true;

            extending = false;                  // Manage bool variables
            contact = true;
            player.swinging = true;
            player.teatherSwinging = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (retracting && other.gameObject.tag == "Teather Spawn")            // Grapplehook is removed from the game when it comes back to the player
        {
            player.teatherOut = false;
            Destroy(teather);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Terrain" && contact == false)     // Grapple hook starts retracting if the colliding surface is invalid
        {
            retracting = true;
        }
    }

    void Shift()        // Shift the X velocity of the grappling hook to be equal to the base x velocity of the hook, + the player's x velocity, causes hook to move with the player.
    {
        body.velocity = new Vector3(teatherVelocity.x + playerBody.velocity.x, teatherVelocity.y, teatherVelocity.z);
    }

    public void Retract()
    {
        if (contact)      // If the grappling hook made contact with a valid surface, release it, disable it, and default related variables
        {
            joint.enabled = false;
            contact = false;
            extending = false;
            player.swinging = false;
            player.teatherSwinging = false;
        }

        if (distance != 0)      // Move towards the player's current location
        {
            float xT = (transform.position.x - player.teatherSpawn.transform.position.x) / Mathf.Abs(transform.position.x - player.teatherSpawn.transform.position.x);
            float yT = (transform.position.y - player.teatherSpawn.transform.position.y) / Mathf.Abs(transform.position.y - player.teatherSpawn.transform.position.y);
            float x = Mathf.Abs(transform.position.x - player.teatherSpawn.transform.position.x) / distance * xT;
            float y = Mathf.Abs(transform.position.y - player.teatherSpawn.transform.position.y) / distance * yT;
            body.velocity = new Vector2(-x, -y) * speed + playerBody.velocity;
        }
    }

    public void Swing()
    {
        if (currentSwing > maxSwing)      // If the grapple was created outside of the normal max swing length, steadily reduce to max length
        {
            currentSwing = currentSwing - pullRate * Time.fixedDeltaTime;
            if (currentSwing < maxSwing)
            {
                currentSwing = maxSwing;
            }
        }

        if (!Input.GetButtonDown("Focus"))
        {
            if (player.vMove > 0)           // Retract teather
            {
                currentSwing = currentSwing - climbSpeed * player.vMove * Time.fixedDeltaTime;
                if (currentSwing < minSwing)
                    currentSwing = minSwing;
            }
            else if (player.vMove < 0)      // Extend teather
            {
                currentSwing = currentSwing - climbSpeed * player.vMove * Time.fixedDeltaTime;
                if (currentSwing > maxSwing)
                    currentSwing = maxSwing;
            }

            if (angle <= -(pushRange - 20))        // Is the player too high? Push him towards the center
            {
                Deaccel();
            }
            else if (angle >= pushRange - 20)   // Is the player too high? Push him towards the center
            {
                Deaccel();
            }
            else if (player.hMove > 0 && currentSpeed < maxSwingSpeed)                  // If the player is not too high, read his input and accelerate him up to the max
            {
                if (player.transform.position.x < transform.position.x)
                {
                    Accelerate(new Vector2(transform.position.x - player.transform.position.x,
                        transform.position.y - distance - player.transform.position.y), 1);
                }
                else
                {
                    Accelerate(new Vector2(transform.position.x + distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y), 1);
                }
            }
            else if (player.hMove < 0 && currentSpeed < maxSwingSpeed)                  // If the player is not too high, read his input and accelerate him up to the max
            {
                if (player.transform.position.x > transform.position.x)
                {
                    Accelerate(new Vector2(transform.position.x - player.transform.position.x,
                        transform.position.y - distance - player.transform.position.y), 1);
                }
                else
                {
                    Accelerate(new Vector2(transform.position.x - distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y), 1);
                }
            }
            else if (player.hMove == 0)                     // If the player is not pressing any buttons, gradually slow him down
            {
                SlowDown();
            }
            SlowToMaxSpeed();
        }
        else if (Input.GetButtonDown("Focus"))
        {
            // Aim weapon
        }

        joint.distance = currentSwing;      // Update the current distance from the hook
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        player.facing = m_FacingRight;
        player.controller.m_FacingRight = m_FacingRight;

        player.transform.Rotate(0.0f, 180.0f, 0, Space.Self);
    }

    private void Accelerate(Vector2 destination, float rate)        // Accelerate player towards a direction
    {
        playerBody.AddForce(destination * Time.fixedDeltaTime * accel * rate);
    }

    private void SlowDown()             // Called to gradually slow the grappling player down
    {
        float curSpeed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
        Vector2 destination;

        if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
        new Vector2(transform.position.x, transform.position.y - distance)) > 2)
        {
            destination = new Vector2(transform.position.x - distance - player.transform.position.x,
                transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * accel * .25f;
            playerBody.AddForce(destination);
        }
        else if (curSpeed > .4f)
        {
            playerBody.velocity = playerBody.velocity * .98f;
        }
        else if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
        new Vector2(transform.position.x, transform.position.y - distance)) < .25f)
            playerBody.velocity = new Vector2(0, 0);
    }

    private void Deaccel()                  // If the player is moving up, increase the gravity and slow him  down further if he past the pushrange
    {                                       // Called when the player is nearing the push range
        if (playerBody.velocity.y > 0)
        {
            playerBody.gravityScale = playerBody.gravityScale + Time.fixedDeltaTime * gravUp;
            if (playerBody.velocity.y > 5 && Mathf.Abs(angle) > pushRange)
                playerBody.velocity = playerBody.velocity * .95f;
        }
    }

    private void SlowToMaxSpeed()           // Set Velocity to the max speed if it is over the max speed
    {
        if (currentSpeed > maxSwingSpeed)
        {
            float curSpeed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
            playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y) / curSpeed * maxSwingSpeed;
        }
    }

    private void DistanceAngleSpeed()       // Figures out the current distance from the player, the direction the teather is in and the speed of the player
    {
        distance = Vector2.Distance(new Vector2(player.teatherSpawn.transform.position.x, player.teatherSpawn.transform.position.y),
                new Vector2(transform.position.x, transform.position.y));
        angle = Vector2.SignedAngle(new Vector2(0, -distance), new Vector2(player.teatherSpawn.transform.position.x - transform.position.x, player.teatherSpawn.transform.position.y - transform.position.y));
        currentSpeed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
    }

    private void SetRopePositionRotation()  // Position the rope inbetween the teatherSpawn and the claw, rotate into that positon, scale the rope to fit that gap
    {
        teather.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        float xPos = player.teatherSpawn.position.x - (player.teatherSpawn.position.x - transform.position.x) / 2;
        float yPos = player.teatherSpawn.position.y - (player.teatherSpawn.position.y - transform.position.y) / 2;
        teather.transform.position = new Vector3(xPos, yPos, player.transform.position.z);
        teather.transform.localScale = new Vector3(teather.transform.localScale.x, distance / 2, teather.transform.localScale.z);
    }
}
