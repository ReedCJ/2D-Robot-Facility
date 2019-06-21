using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float []playerPos;
    public float []lastCheckpoint;
    public string[] cards;
    public bool reached;
    public float health;

    public SaveData(PlayerController player)
    {
        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;

        GameMaster GM = GameMaster.instance;
        reached = GM.reachedPoint;
        if (reached)
        {
            Vector3 checkPoint = GM.checkPoint;
            lastCheckpoint = new float[3];
            lastCheckpoint[0] = checkPoint.x;
            lastCheckpoint[1] = checkPoint.y;
            lastCheckpoint[2] = checkPoint.z;
        }

        KeycardController keys = player.gameObject.GetComponent<KeycardController>();
        cards = new string[keys.keycards.Count - 1];
        for (int i = 1; i < keys.keycards.Count; i++)
            cards[i - 1] = keys.keycards[i];
    }

    public SaveData(PlayerController player, PlayerHealth hp)
    {
        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;

        GameMaster GM = GameMaster.instance;
        reached = GM.reachedPoint;
        if (reached)
        {
            Vector3 checkPoint = GM.checkPoint;
            lastCheckpoint = new float[3];
            lastCheckpoint[0] = checkPoint.x;
            lastCheckpoint[1] = checkPoint.y;
            lastCheckpoint[2] = checkPoint.z;
        }

        KeycardController keys = player.gameObject.GetComponent<KeycardController>();
        cards = new string[keys.keycards.Count];
        for (int i = 1; keys.keycards.Count < i; i++)
            cards[i - 1] = keys.keycards[i];
    }
}
