using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageCharger : MonoBehaviour
{
    [SerializeField] private float damage;

    private PlayerHealth player;
    private PlayerController playerController;

    private bool right;
    private bool top;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        if (collision.gameObject.tag == "Player")
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collider.bounds.center;

            right = contactPoint.x > center.x;
            top = contactPoint.y > center.y;
        }
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
                else playerController.contactAnimateCharger(right);
            }
        }
    }
}
