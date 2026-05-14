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
    GoalHandler goalHandler;
    BallShooter ballShooter;

    public GameInfo gameInfo;

    public int currentShot;

    [Header("Stadium stats")]
    [SerializeField] int damageForGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catchHandler = FindFirstObjectByType<CatchHandler>();
        goalHandler = FindFirstObjectByType<GoalHandler>();
        ballShooter = FindFirstObjectByType<BallShooter>();

        ResetStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CatchBall()
    {
        Debug.Log("Ball caught");
        ChangeHealthPercentage(gameInfo.catchHealPercentage);
        AddPoints(gameInfo.catchMoneyReward);
        ballShooter.CatchBall();

        NextShot();
    }

    public void RepelBall()
    {
        Debug.Log("Ball repelled");
        AddPoints(gameInfo.repelMoneyReward);
        ballShooter.RepelBall();

        NextShot();
    }

    public void Goal()
    {
        Debug.Log("Goal");
        ChangeHealth(damageForGoal);
        gameInfo.combo = 0;
        gameInfo.CalculateMultiplier();
        catchHandler.Goal();
        ballShooter.Goal();
    }

    void ChangeHealth(int amount)
    {
        gameInfo.currentHealth += amount;

        gameInfo.currentHealth = Mathf.Clamp(gameInfo.currentHealth, 0, gameInfo.maxHealth);

        if (gameInfo.currentHealth <= 0)
        {
            LevelManager.Instance.LoadMainMenu();
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

    void NextShot()
    {
        currentShot++;

        if (currentShot > gameInfo.shotsPerLevel) //If all shots saved this level
        {
            LevelManager.Instance.LoadShop();
        }
    }

    void ResetStats()
    {
        currentShot = 1;
    }
}