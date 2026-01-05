using UnityEngine;

[CreateAssetMenu(fileName = "VehicleSelector", menuName = "VehicleSelector")]
public class VehicleSelector : ScriptableObject
{
    [field: SerializeField] public Vehicle[] AvailableVehicles { get; set; }
}
