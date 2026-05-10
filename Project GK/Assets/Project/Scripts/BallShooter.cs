using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [SerializeField] GameObject goal;
    [SerializeField] GameObject goalMouth;
    [SerializeField] GameObject ballPrefab;
    GameObject spawnedBall;

    [SerializeField] float shootHeightMin;
    [SerializeField] float shootHeightMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }
    }

    void ShootBall()
    {
        Vector2 goalSize = goalMouth.GetComponent<Collider>().bounds.size;

        var goalXMin = goalMouth.transform.position.x - (goalSize.x / 2);
        var goalXMax = goalMouth.transform.position.x + (goalSize.x / 2);
        var goalYMin = goalMouth.transform.position.y - (goalSize.y / 2);
        var goalYMax = goalMouth.transform.position.y + (goalSize.y / 2);

        float rngX = Random.Range(goalXMin, goalXMax);
        float rngY = Random.Range(goalYMin, goalYMax);

        Vector3 shootPosition = new Vector3(rngX, rngY, goal.transform.position.z);
        
        //Instantiate(ballPrefab, shootPosition, Quaternion.identity);
        spawnedBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        LobShoot(shootPosition);
    }

    void LobShoot(Vector3 shootPos)
    {
        float shootHeight = Random.Range(shootHeightMin, shootHeightMax);

        float shootRelativeYPos = shootPos.y - transform.position.y;
        float highPoint = shootRelativeYPos + shootHeight;

        if (shootRelativeYPos < 0)
            highPoint = shootHeight;

        ShootToPosition(shootPos, highPoint);
    }

    void ShootToPosition(Vector3 targetPos, float trajectoryHeight)
    {
        Vector3 velocity = CalculateShootVelocity(spawnedBall.transform.position, targetPos, trajectoryHeight);

        spawnedBall.GetComponent<Rigidbody>().linearVelocity = velocity;
    }

    Vector3 CalculateShootVelocity(Vector3 startPos, Vector3 endPos, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(endPos.x - startPos.x, 0f, endPos.z - startPos.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}