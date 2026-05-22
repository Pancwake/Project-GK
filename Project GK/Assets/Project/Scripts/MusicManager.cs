using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        MusicSource = GetComponent<AudioSource>();
    }

    [SerializeField] SettingsInfo settingsInfo;

    AudioSource MusicSource;

    [Header("Music")]
    [SerializeField] public AudioClip mainMenuMusic;

    public void UpdateVolume()
    {
        MusicSource.volume = (settingsInfo.musicVolume * settingsInfo.masterVolume);
    }

    public void PlayMusic(AudioClip music)
    {
        MusicSource.clip = music;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
