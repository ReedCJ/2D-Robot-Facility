﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageCharger : MonoBehaviour
{
    [SerializeField] private float damage;

    private PlayerHealth player;
    private PlayerController playerController;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
                if (collision.GetComponent<PlayerHealth>().health <= 0)
                {
                    // collision.gameObject.SetActive(false);
                    playerController.playerDeath();
                }
                else playerController.contactAnimateCharger();
            }
        }
    }
}
