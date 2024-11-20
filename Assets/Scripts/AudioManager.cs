using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    //script reference: https://www.youtube.com/watch?v=6OT43pvUyfY&t=30s

    public Sound[] sounds;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds) 
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start() 
    {
        Play("OverWorld");
    }

    public void Play (string name) 
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
    }
}
