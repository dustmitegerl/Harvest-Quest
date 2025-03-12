using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
//reference for script: https://www.youtube.com/watch?v=pbuJUaO-wpY

{
 [SerializeField] AudioMixer mixer;
 [SerializeField] Slider musicSlider;
 [SerializeField] Slider soundSlider;

 public const string MIXER_MUSIC = "MusicVolume";
 public const string MIXER_SOUND = "SoundVolume";

 void Awake()
 {
    musicSlider.onValueChanged.AddListener(SetMusicVolume);
    soundSlider.onValueChanged.AddListener(SetSoundVolume);
 }

 void Start()
 {
    musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
    soundSlider.value = PlayerPrefs.GetFloat(AudioManager.SOUND_KEY, 1f);
 }
 void OnDisable()
 {
    PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
    PlayerPrefs.SetFloat(AudioManager.SOUND_KEY, soundSlider.value);
 }

 void SetMusicVolume(float value)
 {
    mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
 }

 void SetSoundVolume(float value)
 {
    mixer.SetFloat(MIXER_SOUND, Mathf.Log10(value) * 20);
 }
}
