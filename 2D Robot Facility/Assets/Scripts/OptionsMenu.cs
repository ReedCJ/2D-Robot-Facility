using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Button backButton;
    public Animator animator;
    private AudioManager audio;

    private IEnumerator coroutine;
    
    public Dropdown resolutionDropdown;
    private Toggle fullscreen;

    Resolution[] resolutions;

    public TextMeshProUGUI masterText;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI sfxText;

    public void Start()
    {
        audio = FindObjectOfType<AudioManager>();

        //gets available resolutions as detected by unity and adds them to an array.
        //Then the available resolutions are added to a dropdown with the current resolution prepopulated

        resolutions = Screen.resolutions;
//      Debug.Log("There are " + resolutions.Length + " resolutions available");

        resolutionDropdown.ClearOptions();

        
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height && 
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
                PlayerPrefs.SetInt("CurrentResolutionIndex", currentResolutionIndex);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.value = PlayerPrefs.GetInt("CurrentResolutionIndex");

        resolutionDropdown.RefreshShownValue();

        //updates fullscreen toggle position
      //  fullscreen.enabled = Screen.fullScreen;

        //Saves audio changes to persistent playerpref values
        volSlider.value = PlayerPrefs.GetFloat("MVolume", 1f);
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("MVolume"));

        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", 1f);
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("bgmVolume"));

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("sfxVolume"));

    }

    public void OnEnable()
    {
        coroutine = JumperJump();
        StartCoroutine(coroutine);
    }

    public void Update()
    {
        //If player presses escape 
       // if (Input.GetButtonUp("Back"))
          //  backButton.onClick.Invoke();
    }

    public void FixedUpdate()
    {
        
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt("CurrentResolutionIndex", resolutionIndex);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

 /*   public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
*/


    public void SetVolume (float Volume)
    {
        //Changes Master Mixer to variable Volume
        audioMixer.SetFloat("Master", Volume);

        // Make Volume 1 to 100 converted from 0 to -80 
        float aValue = Volume;
        float normal = Mathf.InverseLerp(0, -80, aValue);
        float bValue = Mathf.Lerp(100, 0, normal);

       // Sets converted text in GUI as int
        masterText.text = System.Convert.ToInt32(bValue).ToString();
//      masterText.text = (Remap(Volume,0,-80,0,100)+100).ToString();

        //Saves audio changes to persistent playerpref values
        PlayerPrefs.SetFloat("MVolume", Volume);
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("MVolume"));
    }

    public void SetBGMVolume(float BGMVolume)
    {
        audioMixer.SetFloat("BGM", BGMVolume);

        // Make Volume 1 to 100 converted from 0 to -80 
        float aValue = BGMVolume;
        float normal = Mathf.InverseLerp(0, -80, aValue);
        float bValue = Mathf.Lerp(100, 0, normal);

        //Sets converted text in GUI as int
        bgmText.text = System.Convert.ToInt32(bValue).ToString();

        //Saves audio changes to persistent playerpref values
        PlayerPrefs.SetFloat("bgmVolume", BGMVolume);
        audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("bgmVolume"));
    }

    public void SetSFXVolume(float SFXVolume)
    {
        audioMixer.SetFloat("SFX", SFXVolume);

        // Make Volume 1 to 100 converted from 0 to -80 
        float aValue = SFXVolume;
        float normal = Mathf.InverseLerp(0, -80, aValue);
        float bValue = Mathf.Lerp(100, 0, normal);

        //Sets converted text in GUI as int
        sfxText.text = System.Convert.ToInt32(bValue).ToString();

        //Saves audio changes to persistent playerpref values
        PlayerPrefs.SetFloat("sfxVolume", SFXVolume);
        audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("sfxVolume"));
    }

    public void SetQuality( int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private IEnumerator JumperJump()
    {
        yield return new WaitForSeconds(4f);
        audio.Play("Jumper");
        animator.SetTrigger("Jump");
        coroutine = JumperJump();
        StartCoroutine(coroutine);
    }

}
