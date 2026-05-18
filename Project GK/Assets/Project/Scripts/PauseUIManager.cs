using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    [SerializeField] GameInfo gameInfo;

    [SerializeField] GameObject pauseMenu;

    CustomCursorHandler cursorHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursorHandler = GetComponent<CustomCursorHandler>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameInfo.paused) //Resume game if already paused
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        cursorHandler.HideCursor();
        pauseMenu.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void ResumeGame()
    {
        cursorHandler.ShowCursor();
        pauseMenu.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    public void ExitGame()
    {
        GameManager.Instance.Exit();
    }
}