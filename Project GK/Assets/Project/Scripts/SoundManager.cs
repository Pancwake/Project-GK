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
    }

    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [SerializeField] public List<AudioClip> repelSFX;
    [SerializeField] public List<AudioClip> catchSFX;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySFXFromList(List<AudioClip> clips)
    {
        int rng = Random.Range(0, clips.Count);
        //SFXSource.clip = clips[rng];
        //SFXSource.Play();
        SFXSource.PlayOneShot(clips[rng]);
    }
}