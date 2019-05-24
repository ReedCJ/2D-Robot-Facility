using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerTank : EnemyControllerTemplate
{
    public float mortarCount;
    public float mortarCD;
    public float mortarSpawnHeight;
    public float mortarSpawnDistance;
    public float laserSpeed;
    public float laserDuration;

    private float mc;
    private float sm;
    private float lf;
    private float la;
    private float fl;
    private float mortarSpawnX;
    [SerializeField] private GameObject TankMortar;
    private GameObject laserParent;
    private GameObject laserTrace;
    private GameObject laser;
    private Quaternion placeHolderRotation;

    public Animator TankAnim;

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
            if (!ThereIsFloor() || ThereIsWall(5.5f, 0))
            {
                FlipAround();
                setMovement();
            }
            //move around slow
            MoveAround();
            TankAnim.SetBool("Walking", true);
        }
        //when the player is aggro
        if(aggro)
        {
            TankAnim.SetBool("Walking", false);
            //don't spin around while firing laser
            if (!laserParent.activeSelf)
            {
                FacePlayer();
            }
            //fire laser every x mortars if player is in right area
            if (mc >= mortarCount && PlayerOnLevel() && DistanceToPlayer > 7.0f & HorizontalDistanceToPlayer > 7.0f || DistanceToPlayer < 10 && DistanceToPlayer > 7 && Timer > fl + laserDuration + 0.5)
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
            if(DistanceToPlayer < 7 && !laserParent.activeSelf)
            {
                MoveAround();
                TankAnim.SetBool("Walking", true);
            }
        }
        //controls and deactivates laser
        if(laserParent.activeSelf)
        {
            if(Timer > lf + laserSpeed)
            {
                TankAnim.SetBool("Charging", false);
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
    private Vector2 MortarSpawn()
    {
        if (facing) { mortarSpawnX = body.transform.position.x + mortarSpawnDistance; }
        else { mortarSpawnX = body.transform.position.x - mortarSpawnDistance; }
        return new Vector2(mortarSpawnX, body.transform.position.y + mortarSpawnHeight);
    }

    private void FireMortar()
    {
        Debug.Log("Mortar Fired");
        Instantiate(TankMortar, MortarSpawn(), placeHolderRotation).GetComponent<TankMortarController>().SetMortar(facing, MortarSpawn(), player.transform.position, this.gameObject);
    }
    //fires the laser
    private void FireLaser()
    {
        //RotateLaser(GetAngleToPlayer(), false);
        //RotateLaser(GetAngleToPlayer(), true);
        Vector3 LaserLookAt = new Vector3(player.transform.position.x, player.transform.position.y, laserParent.transform.position.z);
        laserParent.transform.LookAt(LaserLookAt);
        laserParent.SetActive(true);
        TankAnim.SetBool("Charging", true);
        laserTrace.SetActive(true);
        TankAnim.SetTrigger("Shoot");
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
