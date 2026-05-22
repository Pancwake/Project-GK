using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    MainMenuManager mainMenuManager;

    [SerializeField] GameObject deleteSaveButton;
    [SerializeField] GameObject confirmDeleteSave;
    [SerializeField] GameObject acknowledgeDeletion;
    [SerializeField] SettingsInfo settingsInfo;

    [Header("Video")]
    [SerializeField] Slider screenShakeSlider;

    [Header("Audio")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundVolumeSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuManager = GetComponent<MainMenuManager>();

        ShowDeletionButton();
        SetSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitOptions()
    {
        PlayerPrefs.Save(); //Save settings on option exit
        mainMenuManager.OpenMainMenu();
        ShowDeletionButton();
    }

    void SetSettings()
    {
        screenShakeSlider.value = PlayerPrefs.GetFloat("ScreenShake", 1);

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1);
    }

    #region video
    public void ChangeScreenShake()
    {
        settingsInfo.screenShake = screenShakeSlider.value;
        PlayerPrefs.SetFloat("ScreenShake", screenShakeSlider.value);
    }
    #endregion

    #region audio
    public void ChangeMasterVolume()
    {
        settingsInfo.masterVolume = masterVolumeSlider.value;
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);

        MusicManager.Instance.UpdateVolume();
        SoundManager.Instance.UpdateVolume();
    }

    public void ChangeMusicVolume()
    {
        settingsInfo.musicVolume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        MusicManager.Instance.UpdateVolume();
    }

    public void ChangeSoundVolume()
    {
        settingsInfo.soundVolume = soundVolumeSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", soundVolumeSlider.value);

        SoundManager.Instance.UpdateVolume();
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