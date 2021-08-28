using UnityEngine.Audio;
using UnityEngine;

/*
Welcome to Java Hut OOP Jr.
*/
[System.Serializable]
public class Sound {

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source; // dont want this in inspector cause we populate in audio manager
}
