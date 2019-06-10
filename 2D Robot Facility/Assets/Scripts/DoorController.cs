using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject player;
    private GameObject blocker;
    private bool locked;
    [SerializeField] private string doorCode;
    [SerializeField] private bool lockLeft;
    [SerializeField] private bool lockRight;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        blocker = gameObject.transform.GetChild(0).gameObject;
        locked = false;
    }

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    public bool LockLeft
    {
        get { return lockLeft; }
        set { lockLeft = value; }
    }

    public bool LockRight
    {
        get { return lockRight; }
        set { lockRight = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!locked)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (doorCode == "none")
                    blocker.SetActive(false);

                foreach (string k in player.GetComponent<KeycardController>().keycards)
                {
                    if (k == doorCode)
                    {
                        blocker.SetActive(false);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            blocker.SetActive(true);
        }

        /* In the wall now for smaller hitbox
        if (lockLeft)
        {
            if (player.transform.position.x < this.transform.position.x)
            {
                locked = true;
                lockLeft = false;
            }
        }
        if(lockRight)
        {
            if (player.transform.position.x > this.transform.position.x)
            {
                locked = true;
                lockRight = false;
            }
        }
        */
    }
    public void Unlock()
    {
        locked = false;
    }
}
