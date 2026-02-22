using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    [field: SerializeField] public WaypointNode NextWaypointNode { get; set; }
    [field: SerializeField] public float MinDistanceToReachWaypoint { get; set; } = 5f;
    [field: SerializeField] public float RecommendedSpeed { get; set; }
}
