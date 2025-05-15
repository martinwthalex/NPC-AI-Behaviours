using UnityEngine;
using System.Collections.Generic;

public class PatrolState : IState
{
    #region Variables
    private EnemyAI enemy;
    private int currentIndex = 0;
    private Transform[] waypoints;

    private List<Transform> shuffledWaypoints;
    #endregion

    public PatrolState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("NPC does not have waypoints assigned. Cannot enter PatrolState.");
            return;
        }
        
        enemy.agent.speed = 1.5f;
        enemy.SetColor(enemy.patrolColor);
        enemy.agent.SetDestination(waypoints[currentIndex].position);
    }

    public void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
        {
            // Check if the enemy has reached the current waypoint
            // If so, move to the next waypoint based on the patrol type
            switch (enemy.patrolType)
            {
                case EnemyAI.PatrolType.Fixed:
                    // Fixed patrol type, just move to the next waypoint
                    currentIndex = (currentIndex + 1) % waypoints.Length;
                    enemy.agent.SetDestination(waypoints[currentIndex].position);
                    break;

                case EnemyAI.PatrolType.Random:
                    // Random patrol type, pick a random waypoint
                    currentIndex = Random.Range(0, waypoints.Length);
                    enemy.agent.SetDestination(waypoints[currentIndex].position);
                    break;

                case EnemyAI.PatrolType.Custom:
                    // Custom patrol type, use the shuffled waypoints
                    currentIndex = (currentIndex + 1) % shuffledWaypoints.Count;

                    if (currentIndex == 0) // If we have looped through all waypoints, shuffle again
                    {
                        ShuffleList(shuffledWaypoints);
                    }

                    enemy.agent.SetDestination(shuffledWaypoints[currentIndex].position);
                    break;

            }

        }
    }


    public void SetWaypoints(Transform[] wps)
    {
        waypoints = wps;
        currentIndex = 0;

        if (enemy.patrolType == EnemyAI.PatrolType.Custom)
        {
            shuffledWaypoints = new List<Transform>(waypoints);
            ShuffleList(shuffledWaypoints);
        }
    }

    private void ShuffleList(List<Transform> list)
    {
        // Shuffle the list of waypoints
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }



    public void Exit() { }
}
