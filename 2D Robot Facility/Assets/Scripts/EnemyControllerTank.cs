using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerTank : EnemyControllerTemplate
{
    public float mortarCount;
    public float mortarCD;
    public float laserSpeed;
    public float laserDuration;

    private float mc;
    private float sm;
    private float lf;
    private float la;
    private GameObject laserParent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sm = 0;
        laserParent = body.gameObject.transform.GetChild(1).gameObject;
        RotateLaser(0, true);
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (Vector2.Distance(body.transform.position, player.transform.position) < aggroRange)
        {
            aggro = true;
        }
        else if (Vector2.Distance(body.transform.position, player.transform.position) > aggroLeash)
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
        //when the player is aggro
        if(aggro)
        {
            FacePlayer();
            if (mc >= mortarCount)
            {
                mc = 0;
                FireLaser();
            }
            //not shooting laser yet and mortars off off cd
            else if (mc < mortarCount && Timer > sm + mortarCD)
            {
                mc++;
                sm = Timer;
                Debug.Log("Shot a mortar");
            }
        }
        if(laserParent.activeSelf)
        {
            if(Timer > lf + laserDuration)
            {
                laserParent.SetActive(false);
            }
        }
        Debug.Log(Vector2.Angle(body.transform.position, player.transform.position));
    }

    private void FireLaser()
    {
        RotateLaser(GetAngleToPlayer(), false);
        RotateLaser(GetAngleToPlayer(), true);
        laserParent.SetActive(true);
        lf = Timer;
        Debug.Log("Shot a laser");
    }

    private void RotateLaser(float laserAngle, bool atPlayer)
    {
        if (atPlayer)
        {
            laserParent.transform.Rotate(0, 0, laserAngle);
            la = laserAngle;
        }
        else
        {
            laserParent.transform.Rotate(0, 0, -la);
        }
    }
}
