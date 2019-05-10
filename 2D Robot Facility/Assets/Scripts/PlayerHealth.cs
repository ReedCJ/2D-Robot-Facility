using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [System.NonSerialized]public float time;

    public float health;
    public float invulnFrames;

    private void Start()
    {
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time < invulnFrames)
        {
            Debug.Log("invuln");
        }
    }
}
