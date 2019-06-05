﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float heal;         // How much this heals when picked up
#pragma warning restore 0649

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().Heal(heal);
            Destroy(this.gameObject);
        }
    }
}
