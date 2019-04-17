using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject restartGame;
    private IEnumerator coroutine;

    void OnDestroy ()
    {
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
