using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class SettingsManager : MonoBehaviour {
    public Toggle fullscreenToggle;
    public Toggle musicToggle;
    public Dropdown resolutionDropDown;
    public Slider musicVolumeSlider;
    public Button backButton;

    public AudioSource musicSource;
    public AudioSource soundSource;
    public Resolution[] resolutions;
    private GameSettings gamesettings;

    void OnEnable()
    {
        gamesettings = new GameSettings();
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });
        musicToggle.onValueChanged.AddListener(delegate { OnMusicToggle(); });
        resolutionDropDown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolueme(); });
        backButton.onClick.AddListener(delegate { OnApplyButton(); });
        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        if (File.Exists(Application.persistentDataPath + "/gamesettings.json") == true)
        {
            LoadSettings();
        }
    }
    public void OnFullScreenToggle()
    {
        gamesettings.fullscreen= Screen.fullScreen = fullscreenToggle.isOn;

    }

    public void OnMusicToggle()
    {
        soundSource.mute = gamesettings.musicMute = musicToggle.isOn ;
        musicSource.mute = gamesettings.musicMute = musicToggle.isOn;
    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, Screen.fullScreen);
    }
    public void OnMusicVolueme()
    {
        soundSource.volume = gamesettings.musicVolume = musicVolumeSlider.value;
        musicSource.volume = gamesettings.musicVolume = musicVolumeSlider.value;
    }
    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gamesettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }
    public void LoadSettings()
    {
        File.ReadAllText(Application.persistentDataPath + "/gamesettings.json");
        gamesettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json")); musicVolumeSlider.value = gamesettings.musicVolume;
        fullscreenToggle.isOn = gamesettings.fullscreen;
        resolutionDropDown.value = gamesettings.resolutionIndex;
        musicToggle.isOn = gamesettings.musicMute;

        resolutionDropDown.RefreshShownValue();
    }
    public void OnApplyButton()
    {
        SaveSettings();
    }
}
