using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform UIPause; //Will assign our panel to this variable so we can enable/disable it
    public TextMeshProUGUI healthText;
    public PlayerHealth player;
    public Image healthBar;
    private Animator animator;
    private GameObject menuPlayer;
    private AudioManager audio;
    private OptionsMenu optionsMenu;
    
    private IEnumerator coroutine;

    public static bool isPaused; //Used to determine paused state

 
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        
        //Check if on main menu scene
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            //assigns menu player model animator on main
            menuPlayer = GameObject.FindWithTag("Menu");
            animator = menuPlayer.GetComponent<Animator>();

            //Plays Menu music if not already playing (returning to main)
            if(!audio.sounds[2].source.isPlaying)
            audio.Play("MenuMusic");
           
            //disables Options Menu at start
            optionsMenu = FindObjectOfType<OptionsMenu>();
            optionsMenu.gameObject.SetActive(false);

            //updates options from persistent playerpref values
            optionsMenu.volSlider.value = PlayerPrefs.GetFloat("MVolume", 1f);
            optionsMenu.audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("MVolume"));

            optionsMenu.bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", 1f);
            optionsMenu.audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("bgmVolume"));

            optionsMenu.sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
            optionsMenu.audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("sfxVolume"));
        }
        else
        {
            //assigns player health to player gameobject
            if (GameObject.FindGameObjectWithTag("Player"))
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

            //in-game menu 
            UIPause.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
            isPaused = false; //make sure isPaused is always false when our scene opens
        }
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
        audio.Fade("MenuMusic", "BGM");

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
        //audio.Fade("BGM", "MenuMusic");
        audio.Stop("BGM");
        SceneManager.LoadScene(0);
        UIPause.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f; //resume game
        var aSources = FindObjectsOfType<AudioSource>(); //resumes audio effect
        foreach (AudioSource s in aSources)
            s.pitch = Time.timeScale;
    }

    public void Pause()
    {
        isPaused = true;
        UIPause.gameObject.SetActive(true); //turn on the pause menu
        Time.timeScale = 0f; //pause the game
        var aSources = FindObjectsOfType<AudioSource>(); //pauses audio
        foreach (AudioSource s in aSources)
            s.pitch = Time.timeScale;
    }

    public void UnPause()
    {
        UIPause.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1f; //resume game 
        coroutine = UnPauseDelay();
        StartCoroutine(coroutine);
        var aSources = FindObjectsOfType<AudioSource>(); //unpauses audio
        foreach (AudioSource s in aSources)
            s.pitch = Time.timeScale;
    }

    private IEnumerator UnPauseDelay()
    {
        yield return new WaitForSeconds(.1f);
        isPaused = false;
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
      //  audio.Stop("MenuMusic");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LoadGame()      // Gets load data, puts it into GM, starts game
    {
        var aSources = FindObjectsOfType<AudioSource>(); //resumes normal audio
        foreach (AudioSource s in aSources)
            s.pitch = Time.timeScale;
        SaveData save = SaveSystem.LoadGame();
        if (save != null)               // If there isn't any load data, don't put it into the GM and don't start game
        {
            animator.SetTrigger("Wake"); //Plays menuPlayer Wake animation
            audio.Play("Start"); //Plays menuPlayer SFX
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
            else
                GM.reachedPoint = false;

            GM.loading = true;
            GM.playerPos.x = save.playerPos[0];
            GM.playerPos.y = save.playerPos[1];
            GM.playerPos.z = save.playerPos[2];

            if (GM.reachedPoint)
                Debug.Log("The player was able to reach the checkpoint at: " + GM.checkPoint.x + " " + GM.checkPoint.y + " " + GM.checkPoint.z);
            else
                Debug.Log("The player was not able to reach a checkpoint.");

            Debug.Log("Player will be position at: " + GM.playerPos.x + " " + GM.playerPos.y + " " + GM.playerPos.z);

            if (GM.health > 0)
                Debug.Log("The player will be spawned with " + GM.health + " health.");
            else if (GM.health == 0)
                Debug.Log("Health was not recorded during the save.");
            else Debug.Log("Invalid health amount was recorded.");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            Debug.Log("Load failed.");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIPause.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1f; //resume game
        var aSources = FindObjectsOfType<AudioSource>(); //resumes audio effect
        foreach (AudioSource s in aSources)
            s.pitch = Time.timeScale;
    }




}