using UnityEngine;
using UnityEngine.AI;

public class VisionChaseBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask obstacleMask;

    private NavMeshAgent agent;

    void Start()
    {
        gameObject.TryGetComponent<NavMeshAgent>(out agent);
        if(!agent) 
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            agent.SetDestination(player.position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer < viewRadius)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer, distToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Draw field of view in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
