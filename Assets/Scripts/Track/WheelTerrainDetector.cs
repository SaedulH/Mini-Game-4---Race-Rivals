using UnityEngine;

public class WheelTerrainDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _offRoadLayer;

    public bool DetectOffRoadTerrain()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector3.forward, 5f, _offRoadLayer);
        if (raycast.collider != null)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        // visualize ray
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
    }
}
