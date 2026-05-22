using NUnit.Framework;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenShakeHandler : MonoBehaviour
{
    Camera cam;

    [SerializeField] SettingsInfo settingsInfo;

    [SerializeField] float screenShakeDuration = 1f;
    [SerializeField] AnimationCurve shakeCurve;

    [Header("Shaking Strength Modifiers")]
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    float goalShakeModifier = 1f;
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    float repelShakeModifier = 1f;
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    float catchShakeModifier = 1f;

    [Header("Shaking Time Modifiers")]
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    float goalShakeTimeodifier = 1f;
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    float repelShakeTimeModifier = 1f;
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    float catchShakeTimeModifier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoalShake()
    {
        StartCoroutine(DoShake(goalShakeModifier, goalShakeTimeodifier));
    }

    public void RepelShake()
    {
        StartCoroutine(DoShake(repelShakeModifier, repelShakeTimeModifier));
    }

    public void CatchShake()
    {
        StartCoroutine(DoShake(catchShakeModifier, catchShakeTimeModifier));
    }

    IEnumerator DoShake(float shakeStrenghtModifier, float timeModifier)
    {
        Vector3 startPos = cam.transform.position;
        float elapsedTime = 0f;
        float duration = screenShakeDuration * timeModifier;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            //Get shake strength depending on what action caused the shake, and the settings
            float shakeStrength = ((shakeCurve.Evaluate(elapsedTime / screenShakeDuration)) * shakeStrenghtModifier) * settingsInfo.screenShake;
            cam.transform.position = startPos + ((Vector3)Random.insideUnitCircle * shakeStrength); 
            yield return null;
        }

        cam.transform.position = startPos;
    }
}