using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerCharger : EnemyControllerTemplate
{
    public float chargeCD;
    public float chargeSpeed;

    private float lastCharge = 0.0f;
    private Vector2 chargeRight;
    private Vector2 chargeLeft;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        setMovement();
        chargeRight = new Vector2(chargeSpeed, 0);
        chargeLeft = new Vector2(chargeSpeed * -1.0f, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //if player gets to close aggro on them
        if (Vector2.Distance(body.transform.position, player.transform.position) < aggroRange && PlayerOnLevel())
        {
            aggro = true;
        }
        //de aggro if you get out of leash range or out of range height wise 4x the height check distance
        else if (Vector2.Distance(body.transform.position, player.transform.position) > aggroLeash || Vector2.Distance(body.transform.position, player.transform.position) > levelCheckHeight * 4 && !PlayerOnLevel())
        {
            aggro = false;
        }
    }

    private void FixedUpdate()
    {
        //move around normally
        if (Timer > 1.0f && !aggro)
        {
            //don't run off edges
            if (!ThereIsFloor()) { facing = !facing; setMovement(); }
            //move around slow
            MoveAround();
        }
        else if (aggro && Timer > lastCharge + chargeCD)
        {
            ChargePlayer();
        }
    }
    //charge at the player
    private void ChargePlayer()
    {
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
        lastCharge = Timer;
    }
}
