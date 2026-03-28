using System;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [field: SerializeField] public AIHandler AIHandler { get; set; }
    [field: SerializeField] public WaypointNode[] Waypoints { get; set; }
    [field: SerializeField] public WaypointNode CurrentWaypoint { get; set; }

    void Start()
    {
        ConfigureWaypoints();
    }


    private void Update()
    {
        if(AIHandler == null || CurrentWaypoint == null)
        {
            return;
        }

        CheckWaypointReached();
    }

    private void ConfigureWaypoints()
    {
        if(Waypoints == null || Waypoints.Length == 0)
        {
            Debug.LogError("WaypointManager: No waypoints assigned!");
            return;
        }
        for (int i = 0; i < Waypoints.Length; i++)
        {
            WaypointNode currentWaypoint = Waypoints[i];
            WaypointNode nextWaypoint = Waypoints[(i + 1) % Waypoints.Length];
            currentWaypoint.NextWaypointNode = nextWaypoint;
            currentWaypoint.name = $"Waypoint_{i + 1}";
        }

        CurrentWaypoint = Waypoints[0];
        //AIHandler.SetNextWaypoint(CurrentWaypoint);
    }

    private void CheckWaypointReached()
    {
        if(Vector3.Distance(AIHandler.transform.position, CurrentWaypoint.transform.position) < CurrentWaypoint.MinDistanceToReachWaypoint)
        {
            CurrentWaypoint = CurrentWaypoint.NextWaypointNode;
            //AIHandler.SetNextWaypoint(CurrentWaypoint);
        }
    }
}
