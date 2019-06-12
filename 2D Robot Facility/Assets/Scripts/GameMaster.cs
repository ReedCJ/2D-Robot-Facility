using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    [System.NonSerialized] public Vector3 playerPos;            // Where the player will respawn if the game just restarted.
    [System.NonSerialized] public Vector3 checkPoint;           // Where the player will respawn if the game just restarted.
    [System.NonSerialized] public float health;                 // The health value of the player
    [System.NonSerialized] public string[] cards;               // Which keycards the player has
    [System.NonSerialized] public bool reachedPoint = false;    // Whether or not the player has reached any checkpoints at all
    [System.NonSerialized] public bool loading = false;         // Is the game loading a save?

    void Awake()        // Don't destroy this object when restarting or loading a new scene, keep player check point information
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }
}
