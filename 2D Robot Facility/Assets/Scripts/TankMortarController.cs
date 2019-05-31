using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMortarController : EnemyControllerTemplate
{
    public float travelTime;
    //public float speed;

    public GameObject tank;

    private bool tankFacing;
    private float startTime;
    private float currentTime;
    private Vector2 spawnPosition;
    private Vector2 playerPosition;
    private Vector2 p1;
    private Vector2 p2;
    private float hdp;
    private Vector2 ep;
    private float mh;
    private GameObject shooter;
    // Start is called before the first frame update
    protected override void Start()
    {
    }
    private void FixedUpdate()
    {
        currentTime = (Timer - startTime) / travelTime;
        body.transform.position = Parabola(spawnPosition, ep, mh, currentTime);
        //they should probably blow up at the end or something
        if(Timer > startTime + travelTime) { Destroy(gameObject); }
    }

    public void SetMortar(bool f, Vector2 sp, Vector2 pp, GameObject s)
    {
        base.Start();
        startTime = Timer;
        tankFacing = f;
        spawnPosition = sp;
        shooter = s;
        playerPosition = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y - 0.6f);
        hdp = shooter.gameObject.GetComponent<EnemyControllerTank>().HorizontalDistanceToPlayer;
        mh = (playerPosition.y - shooter.transform.position.y) + 0.6f;
        if (player.transform.position.y > shooter.transform.position.y + 1)
        {
            ep = GetEndPosition();
        }
        else
        {
            ep = playerPosition;
            travelTime /= 2;
            mh = 3;
        }
    }
    //destroy these on terrain collision
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Terrain") { Destroy(gameObject); }
    }
    //get position
    Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

    private Vector2 GetEndPosition()
    {
        Vector2 endPosition = spawnPosition;
        endPosition.y -= 2;
        if(PlayerToTheRight)
        {
            endPosition.x += hdp * 2;
        }
        else
        {
            endPosition.x -= hdp * 2;
        }
        return endPosition;
    }
}
