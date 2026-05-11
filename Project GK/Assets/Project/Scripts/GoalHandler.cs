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

    public void CatchBall()
    {

    }

    public void Goal(GameObject ball)
    {
        BallScript ballScript = ball.GetComponent<BallScript>();
        if (ballScript.enteredGoal)
            return;

        ballScript.StartDespawn();
        Rigidbody ballRB = ball.GetComponent<Rigidbody>();
        ballRB.isKinematic = false;
        ballRB.linearVelocity = ballScript.velocity;

        Debug.Log("Set velocity");

        ballScript.Goal();

        GameManager.Instance.Goal();

        ballShooter.ResetShoot();
    }
}
