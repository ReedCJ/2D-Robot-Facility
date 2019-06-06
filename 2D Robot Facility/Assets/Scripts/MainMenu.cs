﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject Gamemaster;
#pragma warning restore 0649
    [SerializeField] Transform UIPause; //Will assign our panel to this variable so we can enable/disable it
    public TextMeshProUGUI healthText;
    public PlayerHealth player;
    public Image healthBar;
    private Animator animator;
    private GameObject menuPlayer;
    private AudioManager audio;
    
    private IEnumerator coroutine;

    public static bool isPaused; //Used to determine paused state

 
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        
        //assigns animation controller if main menu
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            menuPlayer = GameObject.FindWithTag("Menu");
            animator = menuPlayer.GetComponent<Animator>();
            audio.Play("MenuMusic");
            //Debug.Log("menuPlayer Animator=" + animator.ToString());
        }

        
              
        //assigns player health to player gameobject
        if (GameObject.FindGameObjectWithTag("Player"))
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        //if in-game menu 
        
        UIPause.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        isPaused = false; //make sure isPaused is always false when our scene opens

          
    }

    
    public void Update()
    {
        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetButtonUp("Pause") && !isPaused && !((SceneManager.GetActiveScene().buildIndex) == 0))
            Pause();
                        
        else if (Input.GetButtonUp("Pause") && isPaused)
        {
             UnPause();
        }

        if (player != null)
        {
            //percentage conversion
            healthText.text = (player.health/player.maxHealth) * 100 + "%";
            //bar 0-1 float conversion
            healthBar.fillAmount = player.health/player.maxHealth;

            if (healthBar.fillAmount <= 1 && healthBar.fillAmount >= .76)
                healthBar.color = new Color(0f / 255.0f, 234.0f / 255.0f, 0f / 255.0f);

            if (healthBar.fillAmount <= .75 && healthBar.fillAmount >= .51)
                healthBar.color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 0f / 255.0f);

            if (healthBar.fillAmount <= .50 && healthBar.fillAmount >= .26)
                healthBar.color = new Color(255.0f / 255.0f, 150.0f / 255.0f, 0f / 255.0f);

            if (healthBar.fillAmount <= .25 && healthBar.fillAmount >= 0)
                healthBar.color = new Color(255.0f / 0f, 0f / 255.0f, 0f / 255.0f);

        }
        

    }

    public void PlayGame()
    {
        //starts delayed StartGame function
        coroutine = StartGame();
        StartCoroutine(coroutine);
        animator.SetTrigger("Wake"); //Plays menuPlayer Wake animation
        audio.Play("Start"); //Plays menuPlayer SFX

    }

    public void ContinueGame()
    {
        coroutine = LoadGame();
        StartCoroutine(coroutine);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        UIPause.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f; //resume game
    }

    public void Pause()
    {
        isPaused = true;
        UIPause.gameObject.SetActive(true); //turn on the pause menu
        Time.timeScale = 0f; //pause the game
    }

    public void UnPause()
    {
        UIPause.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1f; //resume game 
        coroutine = UnPauseDelay();
        StartCoroutine(coroutine);
    }

    private IEnumerator UnPauseDelay()
    {
        yield return new WaitForSeconds(.1f);
        isPaused = false;
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LoadGame()      // Gets load data, puts it into GM, starts game
    {
        SaveData save = SaveSystem.LoadGame();
        if (save != null)               // If there isn't any load data, don't put it into the GM and don't start game
        {
            animator.SetTrigger("Wake"); //Plays menuPlayer Wake animation
            yield return new WaitForSeconds(2f);
            GameMaster GM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
            if (save.health > 0)
                GM.health = save.health;
            if (save.reached)
            {
                GM.reachedPoint = true;
                Vector3 checkPoint = new Vector3();
                checkPoint.x = save.lastCheckpoint[0];
                checkPoint.y = save.lastCheckpoint[1];
                checkPoint.z = save.lastCheckpoint[2];
                GM.checkPoint = checkPoint;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIPause.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1f; //resume game
    }




}