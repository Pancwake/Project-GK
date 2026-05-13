using UnityEngine;

public class GoalMouthHandler : MonoBehaviour
{
    GoalHandler goalHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalHandler = GetComponentInParent<GoalHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            goalHandler.Goal(other.gameObject);
        }
    }
}