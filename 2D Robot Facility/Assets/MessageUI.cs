using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public string message;
    public GameObject messageUIPrefab;
    private GameObject _ui;
    private bool triggered = false;
      
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!triggered)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (messageUIPrefab)
                showMessage();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (messageUIPrefab)
            hideMessage();
        }
    }


    private void showMessage()
    {
        triggered = true;
        _ui = Instantiate(messageUIPrefab);
        _ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
    }

    

    private void hideMessage()
    {
        Destroy(_ui);
        triggered = false;
    }
}
