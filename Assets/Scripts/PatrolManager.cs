using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{
    public Transform[] allWaypoints;

    // List of waypoints assigned to each NPC
    // This will be a list of lists, where each inner list contains the waypoints for a specific NPC
    private List<List<Transform>> npcWaypoints = new List<List<Transform>>();

    public void AssignWaypointsToNPCs(List<EnemyAI> npcs)
    {
        int numNPCs = npcs.Count;
        int numPointsPerNPC = allWaypoints.Length / numNPCs;

        for (int i = 0; i < npcs.Count; i++)
        {
            List<Transform> assigned = new List<Transform>();
        
            for (int j = 0; j < numPointsPerNPC; j++)
            {
                int index = i * numPointsPerNPC + j;
                if (index < allWaypoints.Length)
                {
                    assigned.Add(allWaypoints[index]);
                }
            }

            npcWaypoints.Add(assigned);
            npcs[i].SetWaypoints(assigned.ToArray());
        }
    }
}
