using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player.time > player.invulnFrames)
            {
                player.health -= damage;
                player.time = 0;
                if (collision.GetComponent<PlayerHealth>().health <= 0)
                    collision.gameObject.SetActive(false);
            }
        }
    }
}
