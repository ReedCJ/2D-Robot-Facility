using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatforms : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Collider2D platformCol;
#pragma warning restore 0649

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(player.grounded);
        if (player.transform.position.y > transform.position.y && player.grounded)
            player.thinGround = true;
        else
            player.thinGround = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player.transform.position.y > transform.position.y)
                player.thinGround = true;
            ToggleCol(player.fallThrough, player.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.thinGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }

    private void ToggleCol(bool fallThrough, GameObject chara)
    {
        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<Collider2D>(), fallThrough);
    }
}