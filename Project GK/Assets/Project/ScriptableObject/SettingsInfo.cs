using UnityEngine;

[CreateAssetMenu(fileName = "SettingsInfo", menuName = "Scriptable Objects/SettingsInfo")]
public class SettingsInfo : ScriptableObject
{
    [Header("Video settings")]
    //Resolution
    public float screenShake;

    [Header("Audio")]
    public float masterVolume;
    public float musicVolume;
    public float soundVolume;
}