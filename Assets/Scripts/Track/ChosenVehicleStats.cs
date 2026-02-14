using System;

[Serializable]
public struct ChosenVehicleStats
{
    public float AccelAmount;
    public float TopSpeed;
    public float TopReverseSpeed;

    public float BrakePower;
    public float HandBrakePower;
    public float HandBrakeTurnBoost;
    public float MinSpeedForBrakeEffect;

    public float SteerStrength;
    public float MaxAnglularVelocity;
    public float MaxTurnSpeedLossPercentage;
    public float MaxSteerStrengthLossPercentage;
    public float MinSpeedForSteering;

    public float NormalGrip;
    public float DriftGrip;
    public float MinSpeedToStartDrift;
    public float MinSpeedToMaintainDrift;
    public float MinAngularVelocityToStartDrift;
    public float MinAngularVelocityToMaintainDrift;

    public float MinSpeedForDriftEffect;
    public float MinAngularVelocityForDriftEffect;

    public ChosenVehicleStats(VehicleStats stats)
    {
        AccelAmount = stats.AccelAmount;
        TopSpeed = stats.TopSpeed;
        TopReverseSpeed = stats.TopReverseSpeed;

        BrakePower = stats.BrakePower;
        HandBrakePower = stats.HandBrakePower;
        HandBrakeTurnBoost = stats.HandBrakeTurnBoost;
        MinSpeedForBrakeEffect = stats.MinSpeedForBrakeEffect;

        SteerStrength = stats.SteerStrength;
        MaxAnglularVelocity = stats.MaxAnglularVelocity;
        MaxTurnSpeedLossPercentage = stats.MaxTurnSpeedLossPercentage;
        MaxSteerStrengthLossPercentage = stats.MaxSteerStrengthLossPercentage;
        MinSpeedForSteering = stats.MinSpeedForSteering;

        NormalGrip = stats.NormalGrip;
        DriftGrip = stats.DriftGrip;
        MinSpeedToStartDrift = stats.MinSpeedToStartDrift;
        MinSpeedToMaintainDrift = stats.MinSpeedToMaintainDrift;
        MinAngularVelocityToStartDrift = stats.MinAngularVelocityToStartDrift;
        MinAngularVelocityToMaintainDrift = stats.MinAngularVelocityToMaintainDrift;

        MinSpeedForDriftEffect = stats.MinSpeedForDriftEffect;
        MinAngularVelocityForDriftEffect = stats.MinAngularVelocityForDriftEffect;
    }
}
