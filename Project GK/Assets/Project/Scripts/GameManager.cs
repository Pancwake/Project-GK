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
        ChangeHealth(gameInfo.catchHeal);
        AddPoints(gameInfo.catchMoneyReward);
        ballShooter.CatchBall();
    }

    public void RepelBall()
    {
        Debug.Log("Ball repelled");
        AddPoints(gameInfo.repelMoneyReward);
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
        gameInfo.money += (int)(points * gameInfo.moneyMultiplier);
    }

    void ResetStats()
    {

    }
}