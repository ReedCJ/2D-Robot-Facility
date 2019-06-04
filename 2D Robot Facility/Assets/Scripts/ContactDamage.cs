using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float damage;          // Damage the player will take if hit

    private PlayerHealth player;                    // Script that handles player damage
    private PlayerController playerController;      // Primary player script. Handles player animation
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.GetComponent<PlayerHealth>();
            playerController = collision.GetComponent<PlayerController>();

            if (player.time > player.invulnFrames)
            {
                player.health -= damage;
                Debug.Log("Health Remaining: " + player.health);
                player.time = 0;
                if (player.health <= 0)
                {
                    playerController.playerDeath();
                }
                else playerController.contactAnimate();
            }
        }
    }
}
