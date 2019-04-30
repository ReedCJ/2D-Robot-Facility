using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMortarController : MonoBehaviour
{
    public float height;
    public float travelTime;
    //public float speed;

    public GameObject tank;

    private bool facing;
    private float startTime;
    private float currentTime;
    private Vector2 spawnPosition;
    private Vector2 playerPosition;
    private Vector2 p1;
    private Vector2 p2;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        startTime = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        currentTime = (Timer - startTime) / travelTime;
        body.transform.position = Parabola(spawnPosition, playerPosition, height, currentTime);
        //they should probably blow up at the end or something
        if(Timer > startTime + travelTime) { Destroy(gameObject); }
    }
    //get the stuff you need to draw the curve
    private float Timer
    {
        get { return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().timer; }
    }

    public void SetMortar(bool f, Vector2 sp, Vector2 pp)
    {
        facing = f;
        spawnPosition = sp;
        playerPosition = pp;
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
        Parabola(spawnPosition, playerPosition, height, 0.5f);
    }
}
