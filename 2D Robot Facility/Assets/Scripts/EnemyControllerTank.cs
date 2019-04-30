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
    private float fl;
    [SerializeField] private GameObject TankMortar;
    private GameObject laserParent;
    private GameObject laserTrace;
    private GameObject laser;
    private Quaternion placeHolderRotation;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sm = 0;
        laserParent = body.gameObject.transform.GetChild(1).gameObject;
        laserTrace = body.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        laser = body.gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        placeHolderRotation = new Quaternion();
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (DistanceToPlayer < aggroRange)
        {
            aggro = true;
        }
        else if (DistanceToPlayer > aggroLeash)
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
            //don't spin around while firing laser
            if (!laserParent.activeSelf)
            {
                FacePlayer();
            }
            //fire laser every x mortars if player is in right area
            if (mc >= mortarCount && PlayerOnLevel() && DistanceToPlayer > 3.0f & HorizontalDistanceToPlayer > 4.0f || DistanceToPlayer < 10 && Timer > fl + laserDuration + 0.5)
            {
                mc = 0;
                FireLaser();
                fl = Timer;
            }
            //not shooting laser yet and mortars off off cd, fire mortars
            else if (Timer > sm + mortarCD)
            {
                if (DistanceToPlayer > 10)
                {
                    FireMortar();
                    mc++;
                    sm = Timer;
                }
            }
        }
        //controls and deactivates laser
        if(laserParent.activeSelf)
        {
            if(Timer > lf + laserSpeed)
            {
                laserTrace.SetActive(false);
                laser.SetActive(true);
            }
            if(Timer > lf + laserDuration)
            {
                laserParent.SetActive(false);
                laser.SetActive(false);
            }
        }
    }
    //property that returns a position above the tank enemy
    private Vector2 MortarSpawn
    {
        get { return new Vector2(body.transform.position.x, body.transform.position.y + 2); }
    }
    private void FireMortar()
    {
        Instantiate(TankMortar, MortarSpawn, placeHolderRotation).GetComponent<TankMortarController>().SetMortar(facing, MortarSpawn, player.transform.position);
    }
    //fires the laser
    private void FireLaser()
    {
        //RotateLaser(GetAngleToPlayer(), false);
        //RotateLaser(GetAngleToPlayer(), true);
        laserParent.transform.LookAt(player.transform);
        laserParent.SetActive(true);
        laserTrace.SetActive(true);
        lf = Timer;
    }
    /*Old code from when angles were being tested
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
    */
}
