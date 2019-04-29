using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : EnemyControllerTemplate
{
    public float attackJumpspeed;
    public float attackJumpheight;
    public float attackJumpCD;
    public float moveJumpheight;
    public float moveJumpspeed;
    public float moveJumpCD;
    
    private float attackJumpleft;
    private float attackJumpright;
    private float moveJumpleft;
    private float moveJumpright;
    private float aJCD;
    private float mJCD;
    private float whenJumped;
    private float jumpedHeight;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackJumpright = attackJumpspeed;
        attackJumpleft = attackJumpspeed * -1.0f;
        moveJumpleft = moveJumpspeed * -1.0f;
        moveJumpright = moveJumpspeed;
        jumpedHeight = body.transform.position.y;
    }

    protected override void Update()
    {
        //jump around to move when the player is not in attack range, also don't jump immediately after attacking
        if (jumpedHeight + 0.003 > height && Timer > mJCD && Timer > 2.0f)
        {
            //add public cd value to the cd
            mJCD = Timer + moveJumpCD;
            if(randomMovement)
            {
                if(Random.Range(0,2) == 1)
                {
                    moveJumpspeed = moveJumpright;
                    //if facing left and you jump right flip around
                    if (!facing) { body.transform.Rotate(0, 180, 0, 0); facing = true; }
                }
                else
                {
                    moveJumpspeed = moveJumpleft;
                    //if facing right and you jump left flip around
                    if (facing) { body.transform.Rotate(0, 180, 0, 0); facing = false; }
                }
                //jump around if you aren't jumping off things
                if (LandingSpotExists(facing))
                {
                    Jump(moveJumpspeed, moveJumpheight);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If the player gets close enough and the enemy can jump
        if(player != null && jumpedHeight + 0.003 > height && (Vector2.Distance(body.transform.position, player.transform.position) < 14 && Timer > aJCD))
        {
            //reset Timer
            aJCD = Timer + attackJumpCD;
            //If the enemy is to the right of the player face left(default) and inverse jump speed
            if (body.transform.position.x > player.transform.position.x)
            {
                attackJumpspeed = attackJumpleft;
                //turn if you jump the other direction
                if (facing) { body.transform.Rotate(0, 180, 0, 0); facing = false; }
            }
            else
            {
                attackJumpspeed = attackJumpright;
                //turn if you jump the other direction
                if (!facing) { body.transform.Rotate(0, 180, 0, 0); facing = true; }
            }
            //jump at player
            Jump(attackJumpspeed, attackJumpheight);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Terrain") { StopSliding(); }
    }

    //jump method
    protected override void Jump(float speed, float height)
    {
        base.Jump(speed, height);
        whenJumped = Timer;
        jumpedHeight = body.transform.position.y;
    }
}
