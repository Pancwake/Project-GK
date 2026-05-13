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
    public UpgradeInfo upgradeInfo;

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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            upgradeInfo.UseUpgrade(EUpgrades.maxHealth);
            gameInfo.Upgrade(EUpgrades.maxHealth);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            upgradeInfo.UseUpgrade(EUpgrades.goalHealthPenalty);
            gameInfo.Upgrade(EUpgrades.goalHealthPenalty);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            upgradeInfo.UseUpgrade(EUpgrades.catchHeal);
            gameInfo.Upgrade(EUpgrades.catchHeal);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            upgradeInfo.UseUpgrade(EUpgrades.pointsMultiplierIncrease);
            gameInfo.Upgrade(EUpgrades.pointsMultiplierIncrease);
        }
    }



    public void CatchBall()
    {
        Debug.Log("Ball caught");
        ChangeHealth(gameInfo.catchHeal);
        AddPoints(gameInfo.catchPoints);
        ballShooter.CatchBall();
    }

    public void RepelBall()
    {
        Debug.Log("Ball repelled");
        AddPoints(gameInfo.repelPoints);
        ballShooter.RepelBall();
    }

    public void Goal()
    {
        Debug.Log("Goal");
        ChangeHealth(gameInfo.goalHealthPenalty);
        gameInfo.combo = 0;
        gameInfo.CalculateMultiplier();
        catchHandler.Goal();
        ballShooter.Goal();
    }

    void ChangeHealth(int amount)
    {
        gameInfo.currentHealth += amount;

    }

    void AddPoints(int points)
    {
        gameInfo.combo += 1;
        gameInfo.CalculateMultiplier();
        gameInfo.points += (int)(points * gameInfo.pointsMultiplier);
    }

    void ResetStats()
    {
        gameInfo.ResetStats();
        upgradeInfo.ResetUpgrades();
        //Do this on Start button instead
    }
}