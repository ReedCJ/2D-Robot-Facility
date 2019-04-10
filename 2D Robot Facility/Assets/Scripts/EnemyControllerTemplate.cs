using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerTemplate : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D body;

    public float attackJumpspeed;
    public float attackJumpheight;
    public float attackJumpCD;
    public float moveJumpheight;
    public float moveJumpspeed;
    public float moveJumpCD;
    public bool randomMovement;


    private float timer;
    private float attackJumpleft;
    private float attackJumpright;
    private float moveJumpleft;
    private float moveJumpright;
    private float aJCD;
    private float mJCD;
    private float whenJumped;
    private float jumpedHeight;
    //for stopping
    private float height;

    //true = right;
    private bool facing;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        attackJumpright = attackJumpspeed;
        attackJumpleft = attackJumpspeed * -1.0f;
        moveJumpleft = moveJumpspeed * -1.0f;
        moveJumpright = moveJumpspeed;

        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //jump around to move when the player is not in attack range, also don't jump immediately after attacking
        if (aJCD < timer - 2 && timer > mJCD)
        {
            //add public cd value to the cd
            mJCD = timer + moveJumpCD;
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
                //jump!
                body.AddForce(new Vector2(moveJumpspeed, moveJumpheight));
                whenJumped = timer;
                jumpedHeight = body.transform.position.y;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If the player gets close enough and the enemy can jump
        if(player != null && jumpedHeight + 0.01 > height && (Vector2.Distance(body.transform.position, player.transform.position) < 14 && timer > aJCD))
        {
            //reset timer
            aJCD = timer + attackJumpCD;
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
            body.AddForce(new Vector2(attackJumpspeed, attackJumpheight));
            whenJumped = timer;
            jumpedHeight = body.transform.position.y;
        }

        if (height < body.transform.position.y + 0.001 && timer > whenJumped + 0.5f && jumpedHeight + 0.01 > height)
        {
            body.velocity = new Vector2(0, 0);
        }
        height = body.transform.position.y;
    }
}
