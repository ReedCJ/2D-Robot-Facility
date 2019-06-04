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
        GameObject tempPlayer = GameObject.FindWithTag("Player");
        player = tempPlayer.GetComponent<PlayerController>();
        playerHP = tempPlayer.GetComponent<PlayerHealth>();
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
            Debug.Log("Location saved");
        }
        /*else if (Input.GetKeyDown("y"))
        {
            SaveData save = SaveSystem.LoadGame();
            if (save != null)
            {
                Vector3 location = new Vector3(save.playerPos[0], save.playerPos[1], save.playerPos[2]);
                player.transform.position = location;
                if (save.health > 0)
                    playerHP.health = save.health;
                GameMaster GM = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
                if (save.reached)
                {
                    GM.reachedPoint = save.reached;
                    Vector3 checkPoint = new Vector3();
                    checkPoint.x = save.lastCheckPoint[0];
                    checkPoint.y = save.lastCheckPoint[1];
                    checkPoint.z = save.lastCheckPoint[2];
                    GM.checkPoint = checkPoint;
                }
            }
        }*/
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
