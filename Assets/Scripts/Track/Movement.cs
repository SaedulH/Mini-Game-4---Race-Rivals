using System;
using UnityEngine;
using Utilities;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private ChosenVehicleStats _stats;
    [SerializeField] private VehicleStats _vehicleStats;

    [SerializeField] private VehicleState _state = VehicleState.Idle;

    [SerializeField] private Animator _anim;
    [SerializeField] private EffectsHandler _effects;
    [SerializeField] private bool _isHandBrakesActive = false;
    [SerializeField] private bool _isDrifting = false;

    [SerializeField] private Vector2 _forward = new Vector2(0.0f, 0.75f);
    [SerializeField] private IInputHandler _input;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private Vector3 _pausedVelocity;
    [SerializeField] private float _pausedAngularVelocity;

    void Start()
    {
        _input = GetComponent<IInputHandler>();
        _anim = GetComponentInChildren<Animator>();
        _effects = GetComponent<EffectsHandler>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.centerOfMass = _forward;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            _isActive = true;
            _rb.linearVelocity = _pausedVelocity;
            _rb.angularVelocity = _pausedAngularVelocity;
            _rb.bodyType = RigidbodyType2D.Dynamic;

            PauseEffects(false);
        }
        else
        {
            _isActive = false;
            _pausedVelocity = _rb.linearVelocity;
            _pausedAngularVelocity = _rb.angularVelocity;
            _rb.bodyType = RigidbodyType2D.Static;

            PauseEffects(true);
        }
    }

    private void PauseEffects(bool isPause)
    {
        _effects.PlayExhaustEffect(!isPause);
    }

    private void Update()
    {
        if (_input != null && _isActive)
        {
            float currentSpeed = Vector2.Dot(_rb.linearVelocity, transform.up);
            DetectVehicleState(_input, currentSpeed);
            PlayEffects(currentSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (_input != null && _isActive)
        {
            HandleSteering(_input.Steering);

            ApplyEngineForce(_input.Throttle);

            ApplyLateralGrip();

            ApplySpeedLimit();

            ApplyHandBrake(_input.HandBrake);
        }
    }

    private void DetectVehicleState(IInputHandler inputHandler, float currentSpeed)
    {
        switch (_state)
        {
            case VehicleState.Idle:
                if (inputHandler.Throttle > 0f)
                {
                    _state = VehicleState.Accelerating;
                }
                else if (inputHandler.Throttle < 0f)
                {
                    _state = VehicleState.Reversing;
                }
                break;
            case VehicleState.Accelerating:
                if (inputHandler.Throttle == 0f)
                {
                    _state = VehicleState.Decelerating;
                }
                else if (inputHandler.Throttle < 0f)
                {
                    _state = VehicleState.Braking;
                }
                break;
            case VehicleState.Decelerating:
                if (inputHandler.Throttle == 0f && Mathf.Abs(currentSpeed) < Constants.MAX_IDLE_SPEED)
                {
                    _state = VehicleState.Idle;
                }
                else if (inputHandler.Throttle > 0f)
                {
                    if (currentSpeed < 0f)
                    {
                        _state = VehicleState.Braking;
                    }
                    else
                    {
                        _state = VehicleState.Accelerating;

                    }
                }
                else if (inputHandler.Throttle < 0f)
                {
                    if (currentSpeed > 0f)
                    {
                        _state = VehicleState.Braking;
                    }
                    else
                    {
                        _state = VehicleState.Reversing;
                    }
                }
                break;
            case VehicleState.Braking:
                if (inputHandler.Throttle == 0f)
                {
                    _state = VehicleState.Decelerating;
                }
                else if (inputHandler.Throttle > 0f && currentSpeed > -Constants.MAX_IDLE_SPEED)
                {
                    _state = VehicleState.Accelerating;
                }
                else if (inputHandler.Throttle < 0f && currentSpeed < Constants.MAX_IDLE_SPEED)
                {
                    _state = VehicleState.Reversing;
                }
                break;
            case VehicleState.Reversing:
                if (inputHandler.Throttle == 0f)
                {
                    _state = VehicleState.Decelerating;
                }
                else if (inputHandler.Throttle > 0f)
                {
                    _state = VehicleState.Braking;
                }
                break;
        }
    }

    private void ApplyEngineForce(float throttle)
    {
        if (Mathf.Abs(throttle) < Constants.MIN_THROTTLE)
            return;

        float engineForce = throttle * _stats.AccelAmount;
        _rb.AddForce(engineForce * transform.up);
    }

    private void ApplySpeedLimit()
    {
        float maxSpeed = (_state == VehicleState.Reversing) ? _stats.TopReverseSpeed : _stats.TopSpeed;
        float turnFactor = Mathf.InverseLerp(
            _stats.TurnDetectionAngle,
            _stats.MaxTurnAngle,
            Mathf.Abs(_rb.angularVelocity)
        );

        // How much speed you lose at full turn
        maxSpeed *= Mathf.Lerp(1f, 1f - Constants.MAX_TURN_SPEED_LOSS_PERCENT, turnFactor);

        _rb.linearVelocity = Vector2.ClampMagnitude(
            _rb.linearVelocity,
            maxSpeed
        );
    }

    private void ApplyLateralGrip()
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;

        float forwardVel = Vector2.Dot(_rb.linearVelocity, forward);
        float lateralVel = Vector2.Dot(_rb.linearVelocity, right);

        float grip = Mathf.Lerp(
            _stats.DriftGrip,   // low grip when drifting
            _stats.NormalGrip,  // high grip normally
            _isHandBrakesActive ? 0f : 1f
        );

        lateralVel *= grip;

        _rb.linearVelocity = (forwardVel * forward) + (lateralVel * right);
    }

    public void ApplyHandBrake(bool handBrakeInput)
    {
        _isHandBrakesActive = handBrakeInput;
        if (handBrakeInput)
        {
            float currentSpeed = Vector2.Dot(_rb.linearVelocity, transform.up);
            int counterDirection = (currentSpeed > 0f) ? -1 : 1;
            _rb.AddForce(counterDirection * _stats.HandBrakePower * transform.up);
        }
    }

    public void HandleSteering(float steering)
    {
        if (_rb.linearVelocity.magnitude < Constants.MIN_SPEED_FOR_TURN)
            return;

        float speedFactor = Mathf.InverseLerp(
            0f,
            _stats.TopSpeed,
            _rb.linearVelocity.magnitude
        );

        float steerForce = steering * _stats.SteerStrength * speedFactor;
        if (_state == VehicleState.Accelerating)
        {
            _rb.AddTorque(-steerForce);
        }
        else if (_state == VehicleState.Reversing)
        {
            _rb.AddTorque(steerForce);
        }
    }

    private void PlayEffects(float currentSpeed)
    {
        EngineEffects(currentSpeed);
        TireEffects(currentSpeed);
    }

    private void EngineEffects(float currentSpeed)
    {
        switch (_state)
        {
            case VehicleState.Idle:
                IdleExhaustEffects(currentSpeed);
                break;
            case VehicleState.Accelerating:
                MovingExhaustEffects(Mathf.Abs(currentSpeed), _stats.TopSpeed);
                break;
            case VehicleState.Decelerating:
                //MovingExhaustEffects(Mathf.Abs(currentSpeed) / 4f, _stats.TopSpeed);
                break;
            case VehicleState.Reversing:
                MovingExhaustEffects(Mathf.Abs(currentSpeed), _stats.TopReverseSpeed);
                break;
        }
    }
    private void MovingExhaustEffects(float currentSpeed, float topSpeed)
    {
        float exhaustRate = Mathf.InverseLerp(
            0f,
            topSpeed,
            currentSpeed
        );

        //The further you are from 0, the more exhaust you have
        if (currentSpeed > Constants.MAX_IDLE_SPEED)
        {
            _effects.SetExhaustRate(exhaustRate);
        }
    }

    private void IdleExhaustEffects(float currentSpeed)
    {
        if (Mathf.Abs(currentSpeed) < Constants.MAX_IDLE_SPEED)
        {
            _effects.SetExhaustRate(Constants.IDLE_EXHAUST_RATE);
        }
    }

    private void TireEffects(float currentSpeed)
    {
        AnimateSteering(_input.Steering);
        BrakeEffect(currentSpeed);
        DriftEffect(currentSpeed);
    }

    private void BrakeEffect(float currentSpeed)
    {
        if (_state == VehicleState.Braking || _isHandBrakesActive)
        {
            if (Mathf.Abs(currentSpeed) > Constants.MIN_SPEED_FOR_BRAKE_EFFECTS)
            {
                _effects.PlayBrakeAudio(true);
            }
        }
        else
        {
            _effects.PlayBrakeAudio(false);
        }
    }

    private void DriftEffect(float currentSpeed)
    {
        if (_isHandBrakesActive || Mathf.Abs(_rb.angularVelocity) > Constants.MIN_ANGULAR_VEL_FOR_DRIFT_EFFECTS)
        {
            if (Mathf.Abs(currentSpeed) > Constants.MIN_SPEED_FOR_DRIFT_EFFECTS)
            {
                float driftRate = Mathf.InverseLerp(
                    0f,
                    90f,
                    Mathf.Abs(_rb.angularVelocity)
                );

                _effects.SetDriftRate(driftRate);
                _effects.PlayDriftEffects(true);
            }
        }
        else
        {
            _effects.PlayDriftEffects(false);
        }
    }

    public void AnimateSteering(float steering)
    {
        if(_anim.GetFloat("Steering") != steering)
        {
            _anim.SetFloat("Steering", steering);
        }
    }

    public void AssignVehicleStats(VehicleStats stats)
    {
        _vehicleStats = stats;
        _stats = new ChosenVehicleStats(stats);
        _state = VehicleState.Idle;
    }

    private void OnValidate()
    {
        if (_vehicleStats != null)
        {
            Debug.Log($"Reconfiguring Stats for {gameObject.name}");
            AssignVehicleStats(_vehicleStats);
        }
    }
}
