using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PitfallController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform respawnLocationL;    // Respawn location on the left
    [SerializeField] private Transform respawnLocationR;    // Respawn location on the right
    [SerializeField] private bool respawnLeft;      // Can the player respawn on the left side?
    [SerializeField] private bool respawnRight;     // Can the player respawn on the right side?
    [SerializeField] private float damage;          // Damage player takes when falling outside the map
    [SerializeField] private float respawnDelay;    // How long the respawn is delayed
#pragma warning restore 0649
    private GameObject followCam;                   // The camera following the player

    private PlayerHealth player;                    // Script handling player damage
    private PlayerController playerController;      // Primary player script.

    private bool respawn;                           // Which spawn will the player respawn on? true == left
    private bool tookDamage;            // Safeguard against taking damage twice bug.

    // Start is called before the first frame update
    void Start()
    {
        tookDamage = false;
        followCam = GameObject.FindGameObjectWithTag("FollowCam");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.grounded && playerController.transform.position.x < transform.position.x)
            respawn = true;
        else if (playerController.grounded && playerController.transform.position.x > transform.position.x)
            respawn = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerController.transform.position.y < transform.position.y + transform.localScale.y / 5.0f)
        {
            followCam.SetActive(false);
            PlayerController.animator.SetBool("DeathFall", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.GetComponent<PlayerHealth>();

            if (player.transform.position.y < transform.position.y)
            {
                if (tookDamage == false)
                {
                    player.health -= damage;
                    Debug.Log(player.health);
                    tookDamage = true;
                    player.time = 0;

                    if (player.health > 0)
                        StartCoroutine(DelayRespawn());
                    else
                        playerController.playerDeathFall();
                }
            }
            else
                followCam.SetActive(true);
        }
        else if (collision.gameObject.tag == "Enemy1" && collision.transform.position.y < transform.position.y)
        {
            collision.gameObject.GetComponent<EnemyHealth>().health = 0;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    private void Respawn()          // Place the player on safe ground
    {
        playerController.body.velocity = Vector3.zero;
        PlayerController.animator.SetBool("DeathFall", false);
        if (respawnLeft && respawnRight)        // If both spots are available, choose one
        {
            if (respawn)
                playerController.transform.position = respawnLocationL.position;
            else
                playerController.transform.position = respawnLocationR.position;
            followCam.SetActive(true);
        }
        else if (respawnLeft)                   // If only one spot is available, respawn there
        {
            playerController.transform.position = respawnLocationL.position;
            followCam.SetActive(true);
        }
        else if (respawnRight)
        {
            playerController.transform.position = respawnLocationR.position;
            followCam.SetActive(true);
        }
        else                                    // If no spots are available, kill the player
        {
            player.health = 0;
            playerController.playerDeathFall();
        }
        tookDamage = false;
    }
}
