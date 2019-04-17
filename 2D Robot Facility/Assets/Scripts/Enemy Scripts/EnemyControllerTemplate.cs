using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerTemplate : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D body;
    
    public float floorCheckDistance;
    public float jumpRange;
    public bool randomMovement;

    private float timer;

    //true = right;
    private bool facing;

    // Start is called before the first frame update
    void Start()
    {
        //get rigidbody
        body = GetComponent<Rigidbody2D>();
        //get player
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
    }

    void FixedUpdate()
    {
    }

    //jump method
    private void Jump(float speed, float height)
    {
        body.velocity = new Vector2(0, 0);
        body.AddForce(new Vector2(speed * (1 + (Random.Range(0, jumpRange) / 10)), height * (1 + (Random.Range(0, jumpRange) / 10))));
    }

    //check whether there is floor where you are going
    private bool ThereIsFloor(bool facing)
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
}
