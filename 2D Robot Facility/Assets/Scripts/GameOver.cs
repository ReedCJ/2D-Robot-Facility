using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject restartGame;
    public GameObject inGameUI;
    private IEnumerator coroutine;

    private void Start()
    {
       // gameOverScreen.SetActive(false);
       // restartGame.SetActive(false);
    }

    void OnDestroy ()
    {
        inGameUI.SetActive(false);
        gameOverScreen.SetActive(true);
        restartGame.SetActive(true);
       // coroutine = WaitAndRetry();
       // StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndRetry()
    {
        yield return new WaitForSeconds(1f);
        
    }
}
