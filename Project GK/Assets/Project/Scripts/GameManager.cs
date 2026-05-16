using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    CatchHandler catchHandler;
    BallShooter ballShooter;
    GameplayUIManager gameplayUIManager;

    public GameInfo gameInfo;

    public int currentShot;

    [Header("Stadium stats")]
    [SerializeField] int damageForGoal;

    [SerializeField] float timeBetweenShots = 1f;

    public bool levelOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catchHandler = FindFirstObjectByType<CatchHandler>();
        ballShooter = FindFirstObjectByType<BallShooter>();
        gameplayUIManager = FindFirstObjectByType<GameplayUIManager>();

        ResetStats();

        gameInfo.CalculateDifficulty();

        NextShot();

        gameplayUIManager.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator startShootTimer()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        ballShooter.Shoot();
    }

    public void FailSafe()
    {
        EndShot(false);
    }

    public void CatchBall()
    {
        Debug.Log("Ball caught");
        SoundManager.Instance.PlaySFXFromList(SoundManager.Instance.catchSFX);
        ChangeHealthPercentage(gameInfo.catchHealPercentage);

        int healAmount = (int)((float)gameInfo.maxHealth * ((float)gameInfo.catchHealPercentage / 100f)); //Get 10% of max health to heal
        gameplayUIManager.ShowCatchText(healAmount);

        AddPoints(gameInfo.catchMoneyReward);
        ballShooter.CatchBall();

        EndShot(true);
        gameplayUIManager.UpdateUI();
    }

    public void RepelBall()
    {
        Debug.Log("Ball repelled");
        SoundManager.Instance.PlaySFXFromList(SoundManager.Instance.repelSFX);
        AddPoints(gameInfo.repelMoneyReward);
        ballShooter.RepelBall();

        EndShot(true);
        gameplayUIManager.UpdateUI();
    }

    public void Goal()
    {
        Debug.Log("Goal");
        ChangeHealth(damageForGoal);
        gameInfo.combo = 0;
        gameInfo.CalculateMultiplier();
        catchHandler.Goal();
        ballShooter.Goal();

        //NextShot();
        gameplayUIManager.UpdateUI();
    }

    void ChangeHealth(int amount)
    {
        gameInfo.currentHealth += amount;

        gameInfo.currentHealth = Mathf.Clamp(gameInfo.currentHealth, 0, gameInfo.maxHealth);

        if (gameInfo.currentHealth <= 0)
        {
            levelOver = true;
            StartCoroutine(LoadDelay(ESceneToLoad.loseScreen));
        }
    }

    void ChangeHealthPercentage(int percentage)
    {
        int healAmount = (int)((float)gameInfo.maxHealth * ((float)gameInfo.catchHealPercentage / 100f)); //Get 10% of max health to heal
        ChangeHealth(healAmount);
    }

    void AddPoints(int points)
    {
        gameInfo.combo += 1;
        gameInfo.CalculateMultiplier();
        gameInfo.money += (int)(points * gameInfo.moneyMultiplier);
    }

    void EndShot(bool saved)
    {
        if (saved)
            currentShot++;

        if (currentShot > gameInfo.shotsPerLevel) //If all shots saved this level
        {
            levelOver = true;

            if (gameInfo.currentStadiumIndex >= gameInfo.stadiumAmount) //If at last stadium load win scene
            {
                StartCoroutine(LoadDelay(ESceneToLoad.winScreen));
            }
            else //Load shop after every level
            {
                StartCoroutine(LoadDelay(ESceneToLoad.shop));
            }     
        }
    }

    public void NextShot()
    {
        catchHandler.ResetShot();
        StartCoroutine(startShootTimer());
    }

    IEnumerator LoadDelay(ESceneToLoad scene)
    {
        yield return new WaitForSeconds(timeBetweenShots);

        switch (scene)
        {
            case ESceneToLoad.shop:
                LevelManager.Instance.LoadShop();
                break;
            case ESceneToLoad.winScreen:
                LevelManager.Instance.LoadWinScene();
                break;
            case ESceneToLoad.loseScreen:
                LevelManager.Instance.LoadLoseScene();
                break;
        }
    }

    void ResetStats()
    {
        currentShot = 1;
    }
}

public enum ESceneToLoad
{
    shop,
    winScreen,
    loseScreen,
}