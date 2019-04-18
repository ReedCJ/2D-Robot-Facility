using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatforms : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Collider2D platformCol;
#pragma warning restore 0649

    private PlayerController player;
    [System.NonSerialized] public bool pCol;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (player.transform.position.y > transform.position.y && player.grounded)
            player.thinGround = true;
        else
            player.thinGround = false;
        Debug.Log(player.grounded);
        //Debug.Log(player.fallThrough || player.jumpThrough);
        ToggleCol(player.fallThrough || player.jumpThrough, player.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player.transform.position.y > transform.position.y)
                player.thinGround = true;
            ToggleCol(player.fallThrough || player.jumpThrough, player.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }

    private void ToggleCol(bool passThrough, GameObject chara)
    {
        pCol = passThrough;
        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<BoxCollider2D>(), passThrough);
        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<CircleCollider2D>(), passThrough);
    }
}