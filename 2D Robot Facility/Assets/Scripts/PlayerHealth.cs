using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [System.NonSerialized]public float time;

    public bool invuln;
    public float health;
    public float invulnFrames;

    private void Start()
    {
        time = invulnFrames;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < invulnFrames)
        {
            invuln = true;
            //Debug.Log("invuln");
        }
        else
        {
            invuln = false;
            //Debug.Log("NOT invuln");
        }
    }
}
