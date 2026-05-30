using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] float despawnTime = 1f;

    Vector3 lastPosition;
    Vector3 currentPosition;
    Vector3 currentVelocity;

    [SerializeField] bool ballCatchable;

    Rigidbody rb;
    Collider col;

    bool repelled;

    public bool ballInteractable;

    [SerializeField] float baseSpinSpeed = 1000f;
    Vector3 spinDirection;
    float spinSpeed;

    [SerializeField] GameObject ballMesh;

    //sounds
    bool playedHitFloorSFX;
    bool playedGoalSFX;
    bool playedWindSFX;

    [Header("Hit Floor Sound Speeds")]
    [SerializeField] float minImpactSpeed = 1f;
    [SerializeField] float maxImpactSpeed = 20f;

    [SerializeField] GameObject grassParticle;
    [SerializeField] GameObject asphaltParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballInteractable = true;
        ballCatchable = false;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Only spin if still flying
        if(ballInteractable)
            Spin();
    }

    public void ApplySpin(Vector3 direction, float speedModifier)
    {
        spinDirection = direction.normalized;
        spinSpeed = baseSpinSpeed * speedModifier;
    }

    void Spin()
    {
        ballMesh.transform.Rotate(spinDirection * spinSpeed * Time.deltaTime);
    }

    public void StopInteraction()
    {
        ballInteractable = false;
    }

    public void SetCatchable(bool catchable)
    {
        ballCatchable = catchable;
    }

    public bool GetCatchable()
    {
        return ballCatchable;
    }

    //Calculate velocity depending on positions
    public void ApplyPosition()
    {
        lastPosition = currentPosition;

        currentPosition = transform.position;

        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime;
    }

    public void ContinueVelocity()
    {
        StartDespawn();
        col.isTrigger = false;
        rb.isKinematic = false;
        rb.linearVelocity = currentVelocity;
    }

    public void Catch()
    {
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
        }

        rb.isKinematic = true;     
    }

    public void RepellBall(Vector3 velocity)
    {
        repelled = true;
        col.isTrigger = false;
        rb.isKinematic = false;
        rb.linearVelocity = velocity;
    }

    public void StartDespawn()
    {
        StartCoroutine(DespawnBall());
    }

    IEnumerator DespawnBall()
    {
        yield return new WaitForSeconds(despawnTime);

        DestroyBall();
    }

    public void DestroyBall()
    {
        GameManager.Instance.NextShot();
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (repelled)
        {
            if (collision.transform.CompareTag("Grass"))
            {
                HitGround(EGroundType.Grass);
                StartDespawn(); //Only start despawn after hitting the ground
            }
            else if (collision.transform.CompareTag("Asphalt"))
            {
                HitGround(EGroundType.Asphalt);
            }
        }
        else if (collision.transform.CompareTag("Grass"))
        {
            HitGround(EGroundType.Grass);
        }
        else if (collision.transform.CompareTag("Asphalt"))
        {
            HitGround(EGroundType.Asphalt);
        }
        else if (collision.transform.CompareTag("Goal"))
        {
            if (!playedGoalSFX)
            {
                SoundManager.Instance.PlaySFXFromList(SoundManager.Instance.goalSFX);
                playedGoalSFX = true;
            }  
        }
    }

    void HitGround(EGroundType groundType)
    {
        if (playedHitFloorSFX) //Dont play floor sound twice
            return;

        float speed = rb.linearVelocity.magnitude;

        Debug.Log("Impact speed: " + speed);

        float volumeModifier = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, speed);

        switch (groundType)
        {
            case EGroundType.Asphalt:
                Instantiate(asphaltParticle, transform.position, Quaternion.identity);
                SoundManager.Instance.PlaySFXFromList(SoundManager.Instance.asphaltSFX, volumeModifier);
                break;
            case EGroundType.Grass:
                Instantiate(grassParticle, transform.position, Quaternion.identity);
                SoundManager.Instance.PlaySFXFromList(SoundManager.Instance.grassSFX, volumeModifier);
                break;
        }

        playedHitFloorSFX = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Goal"))
        {
            if (!playedWindSFX)
            {
                SoundManager.Instance.PlaySFXFromList(SoundManager.Instance.windSFX);
                playedWindSFX = true;
            }
        }
    }
}

public enum EGroundType
{
    Asphalt,
    Grass
}