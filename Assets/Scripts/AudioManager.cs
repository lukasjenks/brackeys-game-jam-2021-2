using UnityEngine.Audio;
using UnityEngine;
using System;

// call FindObjectOfType<AudioManager>().Play("NameOfSoundObj") anywhere in proj

/*
Main purpose of manager is to manage all sounds that we need to add or remove as we go
- volume clip
- volume + pitch, spatial blend
- loop property
- etc
*/
public class AudioManager : MonoBehaviour
{
    // Of type Sound (obj - see Sound.cs)
    public Sound[] sounds;

    // Start is called before the first frame update
    // Loop through and for each sound, add an audio source (needed to transmit to audio listener)
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = s.origin.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Soundtrack");
    }

    public bool IsPlaying(string name)
    {
        Sound s = GetSound(name);
        if (s != null)
        {
            return s.source.isPlaying;
        }
        return false;
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // avoid nullref
        if (s == null)
        {
            Debug.LogWarning("Tried to find sound of name " + name + " but failed! Please check the audio manager.");
            return null;
        }
        return s;
    }

    // Called outside the class to play a sound
    public void Play(string name)
    {
        Sound s = GetSound(name);
        if (s != null)
        {
            s.source.Play();
        }
    }

    public void Pause(string name)
    {
        Sound s = GetSound(name);
        if (s != null)
        {
            s.source.Pause();
        }
    }

    // Group functions
    public void StopGroup(string groupName)
    {
        foreach (Sound s in sounds)
        {
            if (s.group == groupName)
            {
                s.source.Pause();
            }
        }
    }

    public void StartGroup(string groupName)
    {
        foreach (Sound s in sounds)
        {
            if (s.group == groupName)
            {
                s.source.Play();
            }
        }
    }
}
