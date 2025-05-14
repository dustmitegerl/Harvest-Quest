//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
//reference to script: https://www.youtube.com/watch?v=pbuJUaO-wpY
  public static AudioManager instance;

 [SerializeField] AudioMixer mixer;

  public const string MUSIC_KEY = "musicVolume";
  public const string SOUND_KEY = "soundVolume";
  
  void Awake()
  {
    if (instance == null)
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }

    
    LoadVolume();
  }

  //Volume is saved in volume settings
  void LoadVolume()
  {
    float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    float soundVolume = PlayerPrefs.GetFloat(SOUND_KEY, 1f);

    mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
    mixer.SetFloat(VolumeSettings.MIXER_SOUND, Mathf.Log10(soundVolume) * 20);
  }
}
