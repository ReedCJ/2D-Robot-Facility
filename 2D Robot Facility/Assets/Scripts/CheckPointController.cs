using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    [Tooltip("Line the bottom of the gameObject's hitbox with ground to have the player respawn on top of that ground. " +
        "Make sure that the entirety of the gameObject's hitbox is not inside any other hitboxes, especially not the bottom" +
        " so that the player wont get stuck in anything when he respawns. CheckPoints wont work unless an object tagged " +
        "GameMaster with the GameMaster script is in the scene.")]
    public string howToUseCheckPoints;
    private GameMaster GM;      // The script that controls where the player spawns when the game is restarted
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector3 location = new Vector3(collision.transform.position.x, transform.position.y - transform.localScale.y / 2 + 1.4f, collision.transform.position.z);
            GM.checkPoint = location;
            GM.reachedPoint = true;
            GM.health = collision.GetComponent<PlayerHealth>().health;
        }
    }
}
