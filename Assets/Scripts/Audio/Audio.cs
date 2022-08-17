using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;
    [Range(-5f, 5f)]
    public float pitch;

    public bool isLooped;

    [HideInInspector]
    public AudioSource audioSource;
}
