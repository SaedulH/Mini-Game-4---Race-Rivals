using UnityEngine;

[CreateAssetMenu(fileName = "VehicleStats", menuName = "VehicleStats")]
public class VehicleStats : ScriptableObject
{
    [field: Header("Header Stats")]
    [field: SerializeField, Range(min: 0, max: 10)] public int Speed { get; set; }
    [field: SerializeField, Range(min: 0, max: 10)] public int Acceleration { get; set; }
    [field: SerializeField, Range(min: 0, max: 10)] public int Handling { get; set; }
    [field: SerializeField, Range(min: 0, max: 10)] public int Braking { get; set; }

    [field: Header("Technical Stats")]

    [field: Header("Speed")]
    [field: SerializeField, Range(min: 0, max: 100)] public float AccelAmount { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float TopSpeed { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float TopReverseSpeed { get; set; }

    [field: Header("Braking")]
    [field: SerializeField, Range(min: 0, max: 100)] public float BrakePower { get; set; }
    [field: SerializeField, Range(min: 0, max: 1)] public float HandBrakePower { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float HandBrakeTurnBoost { get; set; }

    [field: Header("Handling")]
    [field: SerializeField, Range(min: 0, max: 200)] public float SteerStrength { get; set; }
    [field: SerializeField, Range(min: 0, max: 240)] public float MaxAnglularVelocity { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float MinSpeedForSteering { get; set; }
    [field: SerializeField, Range(min: 0, max: 1)] public float MaxTurnSpeedLossPercentage { get; set; }
    [field: SerializeField, Range(min: 0, max: 1)] public float MaxSteerStrengthLossPercentage { get; set; }
    [field: SerializeField, Range(min: 0, max: 50)] public float NormalGrip { get; set; }
    [field: SerializeField, Range(min: 0, max: 50)] public float DriftGrip { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float MinSpeedToStartDrift { get; set; }
    [field: SerializeField, Range(min: 0, max: 100)] public float MinSpeedToMaintainDrift { get; set; }
    [field: SerializeField, Range(min: 0, max: 240)] public float MinAngularVelocityToStartDrift { get; set; }
    [field: SerializeField, Range(min: 0, max: 240)] public float MinAngularVelocityToMaintainDrift { get; set; }

    [field: Header("RigidBody")]
    [field: SerializeField, Range(min: 0, max: 5)] public float Mass { get; set; } = 1f;
    [field: SerializeField, Range(min: 0, max: 5)] public float LinearDamping { get; set; } = 1.5f;
    [field: SerializeField, Range(min: 0, max: 5)] public float AngularDamping { get; set; } = 2f;
    [field: SerializeField] public Vector2 CentreOfMass { get; set; } = new Vector2(0.0f, 0.5f);

    [field: Header("Effects")]

    [field: Header("Exhaust Effects")]
    [field: SerializeField, Range(min: 0, max: 1)] public float LowExhaustRange { get; set; } = 0.4f;
    [field: SerializeField, Range(min: 0, max: 1)] public float HighExhaustRange { get; set; } = 0.6f;
    [field: SerializeField, Range(min: 0, max: 20)] public float LowExhaustRate { get; set; } = 5f;
    [field: SerializeField, Range(min: 0, max: 20)] public float HighExhaustRate { get; set; } = 10f;

    [field: Header("Drift Effects")]
    [field: SerializeField, Range(min: 0, max: 100)] public float MinSpeedForBrakeEffect { get; set; } = 10f;
    [field: SerializeField, Range(min: 0, max: 100)] public float MinSpeedForDriftEffect { get; set; } = 20f;
    [field: SerializeField, Range(min: 0, max: 360)] public float MinAngularVelocityForDriftEffect { get; set; } = 120f;
    [field: SerializeField, Range(min: 0, max: 1)] public float LowDriftRange { get; set; } = 0.2f;
    [field: SerializeField, Range(min: 0, max: 1)] public float HighDriftRange { get; set; } = 0.6f;
    [field: SerializeField, Range(min: 0, max: 20)] public float LowDriftRate { get; set; } = 5f;
    [field: SerializeField, Range(min: 0, max: 20)] public float HighDriftRate { get; set; } = 10f;
}
