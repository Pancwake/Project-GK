using UnityEngine;

public class RisingShoot : ShootBase
{
    [SerializeField] float curveStart = 0.7f;
    [SerializeField] float curveStrength = 2.0f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (shooting)
            CalculateTrajectory();
    }


    public override void StartShoot(GameObject ball, Vector3 targetPos)
    {
        base.StartShoot(ball, targetPos);
    }

    public override void CalculateTrajectory()
    {
        base.CalculateTrajectory();

        Vector3 start = ball.transform.position;
        Vector3 target = targetPos;

        Vector3 prev = start;

        //Draw trajectory in debug
        for (int i = 1; i <= trajectorySteps; i++)
        {
            float t = i / (float)trajectorySteps;

            Vector3 pos = Vector3.Lerp(start, target, t);

            Debug.DrawLine(prev, pos, Color.green);

            prev = pos;
        }
    }

    public override void DoShoot()
    {
        base.DoShoot();
    }

    public override void ResetShoot()
    {
        base.ResetShoot();
    }
}
