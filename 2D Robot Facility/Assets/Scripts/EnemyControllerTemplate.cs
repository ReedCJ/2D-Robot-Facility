using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerTemplate : MonoBehaviour
{
    protected GameObject player;
    protected GameObject gameController;
    protected Rigidbody2D body;
    
    public float floorCheckDistance;
    public float jumpRange;
    public float landingCheckDistance;
    public float moveSpeed;
    public float aggroRange;
    public float aggroLeash;
    public float levelCheckHeight;
    public bool randomMovement;

    protected float height;
    protected bool aggro;

    protected Vector2 moveVector;

    //true = right;
    protected bool facing;

    protected Quaternion faceLeft;
    protected Quaternion faceRight;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //get rigidbody
        body = GetComponent<Rigidbody2D>();
        //get player
        player = GameObject.FindWithTag("Player");
        //get gamecontroller
        gameController = GameObject.FindWithTag("GameController");
        //Set movement for enemies that move
        setMovement();
    }

    protected virtual void Update()
    {
    }

    //timer property from gamecontroller
    protected float Timer
    {
        get { return gameController.GetComponent<GameController>().timer; }
    }
    //distance to the player
    protected float DistanceToPlayer
    {
        get { return Vector2.Distance(body.transform.position, player.transform.position); }
    }
    protected float HorizontalDistanceToPlayer
    {
        get
        {
            if (PlayerToTheRight)
            {
                return player.transform.position.x - body.transform.position.x; 
            }
            else
            {
                return body.transform.position.x - player.transform.position.x;
            }
        }
    }
    //is the player to the right property
    public bool PlayerToTheRight
    {
        get
        {
            //Debug.Log(player.transform.position.x);
            //Debug.Log(body.transform.position.x);
            return player.transform.position.x > body.transform.position.x;
        }
    }

    //jump method
    protected virtual void Jump(float speed, float height)
    {
        body.velocity = new Vector2(0, 0);
        Vector2 debugVector = new Vector2(speed * (1 + (Random.Range(0, jumpRange) / 10)), height * (1 + (Random.Range(0, jumpRange) / 10)));
        body.AddForce(debugVector);
        //Debug.Log(debugVector);
    }

    //check if player is within a certain y range relative to the enemy
    protected bool PlayerOnLevel()
    {
        return player.transform.position.y < body.transform.position.y + levelCheckHeight && player.transform.position.y > body.transform.position.y - levelCheckHeight;
    }

    //check whther the jump will land somewhere
    protected bool LandingSpotExists(bool facing)
    {
        //get distance to check at
        float lcd = landingCheckDistance;
        //negative if facing left
        if (!facing) { lcd *= -1.0f; }
        //get the position you will check from
        Vector2 checkPosition = new Vector2(gameObject.transform.position.x + lcd, gameObject.transform.position.y);
        //raycast down
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, 3.0f);
        //debug
        //Debug.DrawRay(checkPosition, Vector2.down * 3, Color.red, 1.0f);
        //return true if hit collider isn't null
        return hit.collider != null;
    }

    //check whether there is floor where you are going
    protected bool ThereIsFloor()
    {
        //get distance to check at
        float fcd = floorCheckDistance;
        //negative if facing left
        if (!facing) { fcd *= -1.0f; }
        //get the position you will check from
        Vector2 checkPosition = new Vector2(gameObject.transform.position.x + fcd, gameObject.transform.position.y);
        //raycast down 3 at the distance specified
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, 3.0f);
        //debug
        //Debug.DrawRay(checkPosition, Vector2.down * 3, Color.red, 1.0f);
        //return true if hit collider isn't null
        return hit.collider != null;
    }

    //sets the movement of an enemy
    protected void setMovement()
    {
        moveVector = new Vector2(moveSpeed, body.velocity.y);
        if(!facing) { moveVector.x *= -1.0f; }
    }

    //moves enemies around
    protected void MoveAround()
    {
        body.velocity = moveVector;
        FaceMovement();
    }
    //makes the enemy face the correct direction for movement
    protected void FaceMovement()
    {
        if(moveVector.x < 0 && facing || moveVector.x > 0 && !facing)
        {
            FlipAround();
        }
    }
    //make the enemy face the player
    protected void FacePlayer()
    {
        if(PlayerToTheRight && !facing || !PlayerToTheRight && facing)
        {
            FlipAround();
        }
    }

    protected void FlipAround()
    {
        body.transform.Rotate(0, 180, 0, 0);
        facing = !facing;
    }
    
    //stop sldiing
    protected void StopSliding()
    {
        body.velocity = new Vector2(0, body.velocity.y);
    }

    //get angle to player
    protected float GetAngleToPlayer()
    {
        return (-Vector2.Angle(body.transform.position, player.transform.position));
    }

}
