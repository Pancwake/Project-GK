using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class BallShooter : MonoBehaviour
{
    [SerializeField] GameObject goal;
    [SerializeField] GameObject goalMouth;
    [SerializeField] GameObject ballPrefab;
    GameObject spawnedBall;

    [SerializeField] public Transform ballSpawnPos;

    [SerializeField] float failSafeTimer = 5f;

    ShootBase shootScript;

    [SerializeField] GameInfo gameInfo;

    [Header("Shoot Parameters")]

    [Header("Speed")]
    [SerializeField] public float baseShootSpeed = 10f;

    [Header("Curve Strength")]
    [SerializeField] public float minCurveStrength = 2f;
    [SerializeField] public float maxCurveStrength = 2f;

    [Header("Arc Height")]
    [SerializeField] public float minArcHeight = 2f;
    [SerializeField] public float maxArcHeight = 2f;

    [Header("Curve Direction")]
    [SerializeField] public Vector2 minCurveDirection = Vector2.up;
    [SerializeField] public Vector2 maxCurveDirection = Vector2.up;

    Vector3 spinDirection = Vector3.right;

    [Header("Visual Spin")]
    [SerializeField] float spinStrengthMultiplier = 0.25f;
    [SerializeField] float minSpinStrength = 1f;
    [SerializeField] float maxSpinStrength = 10;

    float shootSpeed;

    Coroutine failSafeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootScript = GetComponent<ShootBase>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        //First apply difficulty modifier, then the upgrade modifier
        shootSpeed = (baseShootSpeed * gameInfo.difficultySpeedModifier) * gameInfo.upgradeSpeedModifier;

        if(failSafeCoroutine != null)
            StopCoroutine(failSafeCoroutine);

        failSafeCoroutine = StartCoroutine(FailSafe());

        StartShoot();
    }

    public Vector3 GetTarget()
    {
        Vector2 goalSize = goalMouth.GetComponent<Collider>().bounds.size;

        var goalXMin = goalMouth.transform.position.x - (goalSize.x / 2);
        var goalXMax = goalMouth.transform.position.x + (goalSize.x / 2);
        var goalYMin = goalMouth.transform.position.y - (goalSize.y / 2);
        var goalYMax = goalMouth.transform.position.y + (goalSize.y / 2);

        float rngX = Random.Range(goalXMin, goalXMax);
        float rngY = Random.Range(goalYMin, goalYMax);

        var target = new Vector3(rngX, rngY, goal.transform.position.z);

        return target;
    }

    void StartShoot()
    {
        Vector3 target = GetTarget();

        //Instantiate(ballPrefab, shootPosition, Quaternion.identity);
        spawnedBall = Instantiate(ballPrefab, ballSpawnPos.position, Quaternion.identity);

        float curveStrength = Random.Range(minCurveStrength, maxCurveStrength);
        float arcHeight = Random.Range(minArcHeight, maxArcHeight);
        Vector2 curveDirection = RandomBetween(minCurveDirection, maxCurveDirection);

        ApplySpin(curveDirection, arcHeight, shootSpeed, ballSpawnPos.position, target);

        shootScript.StartShoot(spawnedBall, target, shootSpeed, curveStrength, arcHeight, curveDirection);
    }

    void ApplySpin(Vector2 curveDirection, float arcHeight, float shootSpeed, Vector3 startPos, Vector3 targetPos)
    {
        //Apply spin to the ball
        Vector3 spin = CalculateSpin(curveDirection, arcHeight, shootSpeed, ballSpawnPos.position, targetPos);

        Vector3 spinDir = spin.normalized;
        float spinStrength = spin.magnitude;

        //Apply forward spin if shot is straight
        if (spinDir.sqrMagnitude < 0.01f)
        {
            Vector3 forward = (targetPos - startPos).normalized;
            Vector3 fallbackSpinAxis = Vector3.Cross(Vector3.up, forward);
            spinDir = fallbackSpinAxis;
        }

        Debug.Log("Spin strenght before clamp: " + spinStrength);

        spinStrength = Mathf.Clamp(spinStrength, minSpinStrength, maxSpinStrength);

        Debug.Log("Spin strenght after clamp: " + spinStrength);

        float finalSpinStrength = spinStrength * spinStrengthMultiplier * (shootSpeed / 10f);

        Debug.Log("Apply spin");
        var ballScript = spawnedBall.GetComponent<BallScript>();
        ballScript.ApplySpin(spinDir, finalSpinStrength);
    }

    Vector3 CalculateSpin(Vector2 curveDirection, float arcHeight, float shootSpeed, Vector3 startPos, Vector3 targetPos)
    {
        Vector3 forward = (targetPos - startPos).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        // side spin (curl)
        float sideSpin = curveDirection.x;

        // vertical spin (dip / lift)
        float verticalSpin = curveDirection.y;

        // arc adds “backspin feel”
        float arcSpin = arcHeight;

        Vector3 spin =
            (right * sideSpin) +      // curl left/right
            (Vector3.up * verticalSpin) + // lift/drop spin
            (-forward * arcSpin);     // backspin / topspin influence

        return spin * shootSpeed;
    }

    Vector2 RandomBetween(Vector2 a, Vector2 b)
    {
        return new Vector2(
            Random.Range(Mathf.Min(a.x, b.x), Mathf.Max(a.x, b.x)),
            Random.Range(Mathf.Min(a.y, b.y), Mathf.Max(a.y, b.y))
        );
    }

    public void CatchBall()
    {
        spawnedBall.GetComponent<BallScript>().Catch();
        ResetShoot();
    }

    public void RepelBall()
    {
        ResetShoot();
    }

    public void Goal()
    {
        ResetShoot();
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(failSafeTimer);

        Destroy(spawnedBall);
        ResetShoot();
        GameManager.Instance.FailSafe();
    }

    public void ResetShoot()
    {
        shootScript.ResetShoot();
    }
}