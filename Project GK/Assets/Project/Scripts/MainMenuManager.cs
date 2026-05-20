using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameInfo gameInfo;

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] TMP_Dropdown difficultyDropDown;


    public List<string> difficultyNames;

    int lastSelectedDifficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;

        UpdateDifficulties();
        OpenMainMenu();

        lastSelectedDifficulty = 0;
        difficultyDropDown.value = 0;
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

        //difficultyIndex = 2, unlockedDifficulties = 1 (Cant select)
        //difficultyIndex = 1, unlockedDifficulties = 1 (Can select)
        if (difficultyIndex > gameInfo.unlockedDifficulties) //If this difficulty can't be selected
        {
            if (lastSelectedDifficulty > gameInfo.unlockedDifficulties) //If selected difficulty no longer available
            {
                lastSelectedDifficulty = 0;
            }

            difficultyDropDown.value = lastSelectedDifficulty;
            return;
        }

        lastSelectedDifficulty = difficultyIndex;
        gameInfo.SelectDifficulty(difficultyIndex);
    }

    public void UpdateDifficultySelection()
    {
        difficultyDropDown.ClearOptions();

        List<string> difficultyOptions = new List<string>();

        for (int i = 0; i < difficultyNames.Count; i++)
        {
            //i = 2, unlockedDifficulties = 1 (Set locked)
            //i = 0, unlockedDifficulties = 1 (Set to name)
            if (i <= gameInfo.unlockedDifficulties)
            {
                difficultyOptions.Add(difficultyNames[i]);
            }
            else
            {
                difficultyOptions.Add("Locked");
            }
        }

        difficultyDropDown.AddOptions(difficultyOptions);
    }

    public void OpenOptions()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OpenMainMenu()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    //Update difficulties if save was deleted
    public void UpdateDifficulties()
    {
        gameInfo.GetUnlockedDifficulties();
        UpdateDifficultySelection();
        SelectDifficulty();

    }

    public void DeleteSave()
    {
        gameInfo.DeleteSave();
    }
}