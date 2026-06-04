using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class OptionsManager : MonoBehaviour
{
    MainMenuManager mainMenuManager;

    [SerializeField] GameObject deleteSaveButton;
    [SerializeField] GameObject confirmDeleteSave;
    [SerializeField] GameObject acknowledgeDeletion;
    [SerializeField] SettingsInfo settingsInfo;

    [Header("Video")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown displayModeDropdown;
    [SerializeField] Slider screenShakeSlider;

    [Header("Audio")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundVolumeSlider;
    [SerializeField] Slider crowdVolumeSlider;

    //Resolutions
    Resolution[] resolutions;
    List<Resolution> filteredResolutions; //Only resolutions with current refresh rate (Doesnt clutter the dropdown with refresh rates)
    float currentRefreshRate;
    int currentResolutionIndex = 0;

    //Display Mode
    List<FullScreenMode> displayModes;
    FullScreenMode currentDisplayMode = FullScreenMode.ExclusiveFullScreen;
    int currendDisplayModeIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuManager = GetComponent<MainMenuManager>();

        PopulateResolutionDropdown();
        PopulateDisplayModeDropdown();

        ShowDeletionButton();
        SetValues();
        ApplyAllSettings();
    }

    void PopulateResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> resolutionOptions = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            resolutionOptions.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                //If resolution already set then take it from save, if not take screen resolution
                if (PlayerPrefs.HasKey("ResolutionIndex"))
                {
                    currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
                }
                else
                {
                    currentResolutionIndex = i;
                    PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
                }
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateDisplayModeDropdown()
    {
        displayModes = new List<FullScreenMode>
        {
            FullScreenMode.ExclusiveFullScreen,
            FullScreenMode.FullScreenWindow,
            FullScreenMode.Windowed
        };

        List<string> displayModeOptions = new List<string>();

        for (int i = 0; i < displayModes.Count; i++)
        {
            switch (displayModes[i])
            {
                case FullScreenMode.ExclusiveFullScreen:
                    displayModeOptions.Add("Fullscreen");
                    break;
                case FullScreenMode.FullScreenWindow:
                    displayModeOptions.Add("Borderless");
                    break;
                case FullScreenMode.Windowed:
                    displayModeOptions.Add("Windowed");
                    break;
            }

            if (Screen.fullScreenMode == displayModes[i])
            {
                //If resolution already set then take it from save, if not take screen resolution
                if (PlayerPrefs.HasKey("DisplayModeIndex"))
                {
                    currendDisplayModeIndex = PlayerPrefs.GetInt("DisplayModeIndex");
                }
                else
                {
                    currendDisplayModeIndex = i;
                    PlayerPrefs.SetInt("DisplayModeIndex", currendDisplayModeIndex);
                }
            }
        }

        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(displayModeOptions);
        displayModeDropdown.SetValueWithoutNotify(currendDisplayModeIndex);
        displayModeDropdown.RefreshShownValue();
    }

    public void ExitOptions()
    {
        PlayerPrefs.Save(); //Save settings on option exit
        mainMenuManager.OpenMainMenu();
        ShowDeletionButton();
    }

    void SetValues()
    {
        resolutionDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("ResolutionIndex", 1));
        displayModeDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("DisplayModeIndex", 1));
        screenShakeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("ScreenShake", 1));

        masterVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", 1));
        musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1));
        soundVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SoundVolume", 1));
        crowdVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("CrowdVolume", 1));
    }

    void ApplyAllSettings()
    {
        //Resolution and displayMode
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);
        Resolution resolution = filteredResolutions[resolutionIndex];

        //Get displayMode
        int displayModeIndex = PlayerPrefs.GetInt("DisplayModeIndex", 1);
        FullScreenMode displayMode = displayModes[displayModeIndex];

        Screen.SetResolution(resolution.width, resolution.height, displayMode);

        //Screen shake
        float screenShakeValue = PlayerPrefs.GetFloat("ScreenShake", 1);
        settingsInfo.screenShake = screenShakeValue;

        //Master volume
        float masterVolumeValue = PlayerPrefs.GetFloat("MasterVolume", 1);
        settingsInfo.masterVolume = masterVolumeValue;

        //Music volume
        float musicVolumeValue = PlayerPrefs.GetFloat("MusicVolume", 1);
        settingsInfo.musicVolume = musicVolumeValue;

        //Sound volume
        float soundVolumeValue = PlayerPrefs.GetFloat("SoundVolume", 1);
        settingsInfo.soundVolume = soundVolumeValue;

        //Crowd volume
        float crowdVolumeValue = PlayerPrefs.GetFloat("CrowdVolume", 1);
        settingsInfo.crowdVolume = soundVolumeValue;

        MusicManager.Instance.UpdateVolume();
        SoundManager.Instance.UpdateVolume();
        CrowdSoundManager.Instance.UpdateVolume();
    }

    #region video
    public void ChangeResolution(int index)
    {
        Debug.Log("Change resolution: " + index);

        PlayerPrefs.SetInt("ResolutionIndex", index);
        Resolution resolution = filteredResolutions[index];

        //Get displayMode
        int displayModeIndex = PlayerPrefs.GetInt("DisplayModeIndex", 1);
        FullScreenMode displayMode = displayModes[displayModeIndex];

        Screen.SetResolution(resolution.width, resolution.height, displayMode);
    }

    public void ChangeDisplayMode(int index)
    {
        PlayerPrefs.SetInt("DisplayModeIndex", index);
        FullScreenMode displayMode = displayModes[index];
        Screen.fullScreenMode = displayMode;
    }

    public void ChangeScreenShake(float value)
    {
        settingsInfo.screenShake = value;
        PlayerPrefs.SetFloat("ScreenShake", value);
    }
    #endregion

    #region audio
    public void ChangeMasterVolume(float value)
    {
        settingsInfo.masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);

        MusicManager.Instance.UpdateVolume();
        SoundManager.Instance.UpdateVolume();
    }

    public void ChangeMusicVolume(float value)
    {
        settingsInfo.musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);

        MusicManager.Instance.UpdateVolume();
    }

    public void ChangeSoundVolume(float value)
    {
        settingsInfo.soundVolume = value;
        PlayerPrefs.SetFloat("SoundVolume", value);

        SoundManager.Instance.UpdateVolume();
    }

    public void ChangeCrowdVolume(float value)
    {
        settingsInfo.crowdVolume = value;
        PlayerPrefs.SetFloat("CrowdVolume", value);

        CrowdSoundManager.Instance.UpdateVolume();
    }
    #endregion

    #region save data
    public void PressDeleteSave()
    {
        deleteSaveButton.SetActive(false);
        acknowledgeDeletion.SetActive(false);
        confirmDeleteSave.SetActive(true);
    }

    public void PressConfirmDeleteSave()
    {
        deleteSaveButton.SetActive(false);
        confirmDeleteSave.SetActive(false);
        acknowledgeDeletion.SetActive(true);

        mainMenuManager.DeleteSave();
        mainMenuManager.UpdateDifficulties();
    }

    public void ShowDeletionButton()
    {
        confirmDeleteSave.SetActive(false);
        acknowledgeDeletion.SetActive(false);
        deleteSaveButton.SetActive(true);
    }
    #endregion
}