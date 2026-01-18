using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleStats", menuName = "VehicleStats")]
public class VehicleStats : ScriptableObject
{
    [field: Header("Header Stats")]
    [field: SerializeField, Range(min: 0, max: 10)] public int Speed { get; set; }
    [field: SerializeField, Range(min: 0, max: 10)] public int Acceleration { get; set; }
    [field: SerializeField, Range(min: 0, max: 10)] public int Handling { get; set; }
    [field: SerializeField, Range(min: 0, max: 10)] public int Braking { get; set; }

    [field: Header("Stats")]
    [field: SerializeField, Range(min: 0, max: 100)] public float ThrottlePower { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float BrakePower { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float ReversePower { get; set; }

    [field: SerializeField, Range(min: 0, max: 100)] public float TopSpeed { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float TopReverseSpeed { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float SteerStrength { get; set; }

    [field: SerializeField, Range(min: 0, max: 1)] public float BrakeDamping { get; set; }
    [field: SerializeField, Range(min: 0, max: 1)] public float DriftDamper { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float Multiplier { get; set; }
    [field: SerializeField, Range(min: 0, max: 180)] public float MaxTurn { get; set; }
    [field: SerializeField, Range(min: 0, max: 180)] public float TurnDetection { get; set; }
    [field: SerializeField, Range(min: 0, max: 50)] public float ReductionAmount { get; set; }
}
