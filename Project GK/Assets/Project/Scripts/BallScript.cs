using System.Collections;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public bool enteredGoal; 

    [SerializeField] float despawnTime = 1f;

    public Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Goal()
    {
        enteredGoal = true;
    }

    public void StartDespawn()
    {
        StartCoroutine(DespawnBall());
    }

    IEnumerator DespawnBall()
    {
        yield return new WaitForSeconds(despawnTime);

        Destroy(this.gameObject);
    }
}