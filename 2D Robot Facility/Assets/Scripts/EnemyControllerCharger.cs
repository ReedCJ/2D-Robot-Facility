using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerCharger : EnemyControllerTemplate
{
    public float chargeCD;
    public float chargeSpeed;
    public float chargeDuration;

    private float lastCharge = 0.0f;
    private Vector2 chargeRight;
    private Vector2 chargeLeft;
    private int nofloor = 0;

    [SerializeField] private bool boss;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        chargeRight = new Vector2(chargeSpeed, 0);
        chargeLeft = new Vector2(chargeSpeed * -1.0f, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Debug.Log("Player to the right " + PlayerToTheRight);
        //Debug.Log("Facing right " + facing);
        //Debug.Log("moveVector.x " + moveVector.x);
        //if player gets to close aggro on them
        if (DistanceToPlayer < aggroRange && PlayerOnLevel())
        {
            aggro = true;
        }
        //de aggro if you get out of leash range or out of range height wise 4x the height check distance
        else if (!boss && DistanceToPlayer > aggroLeash || !boss && DistanceToPlayer > levelCheckHeight * 6 && !PlayerOnLevel())
        {
            aggro = false;
        }
    }

    private void FixedUpdate()
    {
        //move around normally
        if (Timer > 1.0f && !aggro)
        {
            if (!boss && !ThereIsFloor() && OverGround() || ThereIsWall(4f,0))
            {
                //nofloor++;
                FlipAround();
                setMovement();
                //Debug.Log(hit2.collider);
            }
            if (OverGround()) { MoveAround(); }
        }
        else if (aggro && OverGround())
        {
            if(Timer > lastCharge + chargeCD)
            {
                ChargePlayer();
            }
            if (Timer > lastCharge + chargeDuration)
            {
                CutSpeed();
                FacePlayer();
            }
        }
    }

    //charge at the player
    private void ChargePlayer()
    {
        FacePlayer();
        lastCharge = Timer;
        //Debug.Log("Charged");
        if (PlayerToTheRight)
        {
            //Debug.Log("Charged Right");
            body.AddForce(chargeRight);
            //Debug.Log(chargeRight);
        }
        else
        {
            body.AddForce(chargeLeft);
        }
    }

    private void CutSpeed()
    {
        body.velocity = new Vector2(body.velocity.x * 0.9f, body.velocity.y);
    }
}
