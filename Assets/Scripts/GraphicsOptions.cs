using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GraphicsOptions : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown displayModeDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle vsyncToggle;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions = new List<Resolution>();

    [System.Obsolete]
    void Start()
    {
        // Récupérer toutes les résolutions et filtrer les fréquences
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        filteredResolutions.Clear();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        foreach (var res in resolutions)
        {
            float refreshRate = (float)res.refreshRateRatio.value;
            if (Mathf.Approximately(refreshRate, 60f) ||
                Mathf.Approximately(refreshRate, 120f) ||
                Mathf.Approximately(refreshRate, 144f))
            {
                filteredResolutions.Add(res);
                options.Add(res.width + " x " + res.height + " @ " + refreshRate + "Hz");

                if (res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height &&
                    Mathf.Approximately(refreshRate, (float)Screen.currentResolution.refreshRateRatio.value))
                {
                    currentResolutionIndex = filteredResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);

        // Remplir le mode d'affichage
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string> { "Fullscreen", "Borderless", "Windowed" });

        // Remplir la qualité graphique
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));

        LoadSettings(currentResolutionIndex);
    }

    [System.Obsolete]
    public void ApplyGraphics()
    {
        Resolution res = filteredResolutions[resolutionDropdown.value];
        FullScreenMode mode = (FullScreenMode)displayModeDropdown.value;

        Screen.SetResolution(res.width, res.height, mode, (int)res.refreshRateRatio.value);
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;

        SaveSettings();
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("DisplayMode", displayModeDropdown.value);
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    [System.Obsolete]
    void LoadSettings(int defaultResolutionIndex)
    {
        // Charger la résolution
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", defaultResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        // Charger le mode d'affichage
        displayModeDropdown.value = PlayerPrefs.GetInt("DisplayMode", 0);
        displayModeDropdown.RefreshShownValue();

        // Charger la qualité
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        qualityDropdown.RefreshShownValue();

        // Charger le V-Sync
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 0) == 1;

        // Appliquer automatiquement
        ApplyGraphics();
    }

    [System.Obsolete]
    void Update()
    {
        ApplyGraphics();   
    }
}

