using NUnit.Framework;
using System.Collections;
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
    [SerializeField] GameObject cheatText;
    [SerializeField] float cheatTextTime = 1f;

    List<KeyCode> konamiCode = new List<KeyCode>
        {
            KeyCode.UpArrow,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.B,
            KeyCode.A,
            KeyCode.Return
        };

    int currentKonamiCodeIndex = 0;

    public List<string> difficultyNames;

    int lastSelectedDifficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;

        cheatText.SetActive(false);

        UpdateDifficulties();
        OpenMainMenu();

        lastSelectedDifficulty = 0;
        difficultyDropDown.value = 0;

        MusicManager.Instance.PlayMusic(MusicManager.Instance.mainMenuMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(konamiCode[currentKonamiCodeIndex]))
        {
            currentKonamiCodeIndex++;

            if (currentKonamiCodeIndex == konamiCode.Count)
            {
                currentKonamiCodeIndex = 0;
                gameInfo.UnlockAllDifficulties();
                UpdateDifficulties();

                cheatText.SetActive(true);
                StartCoroutine(cheatTextDisappearance());
            }
        }
        else if (Input.anyKeyDown)
        {
            currentKonamiCodeIndex = 0;
        }
    }

    IEnumerator cheatTextDisappearance()
    {
        yield return new WaitForSeconds(cheatTextTime);

        cheatText.SetActive(false);
    }

    public void StartGame()
    {
        MusicManager.Instance.StopMusic();

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