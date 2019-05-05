using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeatherController : MonoBehaviour
{
    private DistanceJoint2D joint;              //
    private Rigidbody2D body;                   // Used to move the hook to different places.
    private Rigidbody2D playerBody;             // Reference to player body to apply swinging physics
    private PlayerController player;            // Script reference for the player
    public bool retracting;                    // Is the teather currently retracting?
    private bool extending;                     // Is the teather currently extending?
    private bool contact;                       // Is the hook currently touching a grappable surface?
    private bool belowAnchor;                   // Is the player below the anchor?
    private float distance;                     // Current distance of teather from player
    private bool m_FacingRight;                 // Used for flipping the player, true == right
    private float currentSwing;                 // Current swing range, is steadily reduced to max when above max
    private float currentSpeed;                 // Current speed of swing
    private Vector3 teatherVelocity;            // Base direction of grapple

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


    // Start is called before the first frame update
    void Start()
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

        Shift();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Teather"))                 // Input to retract teather
            retracting = true;
        else if (distance > teatherRange && !contact)       // Start retracting after reaching the maximum teatherRange
            retracting = true;
        if (transform.position.y < player.transform.position.y)
            belowAnchor = false;
        else belowAnchor = true;

        distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
                new Vector2(transform.position.x, transform.position.y));       /// Calculate the current distance between the hook and the player

        if (playerBody.velocity.x > 0 && !m_FacingRight && contact && belowAnchor)      // Flip the player if the hook is attached, and he is moving right, but facing left
            Flip();
        else if (playerBody.velocity.x < 0 && m_FacingRight && contact && belowAnchor)  // Flip the player if the hook is attached, and he is moving left, but facing right
            Flip();
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
            if ((player.transform.position.x > transform.position.x && m_FacingRight)
                || player.transform.position.x < transform.position.x && !m_FacingRight)
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
        else if (other.gameObject.tag == "Terrain" && contact == false)     // Grapple hook starts retracting if the colliding surface is invalid
        {
            retracting = true;
        }
        else if (retracting && other.gameObject.tag == "Player")            // Grapplehook is removed from the game when it comes back to the player
        {
            player.teatherOut = false;
            Destroy(gameObject);
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
            float xT = (transform.position.x - player.transform.position.x) / Mathf.Abs(transform.position.x - player.transform.position.x);
            float yT = (transform.position.y - player.transform.position.y) / Mathf.Abs(transform.position.y - player.transform.position.y);
            float x = Mathf.Abs(transform.position.x - player.transform.position.x) / distance * xT;
            float y = Mathf.Abs(transform.position.y - player.transform.position.y) / distance * yT;
            body.velocity = new Vector2(-x, -y) * speed;
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


            float currentSpeed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
            
            if (player.transform.position.y >= -pushRange + transform.position.y        // Is the player too high? Push him towards the center
                && player.transform.position.x <= transform.position.x)
            {
                Accelerate(new Vector2(transform.position.x - distance - player.transform.position.x,
                     transform.position.y - player.transform.position.y));
            }
            else if (player.transform.position.y >= -pushRange + transform.position.y   // Is the player too high? Push him towards the center
                && player.transform.position.x > transform.position.x)
            {
                Accelerate(new Vector2(transform.position.x + distance - player.transform.position.x,
                    transform.position.y - player.transform.position.y));
            }
            else if (player.hMove > 0 && currentSpeed < maxSwingSpeed)                  // If the player is not too high, read his input and accelerate him up to the max
            {
                if (player.transform.position.x < transform.position.x)
                {
                    Accelerate(new Vector2(transform.position.x - player.transform.position.x,
                        transform.position.y - distance - player.transform.position.y));
                }
                else
                {
                    Accelerate(new Vector2(transform.position.x + distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y));
                }
            }
            else if (player.hMove < 0 && currentSpeed < maxSwingSpeed)                  // If the player is not too high, read his input and accelerate him up to the max
            {
                if (player.transform.position.x > transform.position.x)
                {
                    Accelerate(new Vector2(transform.position.x - player.transform.position.x,
                        transform.position.y - distance - player.transform.position.y));
                }
                else
                {
                    Accelerate(new Vector2(transform.position.x - distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y));
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

        player.transform.Rotate(0.0f, 180.0f, 0, Space.Self);
    }

    private void Accelerate(Vector2 destination)        // Accelerate player towards a direction
    {
        playerBody.AddForce(destination * Time.fixedDeltaTime * accel);
    }

    private void SlowDown()             // Called to gradually slow the grappling player down
    {
        float speed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
        Vector2 destination;

        if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
        new Vector2(transform.position.x, transform.position.y - distance)) > 2)
        {
            destination = new Vector2(transform.position.x - distance - player.transform.position.x,
                transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * accel * .25f;
            playerBody.AddForce(destination);
        }
        else if (speed > .4f)
        {
            playerBody.velocity = playerBody.velocity * .98f;
        }
        else if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
        new Vector2(transform.position.x, transform.position.y - distance)) < .25f)
            playerBody.velocity = new Vector2(0, 0);
    }

    private void SlowToMaxSpeed()              // Set Velocity to the max speed if it is over the max speed
    {
        float curSpeed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
        if (currentSpeed > maxSwingSpeed)
        {
            float speed = Mathf.Sqrt(Mathf.Pow(playerBody.velocity.x, 2) + Mathf.Pow(playerBody.velocity.y, 2));
            playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y) / speed * maxSwingSpeed;
        }
    }
}
