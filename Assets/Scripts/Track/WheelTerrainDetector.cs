using System;
using UnityEngine;
using Utilities;

public class WheelTerrainDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _offRoadLayer;
    [field: SerializeField] public TerrainType WheelTerrain { get; private set; }

    public TerrainType DetectOffRoadTerrain()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector3.forward, 5f, _offRoadLayer);
        if (raycast.collider != null)
        {
            WheelTerrain = GetTerrainType(raycast.collider);
        } else if (WheelTerrain != TerrainType.Road)
        {
            WheelTerrain = TerrainType.Road;
        }

        return WheelTerrain;
    }

    private TerrainType GetTerrainType(Collider2D collider)
    {
        if (collider.CompareTag("Grass"))
        {
            return TerrainType.Grass;
        }
        else if (collider.CompareTag("Dirt"))
        {
            return TerrainType.Dirt;
        }
        else if (collider.CompareTag("Gravel"))
        {
            return TerrainType.Gravel;
        }
        else
        {
            return TerrainType.Road;
        }
    }

    private void OnDrawGizmos()
    {
        // visualize ray
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
    }
}
