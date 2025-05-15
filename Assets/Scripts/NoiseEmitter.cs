using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    public float noiseRadius = 6f;
    public LayerMask npcMask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // simulates the noise emission
        {
            EmitNoise();
        }
    }

    void EmitNoise()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noiseRadius, npcMask);
        
        foreach (var hit in hitColliders)
        {
            EnemyAI enemy = hit.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.HearNoise(transform.position);
            }            
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }

}
