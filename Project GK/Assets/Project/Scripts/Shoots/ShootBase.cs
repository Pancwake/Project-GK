using UnityEngine;

public class ShootBase : MonoBehaviour
{
    [SerializeField] protected float shootSpeed;

    protected GameObject ball;
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
            DoShoot();
    }

    public virtual void StartShoot(GameObject ball, Vector3 targetPos)
    {
        this.ball = ball;
        this.targetPos = targetPos;
        shooting = true;
    }

    public virtual void CalculateTrajectory()
    {

    }

    public virtual void DoShoot()
    {

    }

    public virtual void EndShoot()
    {
        shooting = false;
    }
}