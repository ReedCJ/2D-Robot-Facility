using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject restartGame;
    public GameObject inGameUI;
    private IEnumerator coroutine;
    private bool dead;

    private AudioManager audio;

    void Start()
    {
        dead = false;
        audio = FindObjectOfType<AudioManager>();
       // gameOverScreen.SetActive(false);
       // restartGame.SetActive(false);

    }

    public void gameOver ()
    {
        if (!dead)
        {
            //if (audio != null)
               // audio.Play("Dead");

            //disable pause menu
            inGameUI.SetActive(false);

            //enable Game Over text element
            gameOverScreen.SetActive(true);
        
            //start timed try again message
            coroutine = WaitAndRetry();
            StartCoroutine(coroutine);

            //slow motion death screen effect
            Time.timeScale = 0.5f;

            // slows sound effect
            var aSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource s in aSources)
                s.pitch = Time.timeScale;

            dead = true;
        }
        
    }

    private IEnumerator WaitAndRetry()
    {
        yield return new WaitForSeconds(1f);
        gameOverScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "T R Y  A G A I N ?";
        restartGame.SetActive(true);
    }

}
