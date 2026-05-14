using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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

    [SerializeField] GameInfo gameInfo;

    int currentLevelIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        //Load first level
        currentLevelIndex = 1;
        SceneManager.LoadScene(currentLevelIndex); 
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("UpgradeShop");
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }

    //Check if all difficulties for this level have been played
    public void LoadNextLevel()
    {
        gameInfo.currentStadiumLevel++; //Increase difficulty by 1

        if (gameInfo.currentStadiumLevel > gameInfo.levelsPerStadium) //If above max difficulty
        {
            gameInfo.currentStadiumLevel = 1; //Reset difficulty
            currentLevelIndex += 1;
            SceneManager.LoadScene(currentLevelIndex);
        }
        else
        {
            LoadCurrentLevel();
        }
    }
}