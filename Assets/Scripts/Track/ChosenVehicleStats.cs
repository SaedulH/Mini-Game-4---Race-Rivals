using System;

[Serializable]
public struct ChosenVehicleStats
{
    public float AccelAmount;
    public float TopSpeed;
    public float TopReverseSpeed;

    public float BrakePower;
    public float HandBrakePower;

    public float SteerStrength;
    public float MaxTurnAngle;
    public float TurnDetectionAngle;
    public float DriftGrip;
    public float NormalGrip;

    public ChosenVehicleStats(VehicleStats stats)
    {
        AccelAmount = stats.AccelAmount;
        TopSpeed = stats.TopSpeed;
        TopReverseSpeed = stats.TopReverseSpeed;

        BrakePower = stats.BrakePower;
        HandBrakePower = stats.HandBrakePower;

        SteerStrength = stats.SteerStrength;
        MaxTurnAngle = stats.MaxTurnAngle;
        TurnDetectionAngle = stats.TurnDetectionAngle;
        DriftGrip = stats.DriftGrip;
        NormalGrip = stats.NormalGrip;
    }
}
