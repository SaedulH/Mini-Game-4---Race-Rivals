using System;
using UnityEngine;
using Utilities;

public class TerrainDetector : MonoBehaviour
{
    [field: SerializeField] public WheelTerrainDetector[] WheelDetectors { get; set; }
    [field: SerializeField] public float TotalOffRoadFactor { get; private set; }

    private void FixedUpdate()
    {
        SetOffRoadFactor();
    }

    public void SetOffRoadFactor()
    {
        float offRoadFactor = 0;
        foreach (WheelTerrainDetector wheel in WheelDetectors)
        {
            offRoadFactor += GetOffRoadFactor(wheel.DetectOffRoadTerrain());
        }
        TotalOffRoadFactor = offRoadFactor * 0.25f;
    }

    public float GetOffRoadFactor(TerrainType terrainType)
    {
        switch (terrainType)
        {
            case TerrainType.Grass:
                return Constants.GRASS_TERRAIN_FACTOR;
            case TerrainType.Dirt:
                return Constants.DIRT_TERRAIN_FACTOR;
            case TerrainType.Gravel:
                return Constants.GRAVEL_TERRAIN_FACTOR;
            case TerrainType.Road:
                return Constants.ROAD_TERRAIN_FACTOR;
            default:
                return Constants.ROAD_TERRAIN_FACTOR;
        }
    }
}
