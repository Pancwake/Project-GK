using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShootBase : MonoBehaviour
{
    protected float shootSpeed;

    protected GameObject ball;
    protected BallScript ballScript;
    protected BallShooter ballShooter;
    protected Vector3 startPos;
    protected Vector3 targetPos;

    protected float shootDuration;
    protected float shootElapsedTime;

    protected bool shooting;

    protected int trajectorySteps = 30;

    Vector3 lastBallPosition;
    Vector3 currentBallPosition;

    public virtual void Start()
    {
        ballShooter = GetComponent<BallShooter>();
    }

    public virtual void Update()
    {
        if (shooting)
        {
            CalculateTrajectory();
            DoShoot();
        }
    }

    public virtual void StartShoot(GameObject ball, Vector3 targetPos, float speed)
    {
        this.shootSpeed = speed;
        this.ball = ball;
        ballScript = ball.GetComponent<BallScript>();
        this.targetPos = targetPos;

        lastBallPosition = Vector3.zero;

        startPos = ball.transform.position;

        shootDuration = Vector3.Distance(startPos, targetPos) / shootSpeed;

        shootElapsedTime = 0f;
        shooting = true;
    }

    public virtual void CalculateTrajectory()
    {

    }

    public virtual void DoShoot()
    {
        //Caluclate velocity from position difference
        currentBallPosition = ball.transform.position;

        if (lastBallPosition != Vector3.zero)
        {
            var velocity = (currentBallPosition - lastBallPosition) / Time.deltaTime;
            ballScript.currentVelocity = velocity;
        }

        lastBallPosition = ball.transform.position;
    }

    public virtual void ResetShoot()
    {
        shooting = false;
    }
}