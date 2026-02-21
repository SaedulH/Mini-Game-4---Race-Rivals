using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    [field: SerializeField] public WheelTerrainDetector[] WheelDetectors { get; set; }

    public float GetOffRoadWheelCount()
    {
        float offRoadWheels = 0;
        foreach (var wheel in WheelDetectors)
        {
            if (wheel.DetectOffRoadTerrain())
            {
                offRoadWheels++;
            }
        }
        return offRoadWheels;
    }
}
