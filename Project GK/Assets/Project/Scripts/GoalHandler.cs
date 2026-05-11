using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoalHandler : MonoBehaviour
{
    BallShooter ballShooter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballShooter = FindFirstObjectByType<BallShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Goal()
    {
        Debug.Log("GOAL!");

        ballShooter.EndShoot();
    }
}
