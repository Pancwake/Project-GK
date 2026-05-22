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

    [Header("Sound Effects")]
    [SerializeField] public List<AudioClip> repelSFX;
    [SerializeField] public List<AudioClip> catchSFX;

    public void UpdateVolume()
    {
        SFXSource.volume = (settingsInfo.soundVolume * settingsInfo.masterVolume);
    }

    public void PlaySFXFromList(List<AudioClip> clips)
    {
        int rng = Random.Range(0, clips.Count);
        SFXSource.PlayOneShot(clips[rng]);
    }
}