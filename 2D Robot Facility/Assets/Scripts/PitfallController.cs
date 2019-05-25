using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PitfallController : MonoBehaviour
{
    private PlayerHealth player;
    private PlayerController playerController;
    private Cinemachine followCam;
    private Cinemachine fallCam;
    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        followCam.GetComponent<CinemachineVirtualCamera>().m_Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.GetComponent<PlayerHealth>();
            playerController = collision.GetComponent<PlayerController>();

            if (player.transform.position.y < transform.position.y)
            {
                player.health -= damage;
                Debug.Log("Health Remaining: " + player.health);
                player.time = 0;
                if (collision.GetComponent<PlayerHealth>().health <= 0)
                {
                    playerController.playerDeath();
                }
                else playerController.contactAnimate();
            }
        }
    }
}
