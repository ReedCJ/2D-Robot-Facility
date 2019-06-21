using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameMaster GM = GameMaster.instance;
        if (GM.loading)
        {
            transform.position = GM.playerPos;
            GM.loading = false;
        }
        else if (GM.reachedPoint)
        {
            if (GM.health != 0)
                GetComponent<PlayerHealth>().health = GM.health;
            transform.position = GM.checkPoint;
        }
    }
}
