using UnityEngine;

public class WheelTerrainDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _offRoadLayer;

    public bool DetectOffRoadTerrain()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, -transform.up, 1f, _offRoadLayer);
        if (raycast.collider != null)
        {
            return true;
        }

        return false;
    }
}
