using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeatherController : MonoBehaviour
{
    private DistanceJoint2D joint;              // 
    private Rigidbody2D body;                   // Used to move the hook to different places.
    private PlayerController player;            // Script reference for the player
    private bool retracting;                    // Is the teather currently retracting
    private bool contact;                       // Has the teather made contact with a grappable surface?
    private float distance;                     // Current distance of tether from player
    private float facing;

    public float deployAngle;                   // Tether deploy angle relative to the character's front
    public float speed;                         // Teather movement speed
    public float teatherRange;                  // Max travel distance
    public float swingRange;                    // Max swing range
    public float swingSpeed;                    // Speed of swinging


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        joint = player.GetComponent<DistanceJoint2D>();
        joint.enabled = false;

        Vector3 targetVelocity;
        
        if (!player.up)
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
        if (distance > teatherRange)
            retracting = true;

        distance = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.y - player.transform.position.y, 2));
    }

    void FixedUpdate()
    {
        if (retracting)
        {
            Retract();
        }
        else if (contact)
        {
            // Swing player
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Terrain" && other.GetComponent<Collider2D>().gameObject.GetComponent<Rigidbody2D>() /*&& other.gameObject.Grappable*/ && !contact && !retracting)
        {
            body.velocity = Vector2.zero;
            contact = true;
            joint.connectedAnchor = new Vector2(transform.position.x, transform.position.y);
            joint.enabled = true;
            joint.connectedBody = other.GetComponent<Collider2D>().gameObject.GetComponent<Rigidbody2D>();
            joint.distance = teatherRange;
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
        joint.enabled = false;
        if (distance != 0)
        {
            float xT = (transform.position.x - player.transform.position.x) / Mathf.Abs(transform.position.x - player.transform.position.x);
            float yT = (transform.position.y - player.transform.position.y) / Mathf.Abs(transform.position.y - player.transform.position.y);
            float x = Mathf.Abs(transform.position.x - player.transform.position.x) / distance * xT;
            float y = Mathf.Abs(transform.position.y - player.transform.position.y) / distance * yT;
            body.velocity = new Vector3(-x, -y, 0) * speed;
        }
    }
}
