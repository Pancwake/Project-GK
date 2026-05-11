using UnityEngine;

public class StraightShoot : ShootBase
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
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

        shootElapsedTime += Time.deltaTime;

        float t = shootElapsedTime / shootDuration;
        t = Mathf.Clamp01(t);

        ball.transform.position = Vector3.Lerp(startPos, targetPos, t);
    }

    public override void ResetShoot()
    {
        base.ResetShoot();
    }
}
