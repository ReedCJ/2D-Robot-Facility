using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float []playerPos;
    public float []lastCheckpoint;
    public bool reached;
    public float health;

    public SaveData(PlayerController player)
    {
        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;

        GameMaster GM = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
        reached = GM.reachedPoint;
        if (reached)
        {
            Vector3 checkPoint = GM.checkPoint;
            lastCheckpoint = new float[3];
            lastCheckpoint[0] = checkPoint.x;
            lastCheckpoint[1] = checkPoint.y;
            lastCheckpoint[2] = checkPoint.z;
        }
    }

    public SaveData(PlayerController player, PlayerHealth hp)
    {
        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;
        health = hp.health;

        GameMaster GM = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
        reached = GM.reachedPoint;
        Vector3 checkPoint = GM.checkPoint;
        lastCheckpoint[0] = checkPoint.x;
        lastCheckpoint[1] = checkPoint.y;
        lastCheckpoint[2] = checkPoint.z;
    }
}
