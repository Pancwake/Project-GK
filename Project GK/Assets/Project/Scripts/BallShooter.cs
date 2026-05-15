using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class BallShooter : MonoBehaviour
{
    [SerializeField] GameObject goal;
    [SerializeField] GameObject goalMouth;
    [SerializeField] GameObject ballPrefab;
    GameObject spawnedBall;

    [SerializeField] Transform ballSpawnPos;

    bool shooting;

    ShootBase shootScript;

    [SerializeField] GameInfo gameInfo;

    [Header("Shoot Parameters")]

    [Header("Speed")]
    [SerializeField] float baseShootSpeed = 10f;

    [Header("Curve Strength")]
    [SerializeField] float minCurveStrength = 2f;
    [SerializeField] float maxCurveStrength = 2f;

    [Header("Arc Height")]
    [SerializeField] float minArcHeight = 2f;
    [SerializeField] float maxArcHeight = 2f;

    [Header("Curve Direction")]
    [SerializeField] Vector2 minCurveDirection = Vector2.up;
    [SerializeField] Vector2 maxCurveDirection = Vector2.up;

    [Header("Visual Spin")]
    [SerializeField] public Vector3 spinDirection = Vector3.right;

    float shootSpeed;

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

        StartShoot();
    }

    void StartShoot()
    {
        Vector2 goalSize = goalMouth.GetComponent<Collider>().bounds.size;

        var goalXMin = goalMouth.transform.position.x - (goalSize.x / 2);
        var goalXMax = goalMouth.transform.position.x + (goalSize.x / 2);
        var goalYMin = goalMouth.transform.position.y - (goalSize.y / 2);
        var goalYMax = goalMouth.transform.position.y + (goalSize.y / 2);

        float rngX = Random.Range(goalXMin, goalXMax);
        float rngY = Random.Range(goalYMin, goalYMax);

        var target = new Vector3(rngX, rngY, goal.transform.position.z);

        //Instantiate(ballPrefab, shootPosition, Quaternion.identity);
        spawnedBall = Instantiate(ballPrefab, ballSpawnPos.position, Quaternion.identity);

        float curveStrength = Random.Range(minCurveStrength, maxCurveStrength);
        float arcHeight = Random.Range(minArcHeight, maxArcHeight);
        Vector2 curveDirection = RandomBetween(minCurveDirection, maxCurveDirection);

        shootScript.StartShoot(spawnedBall, target, shootSpeed, curveStrength, arcHeight, curveDirection);

        shooting = true;
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

    public void ResetShoot()
    {
        shootScript.ResetShoot();
        shooting = false;
    }
}