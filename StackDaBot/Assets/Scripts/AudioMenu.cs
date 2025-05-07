using UnityEngine;
using UnityEngine.Audio;

public class AudioMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
    }
        public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
        public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
}
