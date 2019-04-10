using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver2 : MonoBehaviour
{

    public GameObject restartGame;
    private IEnumerator coroutine;


    // Start is called before the first frame update
    void Start()
    {
        coroutine = WaitAndRetry();
        StartCoroutine(coroutine);
    }
   

    private IEnumerator WaitAndRetry()
    {
        yield return new WaitForSeconds(2f);
        restartGame.SetActive(true);
    }
}


