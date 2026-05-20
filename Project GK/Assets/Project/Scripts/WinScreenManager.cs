using UnityEngine;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] GameInfo gameInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;

        gameInfo.TryUnlockDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }
}