using System;
using UnityEngine;
using UnityEngine.Splines;
using Utilities;

public class AIHandler : MonoBehaviour, IInputHandler
{
    public float Throttle { get; set; }
    public float Steering { get; set; }
    public bool HandBrake { get; set; }

    [field: SerializeReference] public SplineContainer WaypointSpline { get; private set; }
    [field: SerializeField] private float LookAhead { get; set; } = 0.02f;
    private float _splinePos;

    [Header("Driving")]
    [field: SerializeField] public float MaxSteeringAngle { get; private set; }
    [field: SerializeField] public float SteeringSmooth { get; private set; }

    [Header("Speed Control")]
    [field: SerializeField] public float MaxThrottle { get; private set; }
    [field: SerializeField] public float MinThrottle { get; private set; }
    [field: SerializeField] public float BrakeAngle { get; private set; }

    private bool _isActive = false;

    private float _currentThrottle = 0f;
    private float _currentSteering = 0f;
    private bool _currentHandbrake = false;

    // Update is called once per frame
    private void Update()
    {
        if (!_isActive || WaypointSpline == null)
        {
            Throttle = 0;
            Steering = 0;
            HandBrake = false;
            return;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        float speedFactor = Mathf.InverseLerp(MinThrottle, MaxThrottle, Throttle);
        LookAhead = Mathf.Lerp(
            Constants.AI_SPLINE_MIN_LOOK_AHEAD,
            Constants.AI_SPLINE_MAX_LOOK_AHEAD,
            speedFactor
        );

        float targetT = Mathf.Clamp01(_splinePos + LookAhead);
        Vector3 targetPosition = WaypointSpline.EvaluatePosition(targetT);

        Vector2 direction = (targetPosition - transform.position).normalized;

        float angle = Vector2.SignedAngle(transform.up, direction);

        float targetSteering = Mathf.Clamp(angle / MaxSteeringAngle, -1f, 1f);

        _currentSteering = Mathf.Lerp(
            _currentSteering,
            targetSteering,
            Time.fixedDeltaTime * SteeringSmooth
        );

        Steering = _currentSteering;

        Vector3 forwardNow = WaypointSpline.EvaluateTangent(_splinePos);
        Vector3 forwardAhead = WaypointSpline.EvaluateTangent(targetT);

        float curvature = Vector3.Angle(forwardNow, forwardAhead);

        Throttle = Mathf.Lerp(MaxThrottle, MinThrottle, curvature / 90f);

        //HandBrake = curvature > BrakeAngle;

        Debug.DrawLine(transform.position, targetPosition, Color.cyan);
    }

    public void SetWaypointSpline()
    {
        GameObject splineObject = GameObject.FindGameObjectWithTag("WaypointSpline");
        if (splineObject == null)
        {
            Debug.LogError("Error setting WaypointSpline for AI, splineObject not found");
        }
        WaypointSpline = splineObject.GetComponent<SplineContainer>();
    }

    public void SetDifficulty(string difficulty)
    {
        if (Enum.TryParse(difficulty, out Difficulty parsedDifficulty))
        {
            SetVariables(parsedDifficulty);
        }
        else
        {
            SetVariables(Difficulty.Easy);
        }

        _isActive = true;
    }

    private void SetVariables(Difficulty difficulty)
    {
        if (difficulty == Difficulty.Easy)
        {
            MaxSteeringAngle = Constants.AI_EASY_MAX_STEERING_ANGLE;
            SteeringSmooth = Constants.AI_EASY_STEERING_SMOOTH;
            MinThrottle = Constants.AI_EASY_MIN_THROTTLE;
            MaxThrottle = Constants.AI_EASY_MAX_THROTTLE;
            BrakeAngle = Constants.AI_EASY_BRAKE_ANGLE;
        }
        else
        {
            MaxSteeringAngle = Constants.AI_HARD_MAX_STEERING_ANGLE;
            SteeringSmooth = Constants.AI_HARD_STEERING_SMOOTH;
            MinThrottle = Constants.AI_HARD_MIN_THROTTLE;
            MaxThrottle = Constants.AI_HARD_MAX_THROTTLE;
            BrakeAngle = Constants.AI_HARD_BRAKE_ANGLE;
        }
    }
}
