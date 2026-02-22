using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    [field: SerializeField] public WheelTerrainDetector[] WheelDetectors { get; set; }
    [field: SerializeField] public float OffRoadFactor { get; private set; }

    private void FixedUpdate()
    {
        SetOffRoadFactor();
    }

    public void SetOffRoadFactor()
    {
        float offRoadWheels = 0;
        foreach (var wheel in WheelDetectors)
        {
            if (wheel.DetectOffRoadTerrain())
            {
                offRoadWheels++;
            }
        }
        OffRoadFactor = offRoadWheels * 0.25f;
    }
}
