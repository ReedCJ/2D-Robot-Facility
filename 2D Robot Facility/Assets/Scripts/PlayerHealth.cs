using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [System.NonSerialized]public float time;

    public bool invuln;
    public float maxHealth;
    [System.NonSerialized] public float health;
    public float invulnFrames;

    private void Start()
    {
        time = invulnFrames;
        health = maxHealth;
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

    public void Heal(float regen)       // Gain health, but cap out at the max.
    {
        health = (health + regen) % maxHealth;
    }
}
