using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageCharger : MonoBehaviour
{
    [SerializeField] private float damage;

    private PlayerHealth player;
    private PlayerController playerController;
    private GameObject roboto;
   
    // Start is called before the first frame update
    void Start()
    {
        roboto = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject collider = collision.GetComponent<GameObject>();
        
       if (collision.gameObject.tag == "Player")
       {
            player = collision.GetComponent<PlayerHealth>();
            playerController = collision.GetComponent<PlayerController>();

            //Determines the side the charger contacts the player
            if (roboto.transform.position.x < this.transform.position.x)
            {
                playerController.contactRight = true;
                //Debug.Log("Contacted on Right");
            }
            else
            {
                playerController.contactRight = false;
                //Debug.Log("Contacted on Left");
            }
            
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
