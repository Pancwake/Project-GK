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

        var target = new Vector3(rngX, rngY, goalMouth.transform.position.z);

        //Instantiate(ballPrefab, shootPosition, Quaternion.identity);
        spawnedBall = Instantiate(ballPrefab, ballSpawnPos.position, Quaternion.identity);

        shootScript.StartShoot(spawnedBall, target);

        shooting = true;
    }

    public void CatchBall()
    {
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