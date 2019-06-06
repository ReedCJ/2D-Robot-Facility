using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryController : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            enemy.GetComponent<EnemyControllerTemplate>().PlayerInTerritory = true;
        }
        else if(collision.gameObject == enemy)
        {
            enemy.GetComponent<EnemyControllerTemplate>().territorial = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.GetComponent<EnemyControllerTemplate>().PlayerInTerritory = false;
        }
        else if (collision.gameObject == enemy)
        {
            enemy.GetComponent<EnemyControllerTemplate>().territorial = false;
        }
    }
}
