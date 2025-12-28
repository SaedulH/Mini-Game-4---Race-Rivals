using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    public WaypointNode[] nextWaypointNode;
    public float minDistanceToReachWaypoint = 5;

    public float maxSpeed;
}
