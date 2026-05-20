using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    MainMenuManager mainMenuManager;

    [SerializeField] GameObject deleteSaveButton;
    [SerializeField] GameObject confirmDeleteSave;
    [SerializeField] GameObject acknowledgeDeletion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuManager = GetComponent<MainMenuManager>();

        ShowDeletionButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressDeleteSave()
    {
        deleteSaveButton.SetActive(false);
        acknowledgeDeletion.SetActive(false);
        confirmDeleteSave.SetActive(true);
    }

    public void PressConfirmDeleteSave()
    {
        deleteSaveButton.SetActive(false);
        confirmDeleteSave.SetActive(false);
        acknowledgeDeletion.SetActive(true);

        mainMenuManager.DeleteSave();
        mainMenuManager.UpdateDifficulties();
    }

    public void ShowDeletionButton()
    {
        confirmDeleteSave.SetActive(false);
        acknowledgeDeletion.SetActive(false);
        deleteSaveButton.SetActive(true);
    }
}