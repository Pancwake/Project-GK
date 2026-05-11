using System.Collections;
using UnityEngine;

public class CatchHandler : MonoBehaviour
{
    Camera cam;

    BallShooter ballShooter;

    [SerializeField] bool ballCatchable;

    RaycastHit hit;

    [SerializeField] LayerMask ballLayer;

    [SerializeField] bool canCatch; //If the player can press catch
    [SerializeField] bool isCatching; //If the player is currently trying to catch
    [SerializeField] float catchTime; //How early the button can be pressed to still count as a catch
    [SerializeField] float catchCooldown; //How long the player has to wait before being able to catch again (Anti spam)
    Coroutine catchTimerCoroutine;
    Coroutine catchCooldownCoroutine;

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
        if (ballCatchable)
            CheckCollission();

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
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = 10f;

        mousePos = cam.ScreenToWorldPoint(mousePos);

        if (Physics.Raycast(cam.transform.position, mousePos - cam.transform.position, out hit, 100f, ballLayer)) //If ball hit
        {
            if(TryCatching()) //If ball caught
            {
                CatchBall();
            }
        }

        Debug.DrawRay(cam.transform.position, (mousePos - cam.transform.position) * 100f, Color.red);
    }

    bool TryCatching()
    {
        //Add center check

        return isCatching;
    }

    public void CatchBall()
    {
        GameManager.Instance.CatchBall();
        ballShooter.CatchBall();

        ResetShot();
    }

    void RepelBall()
    {

    }

    public void Goal()
    {
        ResetShot();
    }

    void ResetShot()
    {
        ballCatchable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballCatchable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballCatchable = false;
        }
    }
}
