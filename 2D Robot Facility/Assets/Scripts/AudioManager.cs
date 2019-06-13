using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;
  
	public Sound[] sounds;
    
    private IEnumerator coroutine;
    private Sound sound;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

            if(s.mixerGroup != null)
            {
                s.source.outputAudioMixerGroup = s.mixerGroup;
            }

			//s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

 //       Debug.Log("Playing " + sound);

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        
        s.source.Play();
	}


    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Stop();
    }

    public void Fade(string name2, string name)
    {
        sound = Array.Find(sounds, s => s.name == name2);
        StopAllCoroutines();
        if (sound != null) StartCoroutine(EndSound());

        sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Music " + name + " not found.");
            return;
        }
        StartCoroutine(StartSound());
    }

    private IEnumerator EndSound()
    {
        AudioSource oldSound = sound.source;
        while (oldSound.volume > 0)
        {
            oldSound.volume -= 0.01f;
            yield return new WaitForSeconds(.15f);
        }
        oldSound.Stop();
    }

    private IEnumerator StartSound()
    {
        sound.source.Play();
        float volume = 0f;
        do
        {
            sound.source.volume = volume;
            volume += 0.01f;
            yield return new WaitForSeconds(.15f);
        } while (sound.source.volume <= sound.volume);
    }
}
