using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatforms : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Collider2D platformCol;
    [SerializeField] private ContactFilter2D filter;
#pragma warning restore 0649


    private Vector2 topLeft;        // Used to determine whether the player is overlapping with the platform's collider
    private Vector2 botRight;       // Used to determine whether the player is overlapping with the platform's collider
    private PlayerController player;    // Used to access variables in the player script
    private Collider2D []playerCol;
    private bool overlapping;       // Is the player overlapping with the phyiscs collider?
    [System.NonSerialized] public bool pCol;        // Is the platform collider active with the player?

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerCol = new Collider2D[10];

        topLeft = new Vector2(-transform.localScale.x / 2.0f + transform.position.x, transform.localScale.y / 2.0f + transform.position.y -.007f);
        botRight = new Vector2(transform.localScale.x / 2.0f + transform.position.x, transform.localScale.y / 2.0f * .6f + transform.position.y - .007f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            overlapping = false;
            Physics2D.OverlapArea(topLeft, botRight, filter, playerCol);
            for (int i = 0; i < playerCol.Length; i++)      // Test whether or not the player is overlapping with the platform collider
            {
                if (playerCol[i] != null && playerCol[i].gameObject.GetComponent<PlayerController>() != null && playerCol[i].gameObject.GetComponent<PlayerController>().tag == "Player")
                {
                    overlapping = true;
                }
            }

            if (player.transform.position.y > transform.position.y && player.grounded)      // Is the player on top of the thin platform?
                player.thinGround = true;
            else
                player.thinGround = false;

            player.MoveThroughPlatform();                                                   // Update the player variables
            ToggleCol(((player.fallThrough || (Mathf.Abs(player.transform.position.x - transform.position.x) > transform.localScale.x / 2.0f + .4 && !player.grounded)) || player.jumpThrough || overlapping), player.gameObject);      // Send the proper boolean statement to toggle collision and the player's gameObject
        }
    }

    private void ToggleCol(bool passThrough, GameObject chara)      // Toggle Collision. Call this to update the collision between this platform and the player
    {                                                               // Passthrough should be the truth value of whether or not the player should collide with the platform.
        pCol = !passThrough;

        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<BoxCollider2D>(), passThrough);
        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<CircleCollider2D>(), passThrough);
    }
}