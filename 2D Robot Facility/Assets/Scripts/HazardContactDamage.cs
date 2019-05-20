using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardContactDamage : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float pDamage;
    [SerializeField] private float eDamage;
#pragma warning restore 0649

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

                if (player.time > player.invulnFrames)
                {
                    playerController.contactAnimate();
                    player.health -= pDamage;
                    Debug.Log("Health Remaining: " + player.health);
                    player.time = 0;
                    if (collision.GetComponent<PlayerHealth>().health <= 0)
                    {
                        playerController.playerDeath();
                    }
                }
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

                if (enemy.time > enemy.invulnFrames)
                {
                    enemy.health -= eDamage;
                    enemy.time = 0;
                    if (collision.GetComponent<EnemyHealth>().health <= 0)
                        Destroy(collision.gameObject);
                }
            }
        }
    }
}
