using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    [SerializeField] private GameObject destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CanTeleport = true;
            collision.gameObject.GetComponent<PlayerController>().Teleporter = this.gameObject;
        }
    }

    public GameObject Destination
    {
        get { return destination; }
    }
}
