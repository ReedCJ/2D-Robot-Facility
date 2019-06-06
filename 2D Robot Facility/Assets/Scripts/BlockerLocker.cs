using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerLocker : MonoBehaviour
{
    private DoorController door;
    private GameObject player;
    private GameObject blocker;
    private void Start()
    {
        door = transform.parent.GetComponent<DoorController>();
        player = GameObject.FindGameObjectWithTag("Player");
        blocker = transform.parent.transform.GetChild(0).gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (door.LockLeft)
        {
            if (player.transform.position.x < this.transform.position.x)
            {
                door.Locked = true;
                door.LockLeft = false;
                blocker.SetActive(true);
            }
        }
        if (door.LockRight)
        {
            if (player.transform.position.x > this.transform.position.x)
            {
                door.Locked = true;
                door.LockRight = false;
                blocker.SetActive(true);
            }
        }
    }
}
