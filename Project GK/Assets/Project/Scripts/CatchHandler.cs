using UnityEngine;

public class CatchHandler : MonoBehaviour
{
    Camera cam;

    BallShooter ballShooter;

    bool canCatch;

    RaycastHit hit;

    [SerializeField] LayerMask ballLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        ballShooter = FindFirstObjectByType<BallShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canCatch)
            CheckCollission();
    }

    void CheckCollission()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = 10f;

        mousePos = cam.ScreenToWorldPoint(mousePos);

        if (Physics.Raycast(cam.transform.position, mousePos - cam.transform.position, out hit, 100f, ballLayer))
        {
            TryCatching();
        }

        Debug.DrawRay(cam.transform.position, (mousePos - cam.transform.position) * 100f, Color.red);
    }

    void TryCatching()
    {

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
        canCatch = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            canCatch = true;
        }
    }
}
