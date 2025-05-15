using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    #region Variables
    
    private IState currentState;

    [Header("Dependencies")]
    [SerializeField] public Transform player;
    private Transform[] waypoints;

    [HideInInspector] public NavMeshAgent agent;

    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public SearchState searchState;

    [Header("Vision Settings")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Hearing Settings")]
    public float hearingRadius = 6f;

    public enum PatrolType { Fixed, Random, Custom }

    [Header("Patrol Settings")]
    public PatrolType patrolType = PatrolType.Fixed;

    [Header("Visual Feedback")]
    public Renderer bodyRenderer;
    public Color patrolColor = Color.blue;
    public Color alertColor = Color.red;
    public Color hearingColor = Color.yellow;


    [Header("Animation")]
    public Animator animator;

    #endregion

    void Awake()
    {
        // Initialize the NavMeshAgent and player references
        gameObject.TryGetComponent<NavMeshAgent>(out agent);
        if(!agent) 
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            
        if(!player) player = GameObject.FindGameObjectWithTag("Player").transform;

        // Initialize the states
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        searchState = new SearchState(this);

    }    

    void Update()
    {
        // Update the current state
        currentState?.Update();
        UpdateAnimation();
    }

    public void ChangeState(IState newState)
    {
        // Exit the current state and enter the new state
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetWaypoints(Transform[] wps){
        
        // Set the waypoints for the patrol state
        waypoints = wps;
        patrolState.SetWaypoints(waypoints);

        if (currentState == null)
            ChangeState(patrolState);
    }

    public bool CanSeePlayer()
    {
        
        // Check if the player is within the view radius and angle
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

    public void HearNoise(Vector3 noisePosition)
    {
        
        // Check if the enemy can hear the noise
        if (!CanSeePlayer() && Vector3.Distance(transform.position, noisePosition) <= hearingRadius)
        {
            searchState.SetSearchTarget(noisePosition);
            ChangeState(searchState);

        }
    }


    void OnDrawGizmosSelected()
    {
        
        // Draw the view radius and angle in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    Vector3 DirFromAngle(float angleInDegrees)
    {
        // Calculate the direction from the angle
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SetColor(Color color)
    {
        if (bodyRenderer != null)
            bodyRenderer.material.color = color;
    }

    void UpdateAnimation()
    {
        // Update the animation based on the NavMeshAgent's speed
        if (animator == null || agent == null)
            return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

}
