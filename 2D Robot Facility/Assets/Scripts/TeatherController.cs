using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeatherController : MonoBehaviour
{
    private DistanceJoint2D joint;              //
    private Rigidbody2D body;                   // Used to move the hook to different places.
    private Rigidbody2D playerBody;             // Reference to player body to apply swinging physics
    private PlayerController player;            // Script reference for the player
    private bool retracting;                    // Is the teather currently retracting?
    private bool contact;                       // Is the hook currently touching a grappable surface?
    private float distance;                     // Current distance of tether from player
    private bool m_FacingRight;                 // Used for flipping the player
    private float currentSwing;                 // Current swing range, is steadily reduced to max when above max
    private float currentSpeed;                 // Current speed of swing

    public float deployAngle;                   // Tether deploy angle relative to the character's front
    public float speed;                         // Teather movement speed
    public float teatherRange;                  // Max travel distance
    public float maxSwing;                      // Max swing range
    public float minSwing;                      // Min swing range
    public float swingSpeed;                    // Speed of swinging
    public float pushRange;                     // Max Swing Height
    public float climbSpeed;                    // Speed of retracting grapple
    public float pullRate;                      // Rate that the grapple will pull the player into range in
    public float swingAccel;                    // Rate of swing speed increase


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerBody = player.GetComponent<Rigidbody2D>();
        joint = player.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        contact = false;
        m_FacingRight = player.facing;

        Vector3 targetVelocity;
        float facing;
        
        if (!player.up)     // spawn grappling hook in direction player is facing or upwards if the player is holding the UP input
        {
            if (player.facing)
                facing = 1;
            else
                facing = -1;
            transform.Rotate(0.0f, 0.0f, deployAngle, Space.Self);
            targetVelocity = new Vector2(Mathf.Cos(deployAngle / 180 * Mathf.PI) * facing, Mathf.Sin(deployAngle / 180 * Mathf.PI));
        }
        else
        {
            transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
            targetVelocity = new Vector2(0.0f, 1.0f);
        }
        body.velocity = targetVelocity * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Teather"))
            retracting = true;
        if (distance > teatherRange && !contact)
            retracting = true;

        distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
                new Vector2(transform.position.x, transform.position.y));
    }

    void FixedUpdate()
    {
        if (retracting)
        {
            Retract();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Terrain" && other.GetComponent<Collider2D>().gameObject.GetComponent<Rigidbody2D>() /*&& other.gameObject.Grappable*/ && !contact && !retracting)
        {
            if ((player.transform.position.x > transform.position.x && m_FacingRight)
                || player.transform.position.x < transform.position.x && !m_FacingRight)
                Flip();

            body.velocity = Vector2.zero;

            joint.distance = distance;
            currentSwing = distance;
            joint.connectedBody = body;
            joint.enabled = true;
            
            contact = true;
            player.swinging = true;
            player.teatherSwinging = true;
        }
        else if (other.gameObject.tag == "Terrain")
        {
            retracting = true;
        }
        else if (retracting && other.gameObject.tag == "Player")
        {
            player.teatherOut = false;
            Destroy(gameObject);
        }
    }

    void Retract()
    {
        if (contact)      // If the grappling hook made contact with a valid surface, release it, disable it, and default related variables
        {
            joint.enabled = false;
            contact = false;
            player.swinging = false;
            player.teatherSwinging = false;
        }

        if (distance != 0)      // Move towards the player's current location
        {
            float xT = (transform.position.x - player.transform.position.x) / Mathf.Abs(transform.position.x - player.transform.position.x);
            float yT = (transform.position.y - player.transform.position.y) / Mathf.Abs(transform.position.y - player.transform.position.y);
            float x = Mathf.Abs(transform.position.x - player.transform.position.x) / distance * xT;
            float y = Mathf.Abs(transform.position.y - player.transform.position.y) / distance * yT;
            body.velocity = new Vector3(-x, -y, 0) * speed;
        }
    }

    public void StartRetracting()
    {
        retracting = true;
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
            if (player.vMove > 0)
            {
                currentSwing = currentSwing - climbSpeed * player.vMove * Time.fixedDeltaTime;
                if (currentSwing < minSwing)
                    currentSwing = minSwing;
            }
            else if (player.vMove < 0)
            {
                currentSwing = currentSwing - climbSpeed * player.vMove * Time.fixedDeltaTime;
                if (currentSwing > maxSwing)
                    currentSwing = maxSwing;
            }

            if (player.transform.position.y >= -pushRange + transform.position.y
                && player.transform.position.x <= transform.position.x)
            {
                 Vector2 destination = new Vector2(transform.position.x - distance - player.transform.position.x,
                     transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed;
                playerBody.AddForce(destination);
            }
            else if (player.transform.position.y >= -pushRange + transform.position.y
                && player.transform.position.x > transform.position.x)
            {
                Vector2 destination = new Vector2(transform.position.x + distance - player.transform.position.x,
                    transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed;
                playerBody.AddForce(destination);
            }
            else if (player.hMove > 0)
            {
                if (player.transform.position.x < transform.position.x)
                {
                    Vector2 destination = new Vector2(transform.position.x - player.transform.position.x,
                        transform.position.y - distance - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed * 2;
                    playerBody.AddForce(destination);
                }
                else
                {
                    Vector2 destination = new Vector2(transform.position.x + distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed * 2;
                    playerBody.AddForce(destination);
                }
            }
            else if (player.hMove < 0)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    Vector2 destination = new Vector2(transform.position.x - player.transform.position.x,
                        transform.position.y - distance - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed;
                    playerBody.AddForce(destination);
                }
                else
                {
                    Vector2 destination = new Vector2(transform.position.x - distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed;
                    playerBody.AddForce(destination);
                }
            }
            else
            {
                if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
                new Vector2(transform.position.x, transform.position.y - distance * 2)) > 1)
                {
                    Vector2 destination = new Vector2(transform.position.x - distance - player.transform.position.x,
                        transform.position.y - player.transform.position.y) * Time.fixedDeltaTime * swingSpeed * .25f;
                    playerBody.AddForce(destination);
                }
                else
                {
                    playerBody.velocity = playerBody.velocity * .95f;
                }
            }
        }
        else if (Input.GetButtonDown("Focus"))
        {
            // Aim weapon
        }
        
        joint.distance = currentSwing;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        player.facing = m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = player.transform.localScale;
        theScale.x *= -1;
        player.transform.localScale = theScale;
    }
}
