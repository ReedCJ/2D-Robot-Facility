using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [System.NonSerialized]public float time;

    public float health;
    public float invulnFrames;

    void Update()
    {
        time += Time.deltaTime;
    }
}
