using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class CatchHandler : MonoBehaviour
{
    [SerializeField] GameObject handsCatchPrefab;
    [SerializeField] GameObject handsRepelPrefab;

    [SerializeField] float maxCatchRotationDistance = 4f; //The catch distance for the hands to be fully rotated

    GameObject spawnedCatchObject;
    GameObject spawnedRepelObject;

    Camera cam;

    RaycastHit hit;

    [SerializeField] LayerMask ballLayer;

    [SerializeField] bool canCatch; //If the player can press catch
    [SerializeField] bool isCatching; //If the player is currently trying to catch
    [SerializeField] float catchTime; //How early the button can be pressed to still count as a catch
    [SerializeField] float catchCooldown; //How long the player has to wait before being able to catch again (Anti spam)
    [SerializeField] float repelForce = 1f;
    [SerializeField] float holdTime = 1f; //Time to hold the ball after catching
    [SerializeField] float repelTime = 1f; //Time for the repel hands to stay after repelling
    [SerializeField] float minForwardRepelDirection = 0.1f; //The minimum forward velocity a repelled ball needs to have (prevents it from going into the goal after repel)

    Coroutine catchTimerCoroutine;
    Coroutine catchCooldownCoroutine;

    public bool ballInteractedWith;
    public bool ballHeld;

    GameObject ball;

    [SerializeField] GameInfo gameInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // Time.timeScale = 0.5f;

        cam = Camera.main;

        canCatch = true;
    }

    // Update is called once per frame
    void Update()
    {
        DrawDebugs();

        if (gameInfo.paused)
            return;

        if (ballHeld)
            return;

        if (canCatch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isCatching) //If already trying to catch
                {
                    StopCoroutine(catchTimerCoroutine); //Reset timer
                }

                isCatching = true;
                catchTimerCoroutine = StartCoroutine(CatchTimer());

                canCatch = false;
                catchCooldownCoroutine = StartCoroutine(CatchCooldown());
            }
        }
        
        if (isCatching)
            CheckCollission();
    }

    void DrawDebugs()
    {
        //Show catch area debug
        float minZ = transform.position.z - 0.3f;
        float maxZ = transform.position.z + gameInfo.goalAreaSize;
        Vector3 minVec = new Vector3(transform.position.x, transform.position.y, minZ);
        Vector3 maxVec = new Vector3(transform.position.x, transform.position.y, maxZ);
        Vector3 dir = maxVec - minVec;
        float distance = Vector3.Distance(minVec, maxVec);
        Debug.DrawLine(minVec, maxVec, Color.green);
    }

    IEnumerator CatchTimer()
    {
        yield return new WaitForSeconds(catchTime);

        isCatching = false;
    }

    IEnumerator CatchCooldown()
    {
        yield return new WaitForSeconds(catchCooldown);

        canCatch = true;
    }

    void CheckCollission()
    {
        if (ballInteractedWith) //If ball already interacted (prevents constant checking)
            return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = cam.ScreenToWorldPoint(mousePos);

        if (Physics.Raycast(cam.transform.position, mousePos - cam.transform.position, out hit, 100f, ballLayer)) //If ball hit
        {
            ball = hit.collider.gameObject;
            
            if (!ball.GetComponent<BallScript>().ballInteractable)
                return;

            float minZ = transform.position.z - 1f;
            float maxZ = transform.position.z + gameInfo.goalAreaSize;

            //Catch area float
            if (hit.transform.position.z >= minZ && hit.transform.position.z <= maxZ) //If ball is in the catch area
            {
                if (TryCatching()) //If ball caught
                {
                    CatchBall();
                }
                else
                {
                    RepelBall();
                }

                ballInteractedWith = true;
            }

            //Catch area Trigger
            /*
            if (hit.collider.GetComponent<BallScript>().GetCatchable()) //If ball can be caught currently
            {
                if (TryCatching()) //If ball caught
                {
                    CatchBall();
                }
                else
                {
                    RepelBall();
                }

                ballInteractedWith = true;
            }
            */
        }
        else //If raycast didn't hit anything
        {
            Vector3 direction = (mousePos - cam.transform.position).normalized;

            if (Physics.BoxCast(cam.transform.position, Vector3.one * gameInfo.forgivingRepelRadius, direction, out hit, Quaternion.identity, 100f, ballLayer))
            {
                ball = hit.collider.gameObject;

                if (!ball.GetComponent<BallScript>().ballInteractable)
                    return;

                float minZ = transform.position.z - 1f;
                float maxZ = transform.position.z + gameInfo.goalAreaSize;

                //Catch area float
                if (hit.transform.position.z >= minZ && hit.transform.position.z <= maxZ) //If ball is in the catch area
                {
                    RepelBall(); //Only repel ball with boxcast so catching requires more precision

                    ballInteractedWith = true;
                }
            }
        }

        //Debug
        Vector3 castDirection = (mousePos - cam.transform.position).normalized;
        DrawBoxCast.DrawBoxCastBox(cam.transform.position, Vector3.one * gameInfo.forgivingRepelRadius, Quaternion.identity, castDirection, 100f, Color.yellow);
        Debug.DrawRay(cam.transform.position, (mousePos - cam.transform.position) * 100f, Color.red);
    }

    bool TryCatching()
    {
        Vector3 hitPosition = hit.point;
        hitPosition.z = hit.transform.position.z; //Level it with the ball for distance to middle check

        var ballRadius = hit.collider.bounds.extents.x;
        var distance = Vector3.Distance(hitPosition, hit.transform.position); //Distance from where hit to the middle of the ball
        var maxDistanceToCatch = ballRadius * (float)(gameInfo.catchAreaPercentage / 100f); //The max distance the mouse can be away to allow a catch

        return distance <= maxDistanceToCatch; //If distance is in the catch threshhold
    }

    public void CatchBall()
    {
        ballHeld = true;
        ball.GetComponent<BallScript>().StopInteraction();
        SpawnCatchHands();
        StartCoroutine(LetGoOfBall());

        GameManager.Instance.CatchBall();
    }

    void SpawnCatchHands()
    {
        Vector3 middle = transform.position;
        Vector3 catchPosition = new Vector3(ball.transform.position.x, ball.transform.position.y, transform.position.z);
        float catchDistance = Vector3.Distance(middle, catchPosition); //How far away the ball was from the middle when caught
        Vector3 catchDirection = (catchPosition - middle).normalized; //Where the ball was caught relative to the player position (middle of the goal)

        // ADD THIS: normalize distance into 0–1
        float t = Mathf.Clamp01(catchDistance / maxCatchRotationDistance);

        // base rotation (no tilt)
        Quaternion baseRotation = Quaternion.identity;

        // full directional rotation (your original logic)
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, catchDirection);

        // blend between them
        Quaternion rotation = Quaternion.Slerp(baseRotation, targetRotation, t);

        spawnedCatchObject = Instantiate(handsCatchPrefab, ball.transform.position, rotation);
    }

    IEnumerator LetGoOfBall()
    {
        yield return new WaitForSeconds(holdTime);

        if (spawnedCatchObject != null)
            Destroy(spawnedCatchObject);

        ballHeld = false;
        ResetShot();
        ball.GetComponent<BallScript>().DestroyBall();
    }

    void RepelBall()
    {
        var ballScript = hit.collider.GetComponent<BallScript>();

        ballScript.GetComponent<BallScript>().StopInteraction();

        //Direction = target - start
        //World direction
        var hitDirection = (hit.transform.position - hit.point).normalized; //Direction from hit point 

        //Convert to local direction (to get local forward and not rely on world space)
        Vector3 localDir = transform.InverseTransformDirection(hitDirection);

        //Check if its going less than the minimum
        if (localDir.z < minForwardRepelDirection)
            localDir.z = minForwardRepelDirection;

        //Convert to world space
        hitDirection = transform.TransformDirection(localDir).normalized;

        SpawnRepelHands(hitDirection);

        var velocity = hitDirection.normalized * repelForce;

        ballScript.RepellBall(velocity);

        GameManager.Instance.RepelBall();
    }

    void SpawnRepelHands(Vector3 hitDirection)
    {
        //Get rotation from hit direction
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitDirection);

        spawnedRepelObject = Instantiate(handsRepelPrefab, ball.transform.position, rotation);

        StartCoroutine(RemoveRepelHands());
    }

    IEnumerator RemoveRepelHands()
    {
        yield return new WaitForSeconds(repelTime);

        if (spawnedRepelObject != null)
            Destroy(spawnedRepelObject);
    }


    public void Goal()
    {
        ResetShot();
    }

    public void ResetShot()
    {
        ballInteractedWith = false;
    }
}