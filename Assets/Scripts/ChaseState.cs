using UnityEngine;

public class ChaseState : IState
{
    private EnemyAI enemy;

    public ChaseState(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() {
        enemy.agent.speed = 3.5f;
        enemy.SetColor(enemy.alertColor);
    }

    public void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (distance > 10f)
        {
            enemy.ChangeState(enemy.searchState);
        }
        else
        {
            enemy.agent.SetDestination(enemy.player.position);
        }
    }

    public void Exit() { }
}
