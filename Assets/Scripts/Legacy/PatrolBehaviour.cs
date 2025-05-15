using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private int currentIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        gameObject.TryGetComponent<NavMeshAgent>(out agent);
        if(!agent) 
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[currentIndex].position);
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }
}
