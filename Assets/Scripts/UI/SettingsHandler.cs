using UnityEngine;
using System.Collections;
using Fusion;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
	[Header("Canvases")]
	[SerializeField]
	Canvas canvas;
	[SerializeField]
	Canvas pauseButtonCanvas;
	[SerializeField]
	Canvas pauseSettingsCanvas;

    [Header("Audio")]
    [SerializeField]
    public AudioSource backgroundAudio;

    [Header("Settings")]
    [SerializeField]
    Slider musicVolumeSlider;

	// Use this for initialization
	void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.1f);
            LoadSettingsValues();
        }
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.1f);
            LoadSettingsValues();
        }
        else
        {
            LoadSettingsValues();
        }
	}

    public void LoadSettingsValues()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        //NetworkPlayer.LocaL.GetCom.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SaveSettingsValues()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        //PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);
    }

    public void ChangeMusicVolume()
    {
        backgroundAudio.volume = musicVolumeSlider.value;
        SaveSettingsValues();
    }

}

