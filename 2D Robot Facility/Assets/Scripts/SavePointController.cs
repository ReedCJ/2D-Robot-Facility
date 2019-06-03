using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointController : MonoBehaviour
{
    private bool near;
    private bool safe;
    private PlayerController player;
    private PlayerHealth playerHP;

    // Start is called before the first frame update
    void Start()
    {
        near = false;
        safe = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && safe && near)
        {
            playerHP.health = playerHP.maxHealth;
            SaveSystem.SaveGame(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(player.tag))
        {
            near = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(player.tag))
        {
            near = false;
        }
    }
}
