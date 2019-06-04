﻿using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!locked)
        {
            if (collision.gameObject == player)
            {
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
        if(collision.gameObject == player)
        {
            blocker.SetActive(true);
        }
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
    }

    public void Unlock()
    {
        locked = false;
    }
}
