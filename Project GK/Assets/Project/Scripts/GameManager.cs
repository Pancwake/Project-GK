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
        TakeDamage();
        catchHandler.Goal();
        ballShooter.Goal();
    }

    void TakeDamage()
    {
        gameInfo.currentHealth -= gameInfo.goalHealthPenalty;
        gameInfo.combo = 0;
        gameInfo.CalculateMultiplier();
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
    }
}