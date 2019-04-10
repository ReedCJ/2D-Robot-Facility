using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform UIPause; //Will assign our panel to this variable so we can enable/disable it
   
    bool isPaused; //Used to determine paused state


    void Start()
    {
        UIPause.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        isPaused = false; //make sure isPaused is always false when our scene opens
    }

    
    public void Update()
    {
        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetButtonUp("Cancel") && !isPaused && !((SceneManager.GetActiveScene().buildIndex) == 0))
            Pause();
                        
        else if (Input.GetButtonUp("Cancel") && isPaused)
            UnPause();

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        isPaused = false;
        UIPause.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1f; //resume game
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIPause.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1f; //resume game
    }




}