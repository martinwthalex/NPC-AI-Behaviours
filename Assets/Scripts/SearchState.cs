using UnityEngine;

public class SearchState : IState
{
    private EnemyAI enemy;
    private Vector3 searchPosition;

    private float timer = 0f;

    public SearchState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.speed = 1.5f;
        enemy.SetColor(enemy.hearingColor);
        enemy.agent.SetDestination(searchPosition);
        timer = 0f;
    }


    public void Update()
    {
        timer += Time.deltaTime;

        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(enemy.chaseState);
        }
        else if (timer > 5f)
        {
            enemy.ChangeState(enemy.patrolState);
        }
    }

    public void SetSearchTarget(Vector3 pos)
    {
        // Set the search target position
        searchPosition = pos;
    }


    public void Exit() { }
}
