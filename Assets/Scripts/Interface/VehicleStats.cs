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

    [field: Header("Speed")]
    [field: SerializeField, Range(min: 0, max: 100)] public float AccelAmount { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float TopSpeed { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float TopReverseSpeed { get; set; }

    [field: Header("Braking")]
    [field: SerializeField, Range(min: 0, max: 100)] public float BrakePower { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float HandBrakePower { get; set; }

    [field: Header("Handling")]
    [field: SerializeField, Range(min: 0, max: 100)] public float SteerStrength { get; set; }
    [field: SerializeField, Range(min: 0, max: 180)] public float MaxTurnAngle { get; set; }
    [field: SerializeField, Range(min: 0, max: 180)] public float TurnDetectionAngle { get; set; }
    [field: SerializeField, Range(min: 0, max: 50)] public float NormalGrip { get; set; }
    [field: SerializeField, Range(min: 0, max: 50)] public float DriftGrip { get; set; }
}
