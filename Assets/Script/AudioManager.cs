using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; }
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake(){
        if(instance) Debug.LogError("Ton tai 1 AudioManager");
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start(){
        //PlayMusic("Theme");
    }
    public void PlayMusic(string name){
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if(s == null) Debug.LogError("Khong ton tai sound ten " + name);
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name){
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if(s == null) Debug.LogError("Khong ton tai sound ten " + name);
        sfxSource.PlayOneShot(s.clip);
    }

    public void ToggleMusic(){
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX(){
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float value){
        musicSource.volume = value;
    }

    public void SFXVolume(float value){
        sfxSource.volume = value;
    }
}
