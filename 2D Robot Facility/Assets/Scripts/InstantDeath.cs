using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeath : MonoBehaviour
{
    private PlayerHealth player;
    private PlayerController playerController;

    [System.NonSerialized] public bool active;

    private void Start()
    {
        active = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active)
        {
            if (collision.gameObject.tag == "Player")
            {
                player = collision.GetComponent<PlayerHealth>();
                playerController = collision.GetComponent<PlayerController>();

                playerController.contactAnimate();
                player.health = 0;
                Debug.Log("Health Remaining: " + player.health);
                playerController.playerDeath();
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

                enemy.health = 0;
                Destroy(collision.gameObject);
            }
        }
    }
}
