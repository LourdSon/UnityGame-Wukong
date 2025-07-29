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

    void OnEnable()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("cityVolume"))
        {
            LoadCityVolume();
        }
        else
        {
            SetCityVolume();
        }

        if (PlayerPrefs.HasKey("playerVolume"))
        {
            LoadPlayerVolume();
        }
        else
        {
            SetPlayerVolume();
        }
    }

    void Update()
    {
        SetMusicVolume();
        SetCityVolume();
        SetPlayerVolume();   
    }

    public void SetMusicVolume()
    {
        volume = slider.value;
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
    {
        volume = PlayerPrefs.GetFloat("musicVolume");
        slider.value = volume;
        audioMixer.SetFloat("Music", volume);
    }

    public void SetCityVolume()
    {
        volumeCity = sliderCity.value;
        audioMixer.SetFloat("City", volumeCity);
        PlayerPrefs.SetFloat("cityVolume", volumeCity);
    }

    private void LoadCityVolume()
    {
        volumeCity = PlayerPrefs.GetFloat("cityVolume");
        sliderCity.value = volumeCity;
        audioMixer.SetFloat("City", volumeCity);
    }

    public void SetPlayerVolume()
    {
        volumePlayer = sliderPlayer.value;
        audioMixer.SetFloat("Player", volumePlayer);
        PlayerPrefs.SetFloat("playerVolume", volumePlayer);
    }

    private void LoadPlayerVolume()
    {
        volumePlayer = PlayerPrefs.GetFloat("playerVolume");
        sliderPlayer.value = volumePlayer;
        audioMixer.SetFloat("Player", volumePlayer);
    }
}
