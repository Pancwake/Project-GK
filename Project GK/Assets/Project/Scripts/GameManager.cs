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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catchHandler = FindFirstObjectByType<CatchHandler>();
        goalHandler = FindFirstObjectByType<GoalHandler>();
        ballShooter = FindFirstObjectByType<BallShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CatchBall()
    {
        Debug.Log("Ball caught");
        goalHandler.CatchBall();
        ballShooter.CatchBall();
    }

    public void Goal()
    {
        Debug.Log("Goal");
        catchHandler.Goal();
        ballShooter.Goal();
    }
}
