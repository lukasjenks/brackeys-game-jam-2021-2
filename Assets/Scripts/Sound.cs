using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;
    public string group;
    public AudioClip clip;
    public GameObject origin; // place in 3d space sound eminates from

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector] // dont want this in inspector cause we populate in audio manager
    public AudioSource source;
}
