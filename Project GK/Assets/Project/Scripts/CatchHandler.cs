using System.Collections;
using UnityEngine;

public class CatchHandler : MonoBehaviour
{
    Camera cam;

    BallShooter ballShooter;

    RaycastHit hit;

    [SerializeField] LayerMask ballLayer;

    [SerializeField] bool canCatch; //If the player can press catch
    [SerializeField] bool isCatching; //If the player is currently trying to catch
    [SerializeField] float catchTime; //How early the button can be pressed to still count as a catch
    [SerializeField] float catchCooldown; //How long the player has to wait before being able to catch again (Anti spam)
    [SerializeField] [Range(0f, 100f)] int catchAreaPercentage; //The percentage of the area that the ball can be caught in
    [SerializeField] float repelForce = 1f;
    [SerializeField] float holdTime = 1f; //Time to hold the ball after catching

    Coroutine catchTimerCoroutine;
    Coroutine catchCooldownCoroutine;

    bool ballInteractedWith;
    bool ballHeld;

    GameObject ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // Time.timeScale = 0.5f;

        cam = Camera.main;

        ballShooter = FindFirstObjectByType<BallShooter>();

        canCatch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ballHeld)
        {
            HoldBall();
            return;
        }

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

        CheckCollission();
    }

    void HoldBall()
    {
        //Hold ball animation
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
        }

        Debug.DrawRay(cam.transform.position, (mousePos - cam.transform.position) * 100f, Color.red);
    }

    bool TryCatching()
    {
        Vector3 hitPosition = hit.point;
        hitPosition.z = hit.transform.position.z; //Level it with the ball for distance to middle check

        var ballRadius = hit.collider.bounds.extents.x;
        var distance = Vector3.Distance(hitPosition, hit.transform.position); //Distance from where hit to the middle of the ball
        var maxDistanceToCatch = ballRadius * (float)(catchAreaPercentage / 100f); //The max distance the mouse can be away to allow a catch

        Debug.Log("Catch: " + distance + " <= " + maxDistanceToCatch + " = " + (distance <= maxDistanceToCatch));

        return distance <= maxDistanceToCatch; //If distance is in the catch threshhold
    }

    public void CatchBall()
    {
        ballHeld = true;
        StartCoroutine(LetGoOfBall());

        GameManager.Instance.CatchBall();

        ResetShot();
    }

    IEnumerator LetGoOfBall()
    {
        yield return new WaitForSeconds(holdTime);

        ballHeld = false;
        ball.GetComponent<BallScript>().DestroyBall();
    }

    void RepelBall()
    {
        var ballScript = hit.collider.GetComponent<BallScript>();

        //Direction = target - start
        var hitDirection = hit.transform.position - hit.point; //Direction from hit point 

        var velocity = hitDirection.normalized * repelForce;

        Debug.Log("Repel velocity: " + velocity);

        ballScript.RepellBall(velocity);

        //ballScript.ContinueVelocity();

        GameManager.Instance.RepelBall();
        ResetShot();
    }

    public void Goal()
    {
        ResetShot();
    }

    void ResetShot()
    {
        ballInteractedWith = false;
    }
}