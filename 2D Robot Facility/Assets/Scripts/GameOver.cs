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

    void Start()
    {
       // gameOverScreen.SetActive(false);
       // restartGame.SetActive(false);

    }

    public void gameOver ()
    {
        inGameUI.SetActive(false);
        gameOverScreen.SetActive(true);
        //restartGame.SetActive(true);
        coroutine = WaitAndRetry();
        StartCoroutine(coroutine);
        Time.timeScale = 0.5f;
    }

    private IEnumerator WaitAndRetry()
    {
        yield return new WaitForSeconds(1f);
        gameOverScreen.GetComponent<TextMeshProUGUI>().text = "T R Y  A G A I N";
        restartGame.SetActive(true);
    }

}
