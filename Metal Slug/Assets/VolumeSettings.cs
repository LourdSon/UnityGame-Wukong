using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider slider;
    public Slider sliderCity;
    public Slider sliderPlayer;
    private float volume;
    private float volumeCity;
    private float volumePlayer;

    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if(PlayerPrefs.HasKey("cityVolume"))
        {
            LoadCityVolume();
        }
        else
        {
            SetCityVolume();
        }

        if(PlayerPrefs.HasKey("playerVolume"))
        {
            LoadPlayerVolume();
        }
        else
        {
            SetPlayerVolume();
        }

    }
    public void SetMusicVolume()
    {
        volume = slider.value;
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
    {
        slider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }

    public void SetCityVolume()
    {
        volumeCity = sliderCity.value;
        audioMixer.SetFloat("City", volumeCity);
        PlayerPrefs.SetFloat("cityVolume", volumeCity);
    }
    private void LoadCityVolume()
    {
        sliderCity.value = PlayerPrefs.GetFloat("cityVolume");
        SetCityVolume();
    }
    public void SetPlayerVolume()
    {
        volumePlayer = sliderPlayer.value;
        audioMixer.SetFloat("Player", volumePlayer);
        PlayerPrefs.SetFloat("playerVolume", volumePlayer);
    }

    private void LoadPlayerVolume()
    {
        sliderPlayer.value = PlayerPrefs.GetFloat("playerVolume");
        SetPlayerVolume();
    }



}
