using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        SFXSource = GetComponent<AudioSource>();
    }

    [SerializeField] SettingsInfo settingsInfo;

    AudioSource SFXSource;

    [Header("Gameplay")]
    [SerializeField] public List<AudioClip> catchSFX;
    [SerializeField] public List<AudioClip> repelSFX;
    [SerializeField] public List<AudioClip> goalSFX;
    [SerializeField] public List<AudioClip> windSFX;
    [SerializeField] public List<AudioClip> kickSFX;
    [SerializeField] public List<AudioClip> grassSFX;
    [SerializeField] public List<AudioClip> asphaltSFX;

    [Header("Crowd")]
    [SerializeField] public List<AudioClip> crowdBooSFX;
    [SerializeField] public List<AudioClip> crowdCheerSFX;

    public void UpdateVolume()
    {
        SFXSource.volume = (settingsInfo.soundVolume * settingsInfo.masterVolume);
    }

    public void PlaySFXFromList(List<AudioClip> clips, float volumeModifier = 1)
    {
        int rng = Random.Range(0, clips.Count);
        SFXSource.PlayOneShot(clips[rng], volumeModifier);
    }
}