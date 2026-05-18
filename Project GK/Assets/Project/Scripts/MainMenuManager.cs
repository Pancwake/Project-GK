using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameInfo gameInfo;

    [Header("Difficulties")]
    [SerializeField] List<DifficultyInfo> difficulties;

    [SerializeField] TMP_Dropdown difficultyDropDown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;

        difficultyDropDown.value = 0;
        SelectDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        gameInfo.ResetStats();

        LevelManager.Instance.LoadStadium();
    }

    public void SelectDifficulty()
    {
        int difficultyIndex = difficultyDropDown.value;

        Debug.Log("Difficulty selected: " + difficulties[difficultyIndex].name);

        gameInfo.SelectDifficulty(difficulties[difficultyIndex]);
    }
}