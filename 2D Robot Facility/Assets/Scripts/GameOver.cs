using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject restartGame;
    public GameObject inGameUI;
    private IEnumerator coroutine;

    void Start()
    {
       // gameOverScreen.SetActive(false);
       // restartGame.SetActive(false);

    }

    public void gameOver ()
    {
        inGameUI.SetActive(false);
        gameOverScreen.SetActive(true);
        restartGame.SetActive(true);
       // coroutine = WaitAndRetry();
       // StartCoroutine(coroutine);
    }

}
