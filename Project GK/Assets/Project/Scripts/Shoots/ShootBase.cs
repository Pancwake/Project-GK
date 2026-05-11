using UnityEngine;

public class ShootBase : MonoBehaviour
{
    [SerializeField] protected float shootSpeed;

    protected GameObject ball;
    protected Vector3 startPos;
    protected Vector3 targetPos;

    protected float shootDuration;
    protected float shootElapsedTime;

    protected bool shooting;

    protected int trajectorySteps = 30;

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        if (shooting)
        {
            CalculateTrajectory();
            DoShoot();
        }
    }

    public virtual void StartShoot(GameObject ball, Vector3 targetPos)
    {
        this.ball = ball;
        this.targetPos = targetPos;

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

    }

    public virtual void ResetShoot()
    {
        shooting = false;
    }
}