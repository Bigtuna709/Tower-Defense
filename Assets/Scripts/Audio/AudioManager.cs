using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] Audio[] listOfSFXClips;
    [SerializeField] Audio backgroundMusic;

    private void Start()
    {
        SetAudioPeramaters(backgroundMusic, gameObject);
        backgroundMusic.audioSource.Play();
        foreach (Audio audio in listOfSFXClips)
        {
            SetAudioPeramaters(audio, gameObject);
        }
    }

    private void SetAudioPeramaters(Audio audio, GameObject audioLocation)
    {
        audio.audioSource = audioLocation.AddComponent<AudioSource>();
        audio.audioSource.clip = audio.audioClip;
        audio.audioSource.pitch = audio.pitch;
        audio.audioSource.volume = audio.volume;
        audio.audioSource.playOnAwake = false;
        audio.audioSource.loop = audio.isLooped;
    }

    public void PlayAudio(string name)
    {
        Audio a = Array.Find(listOfSFXClips, audio => audio.name == name);

        if(a == null)
        {
            Debug.Log("<color=red>Incorrect audio name!</color>");
            return;
        }
        Debug.Log("Sound " + a.name);
        a.audioSource.Play();
    }

    public void StopAudio(string name)
    {

    }

    [ContextMenu("PlayClip")]
    private void TestAudioClip()
    {
        PlayAudio("ButtonClick");
    }

}
