using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    private void Start(){
        SetVolume();
    }
    private void SetVolume(){
        if(PlayerPrefs.HasKey("musicVolume")){
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else{
            musicSlider.value = 0.3f;
        }
        if(PlayerPrefs.HasKey("sfxVolume")){
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else{
            sfxSlider.value = 1f;
        }
        AudioManager.Instance.MusicVolume(musicSlider.value);
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }
    public void ToggleMusic(){
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX(){
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume(){
        AudioManager.Instance.MusicVolume(musicSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void SFXVolume(){
        AudioManager.Instance.SFXVolume(sfxSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
}
