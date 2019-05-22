using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatforms : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Collider2D platformCol;
    [SerializeField] private ContactFilter2D filter;
#pragma warning restore 0649


    private Vector2 topLeft;            // Used to determine whether the player is overlapping with the platform's collider
    private Vector2 botRight;           // Used to determine whether the player is overlapping with the platform's collider
    private PlayerController player;    // Used to access variables in the player script
    private Collider2D[] playerCol;
    private Collider2D[] enemies;
    private bool overlapping;           // Is the player overlapping with the phyiscs collider?
    [System.NonSerialized] public bool pCol;        // Is the platform collider active with the player?
    [System.NonSerialized] public bool[] eCol;      // Is the platform collider active with the enemy at this index?

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        playerCol = new Collider2D[4];

        topLeft = new Vector2(-transform.localScale.x / 2.0f + transform.position.x, transform.localScale.y / 2.0f + transform.position.y + .005f);
        botRight = new Vector2(transform.localScale.x / 2.0f + transform.position.x, transform.localScale.y / 2.0f * .2f + transform.position.y - .007f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.OverlapArea(topLeft, botRight, filter, playerCol);
            if (playerCol[0] == null && playerCol[1] == null)
            {
                overlapping = false;
                ToggleCol(false, player.gameObject);
            }
            
            for (int i = 0; i < playerCol.Length; i++)      // Test whether or not the player is overlapping with the platform collider
            {
                if (playerCol[i] != null && playerCol[i].gameObject.GetComponent<PlayerController>() != null && !pCol)
                {
                    overlapping = true;
                }
            }

            if (player.transform.position.y > transform.position.y && player.grounded)      // Is the player on top of the thin platform?
                player.thinGround = true;
            else
                player.thinGround = false;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            // Set up variables for toggling thin platform collision
        }
        playerCol = null;
        playerCol = new Collider2D[4];
    }

            /*private void OnTriggerEnter2D(Collider2D other)
            {
                if (other.gameObject.tag == "Enemy")
                {
                    if (enemies != null)
                    {
                        Collider2D[] temp = new Collider2D[enemies.Length + 1];     // Uncomment these when enemies have an index variable
                        for (int i = 0; i < enemies.Length - 1; i++)
                        {
                            temp[i] = enemies[i];
                        }
                        //temp[temp.length - 1] = other;
                        //temp[temp.Length - 1].GetComponent<ENEMYCONTROLLERSCRIPTPLACEHOLDER>().index = temp.Length - 1;
                        enemies = temp;
                    }
                    else
                    {
                        enemies = new Collider2D[1];
                        enemies[0] = other;                         // Uncomment these when enemies have an index variable
                        //enemies[0].GetComponent<ENEMYCONTROLLERSCRIPTPLACEHOLDER>().index = 0;
                        //ToggleEnemyCol(enemies[0].GetComponent<ENEMYCONTROLLERSCRIPTPLACEHOLDER>().passThrough, 0);
                    }
                }
            }

            private void OnTriggerExit2D(Collider2D other)
            {
                if (other.gameObject.tag == "Enemy")
                {
                    Collider2D[] temp = new Collider2D[enemies.Length - 1];

                    if (temp.Length != 0)
                    {
                        int index = 0;
                        for(int i = 0; i < enemies.Length; i++)
                        {                                                   // Uncomment this when enemies have an index variable
                            //if (i != other.GetComponent<ENEMYSCRIPTPLACEHOLDER>().index)
                            //{
                            //    temp[index] = enemies[i];
                            //    eCol[index] = eCol[i];
                            //    index++;
                            //}
                        }
                    }
                    enemies = temp;
                }
            }*/

    private void FixedUpdate()
    {
        player.MoveThroughPlatform();                                                   // Update the player variables
        ToggleCol(((player.fallThrough || (Mathf.Abs(player.transform.position.x - transform.position.x) > transform.localScale.x / 2.0f + .4 && !player.grounded)) || player.jumpThrough || overlapping), player.gameObject);      // Send the proper boolean statement to toggle collision and the player's gameObject

        /*for (int i = 0; i < eCol.Length; i++)     // Uncomment this when enemies have a passThrough variable
        {
            ToggleEnemyCol(eCol[i].GetComponent<ENEMYSCRIPTPLACEHOLDER>().passThrough, eCol[i], i);
        }*/
    }

    private void ToggleCol(bool passThrough, GameObject chara)      // Toggle Collision. Call this to update the collision between this platform and the player
    {                                                               // Passthrough should be the truth value of whether or not the player should collide with the platform.
        pCol = !passThrough;

        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<BoxCollider2D>(), passThrough);
        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<CircleCollider2D>(), passThrough);
    }

    private void ToggleEnemyCol(bool passThrough, GameObject chara, int index)      // Toggle Collision. Call this to update the collision between this platform and an enemy
    {                                                               // Passthrough should be the truth value of whether or not the enemy should collide with the platform.
        eCol[index] = !passThrough;                                 // Index should be the current index of the enemy as it would appear in the eCol array.

        Physics2D.IgnoreCollision(platformCol, chara.GetComponent<Collider2D>(), passThrough);
    }
}