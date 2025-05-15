using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PatrolManager manager;

    [SerializeField] private List<EnemyAI> enemies;
    
    void Start()
    {
        if(!manager) 
            Debug.LogError("PatrolManager reference not set in GameManager.");
        
        if(enemies == null || enemies.Count == 0)
            Debug.LogError("No enemies assigned in GameManager.");
        
        manager.AssignWaypointsToNPCs(enemies);
    }

}
