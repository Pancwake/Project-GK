using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShootBase : MonoBehaviour
{
    float shootSpeed;

    GameObject ball;
    BallScript ballScript;
    BallShooter ballShooter;

    Vector3 startPos;
    Vector3 controlA;
    Vector3 controlB;
    Vector3 targetPos;

    float t;
    float duration;
    bool shooting;

    int trajectorySteps = 30;

    Vector3 lastBallPosition;
    Vector3 currentBallPosition;

    Rigidbody ballRB;

    float curveStrength;
    float arcHeight;
    Vector2 curveDirection;

    public void Start()
    {
        ballShooter = GetComponent<BallShooter>();
    }

    public void Update()
    {
        if (!shooting) 
            return;

        ball.transform.position = Bezier(t, startPos, controlA, controlB, targetPos);
        ball.GetComponent<BallScript>().ApplyPosition();

        t += Time.deltaTime / duration;

        //At end of shoot
        if (t >= 1f)
        {
            t = 1f;
            shooting = false;

            ballRB.isKinematic = false;
            ball.GetComponent<BallScript>().ContinueVelocity();
        }

        DebugBezierCurve();
    }

    public void StartShoot(GameObject ball, Vector3 targetPos, float shotSpeed, float curveStrength, float arcHeight, Vector2 curveDirection)
    {
        this.ball = ball;
        this.targetPos = targetPos;
        this.shootSpeed = shotSpeed;
        this.curveStrength = curveStrength;
        this.arcHeight = arcHeight;
        this.curveDirection = curveDirection;

        ballRB = ball.GetComponent<Rigidbody>();
        ballRB.isKinematic = true;

        startPos = ball.transform.position;
        this.targetPos = targetPos;

        float distance = Vector3.Distance(startPos, this.targetPos);
        duration = distance / shotSpeed;

        t = 0f;
        shooting = true;

        // Direction
        Vector3 dir = (this.targetPos - startPos).normalized;

        // Side direction for curve
        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;

        Vector3 curveOffset = right * curveDirection.x + Vector3.up * curveDirection.y;

        // Control points define curve shape
        controlA = startPos + dir * (distance * 0.25f) + Vector3.up * arcHeight;

        controlB = startPos + dir * (distance * 0.75f) + curveOffset * curveStrength + Vector3.up * arcHeight;

        ballScript = ball.GetComponent<BallScript>();
        ballScript.ApplySpin(ballShooter.spinDirection, shotSpeed / 10f);
    }

    Vector3 Bezier(float t, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        float u = 1 - t;

        return
            u * u * u * a +
            3 * u * u * t * b +
            3 * u * t * t * c +
            t * t * t * d;
    }

    void DebugBezierCurve()
    {
        int steps = 30;

        Vector3 prev = startPos;

        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;

            Vector3 point = Bezier(t, startPos, controlA, controlB, targetPos);

            Debug.DrawLine(prev, point, Color.green);

            prev = point;
        }

        // Optional: visualize key points
        Debug.DrawLine(startPos, controlA, Color.yellow);
        Debug.DrawLine(controlA, controlB, Color.yellow);
        Debug.DrawLine(controlB, targetPos, Color.yellow);
    }

    public void ResetShoot()
    {
        shooting = false;
    }
}