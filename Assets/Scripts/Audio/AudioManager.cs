using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
//reference to script: https://www.youtube.com/watch?v=pbuJUaO-wpY
// reference for adding music and sfx: https://www.youtube.com/watch?v=eNi67i5my84
  public static AudioManager instance;

 [SerializeField] AudioMixer mixer;

 [SerializeField] AudioSource walkingSource;
 [SerializeField] List<AudioClip> walkingClip = new List<AudioClip>();

  public const string MUSIC_KEY = "musicVolume";
  public const string SOUND_KEY = "soundVolume";
  
//public float CurrentMusicVolume { get; private set; } = 1f;
//public float CurrentSoundVolume { get; private set; } = 1f;

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

public void WalkingSFX()
    {
      AudioClip clip = walkingClip[Random.Range(0, walkingClip.Count)];
      walkingSource.PlayOneShot(clip);
    }
  
  //Volume is saved in volume settings
  void LoadVolume()
  {
    float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    float soundVolume = PlayerPrefs.GetFloat(SOUND_KEY, 1f);

    //CurrentMusicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    //CurrentSoundVolume = PlayerPrefs.GetFloat(SOUND_KEY, 1f);

    //mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(CurrentMusicVolume) * 20);
    //mixer.SetFloat(VolumeSettings.MIXER_SOUND, Mathf.Log10(CurrentSoundVolume) * 20);

    mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
    mixer.SetFloat(VolumeSettings.MIXER_SOUND, Mathf.Log10(soundVolume) * 20);
  }
}
