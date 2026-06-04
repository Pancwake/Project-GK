using System.Collections.Generic;
using UnityEngine;

public class CrowdSoundManager : MonoBehaviour
{
    public static CrowdSoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] SettingsInfo settingsInfo;

    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Crowd")]
    [SerializeField] public AudioClip crowdAmbience;
    [SerializeField] public List<AudioClip> crowdBooSFX;
    [SerializeField] public List<AudioClip> crowdCheerSFX;

    public void UpdateVolume()
    {
        MusicSource.volume = (settingsInfo.crowdVolume * settingsInfo.masterVolume);
        SFXSource.volume = (settingsInfo.crowdVolume * settingsInfo.masterVolume);
    }

    public void PlaySFXFromList(List<AudioClip> clips, float volumeModifier = 1f)
    {
        int rng = Random.Range(0, clips.Count);
        SFXSource.PlayOneShot(clips[rng], volumeModifier);
    }

    public void PlayCrowdAmbience()
    {
        MusicSource.clip = crowdAmbience;
        MusicSource.Play();
    }
}
