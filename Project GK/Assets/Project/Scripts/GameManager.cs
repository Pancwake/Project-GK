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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catchHandler = FindFirstObjectByType<CatchHandler>();
        ballShooter = FindFirstObjectByType<BallShooter>();
        gameplayUIManager = FindFirstObjectByType<GameplayUIManager>();

        ResetStats();

        gameInfo.CalculateDifficulty();

        NextShot(false);

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

    public void CatchBall()
    {
        Debug.Log("Ball caught");
        ChangeHealthPercentage(gameInfo.catchHealPercentage);
        AddPoints(gameInfo.catchMoneyReward);
        ballShooter.CatchBall();

        NextShot(true);
        gameplayUIManager.UpdateUI();
    }

    public void RepelBall()
    {
        Debug.Log("Ball repelled");
        AddPoints(gameInfo.repelMoneyReward);
        ballShooter.RepelBall();

        NextShot(true);
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

        NextShot(false);
        gameplayUIManager.UpdateUI();
    }

    void ChangeHealth(int amount)
    {
        gameInfo.currentHealth += amount;

        gameInfo.currentHealth = Mathf.Clamp(gameInfo.currentHealth, 0, gameInfo.maxHealth);

        if (gameInfo.currentHealth <= 0)
        {
            StartCoroutine(LoseDelay());
        }
    }

    void ChangeHealthPercentage(int percentage)
    {
        int healAmount = (int)(gameInfo.maxHealth * (percentage / 100)); //Get 10% of max health to heal
        ChangeHealth(healAmount);
    }

    void AddPoints(int points)
    {
        gameInfo.combo += 1;
        gameInfo.CalculateMultiplier();
        gameInfo.money += (int)(points * gameInfo.moneyMultiplier);
    }

    void NextShot(bool saved)
    {
        if (saved)
            currentShot++;

        if (currentShot > gameInfo.shotsPerLevel) //If all shots saved this level
        {
            StartCoroutine(ShopDelay());
        }
        else
        {
            StartCoroutine(startShootTimer());
        }
    }

    IEnumerator ShopDelay()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        LevelManager.Instance.LoadShop();
    }

    IEnumerator LoseDelay()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        LevelManager.Instance.LoadLoseScene();
    }

    void ResetStats()
    {
        currentShot = 1;
    }
}