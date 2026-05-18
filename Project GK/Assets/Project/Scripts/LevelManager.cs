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

    public void LoadShop()
    {
        SceneManager.LoadScene("UpgradeShop");
    }

    public void LoadStadium()
    {
        SceneManager.LoadScene(gameInfo.currentStadiumIndex);
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinSrceen");
    }

    //Check if all difficulties for this level have been played
    public void LoadNextLevel()
    {
        gameInfo.currentStadiumLevel++; //Increase difficulty by 1

        if (gameInfo.currentStadiumLevel > gameInfo.levelsPerStadium) //If above max difficulty
        {
            Debug.Log("Load next stadium");
            //Reset difficulty
            gameInfo.currentStadiumIndex += 1; //Enter next stadium
            gameInfo.currentStadiumLevel = 1; 
            SceneManager.LoadScene(gameInfo.currentStadiumIndex);
        }
        else
        {
            Debug.Log("Load current stadium difficulty: " + gameInfo.currentStadiumLevel);
            LoadStadium();
        }
    }
}