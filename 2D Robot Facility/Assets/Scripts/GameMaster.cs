using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    [System.NonSerialized] public Vector3 playerPos;            // Where the player will respawn if the game just restarted.
    [System.NonSerialized] public Vector3 checkPoint;           // Where the player will respawn if the game just restarted.
    [System.NonSerialized] public float health;                 // The health value of the player
    [System.NonSerialized] public string[] cards;               // Which keycards the player has
    [System.NonSerialized] public bool reachedPoint = false;    // Whether or not the player has reached any checkpoints at all
    [System.NonSerialized] public bool loading = false;         // Is the game loading a save?

    public GameObject redCardUI;
    public GameObject redCard;
    public GameObject blueCardUI;
    public GameObject blueCard;
    public GameObject yellowCardUI;
    public GameObject yellowCard;
    public GameObject purpleCardUI;
    public GameObject purpleCard;

    void Awake()        // Don't destroy this object when restarting or loading a new scene, keep player check point information
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Start()
    {
        if(cards != null)
        {
            for (int i = 0; i <= cards.Length; i++)
            {
                if (cards[i] == "Red") 
                {
                    if(redCardUI)
                    Destroy(redCardUI);

                    if (redCard)
                        Destroy(redCard);
                }
                if (cards[i] == "Blue")
                {
                    if(blueCardUI)
                    Destroy(blueCardUI);

                    if (blueCard)
                        Destroy(blueCard);
                }
                if (cards[i] == "Yellow")
                {
                    if(yellowCardUI)
                    Destroy(yellowCardUI);

                    if (yellowCard)
                        Destroy(yellowCard);
                }
                if (cards[i] == "Purple")
                {
                    if(purpleCardUI)
                    Destroy(purpleCardUI);

                    if (purpleCard)
                        Destroy(purpleCard);
                }
            }
        }
        
    }
}
