using UnityEngine;

public class ShootTrajectoryDrawer : MonoBehaviour
{
    BallShooter shooter;

    Vector3 startPos;
    Vector3 controlA;
    Vector3 controlB;
    Vector3 targetPos;

    [Header("Debug Settings")]
    public int samples = 6;
    public int stepsPerTrajectory = 25;
    public float debugDuration = 5f;
    public bool drawKeyPoints = false;

    [ContextMenu("Draw Trajectory Sweep")]
    public void DrawTrajectorySweep()
    {
        shooter = GetComponent<BallShooter>();

        if (shooter == null)
        {
            Debug.LogError("No BallShooter assigned.");
            return;
        }

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)(samples - 1);
            DrawSample(t);
        }
    }

    void DrawSample(float t)
    {
        float curveStrength = Mathf.Lerp(shooter.minCurveStrength, shooter.maxCurveStrength, t);
        float arcHeight = Mathf.Lerp(shooter.minArcHeight, shooter.maxArcHeight, t);

        int rng = UnityEngine.Random.Range(0, shooter.curveDirectionRandoms.Count); //Get a random direction
        CurveDirectionRandom direction = shooter.curveDirectionRandoms[rng];
        Vector2 curveDir = Vector2.Lerp(direction.minCurveDirection, direction.maxCurveDirection, t);

        DebugBezierCurve(curveStrength, arcHeight, curveDir);
    }

    void DebugBezierCurve(float curveStrength, float arcHeight, Vector2 curveDirection)
    {
        startPos = shooter.ballSpawnPos.position;
        targetPos= shooter.GetTarget();
        var shotSpeed = shooter.baseShootSpeed;

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / shotSpeed;

        // Direction
        Vector3 dir = (targetPos - startPos).normalized;

        // Side direction for curve
        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;

        Vector3 curveOffset = right * curveDirection.x + Vector3.up * curveDirection.y;

        // Control points define curve shape
        controlA = startPos + dir * (distance * 0.25f) + curveOffset * curveStrength + Vector3.up * arcHeight;

        controlB = startPos + dir * (distance * 0.75f) + curveOffset * curveStrength + Vector3.up * arcHeight;

        DrawBezierCurve();
    }

    void DrawBezierCurve()
    {
        Vector3 prev = startPos;

        for (int i = 1; i <= stepsPerTrajectory; i++)
        {
            float t = i / (float)stepsPerTrajectory;

            Vector3 point = Bezier(t, startPos, controlA, controlB, targetPos);

            Debug.DrawLine(prev, point, Color.green, debugDuration);

            prev = point;
        }

        if (drawKeyPoints)
        {
            // Optional: visualize key points
            Debug.DrawLine(startPos, controlA, Color.yellow, debugDuration);
            Debug.DrawLine(controlA, controlB, Color.yellow, debugDuration);
            Debug.DrawLine(controlB, targetPos, Color.yellow, debugDuration);
        }
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
}