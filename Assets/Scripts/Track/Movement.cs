using UnityEngine;
using Utilities;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private IInputHandler _input;
    [SerializeField] private Animator _anim;
    [SerializeField] private EffectsHandler _effects;
    [SerializeField] private ChosenVehicleStats _stats;
    [SerializeField] private VehicleStats _vehicleStats;
    [SerializeField] private TerrainDetector _terrainDetector;

    [SerializeField] private VehicleState _state = VehicleState.Idle;
    [SerializeField] private bool _isHandBrakesActive = false;
    [SerializeField] private bool _isDrifting = false;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private Vector3 _pausedVelocity;
    [SerializeField] private float _pausedAngularVelocity;
    [SerializeField] private float _animLerpSteering = 0;

    private bool _isInitialised = false;

    void Awake()
    {
        _input = GetComponent<IInputHandler>();
        _anim = GetComponent<Animator>();
        _effects = GetComponent<EffectsHandler>();
        _rb = GetComponent<Rigidbody2D>();
        _terrainDetector = GetComponent<TerrainDetector>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void AssignVehicleStats(VehicleStats stats)
    {
        _vehicleStats = stats;
        _stats = new ChosenVehicleStats(stats);
        _state = VehicleState.Idle;
        _rb.mass = stats.Mass;
        _rb.linearDamping = stats.LinearDamping;
        _rb.angularDamping = stats.AngularDamping;
        _rb.centerOfMass = stats.CentreOfMass;

        if (_effects == null)
        {
            _effects = GetComponent<EffectsHandler>();
        }
        _effects.SetupEffects(_vehicleStats);

        _isInitialised = true;
    }

    private void OnValidate()
    {
        if (_vehicleStats != null)
        {
            Debug.Log($"Reconfiguring Stats for {gameObject.name}");
            AssignVehicleStats(_vehicleStats);
        }
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
        _effects.PlayDriftEffects(!isPause);
    }

    private void Update()
    {
        if (!_isInitialised) return;

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
            float currentSpeed = Vector2.Dot(_rb.linearVelocity, transform.up);

            HandleSteering(_input.Steering, currentSpeed);

            ApplyHandBrake(_input.HandBrake, _input.Steering, currentSpeed);

            DetectDriftState(_input.Steering, currentSpeed);

            ApplyEngineForce(_input.Throttle);

            ApplyLateralGrip(currentSpeed);

            ApplySpeedLimit();

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
            0f,
            _stats.MaxAnglularVelocity,
            Mathf.Abs(_rb.angularVelocity)
        );

        // How much speed you lose at full turn
        maxSpeed *= Mathf.Lerp(1f, 1f - _stats.MaxTurnSpeedLossPercentage, turnFactor);

        // How much speed you lose offroad
        maxSpeed *= Mathf.Lerp(1f, 1f - _stats.MaxOffRoadSpeedLossPercentage, _terrainDetector.OffRoadFactor);

        _rb.linearVelocity = Vector2.ClampMagnitude(
            _rb.linearVelocity,
            maxSpeed
        );
    }

    private void ApplyLateralGrip(float currentSpeed)
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;

        float lateralVel = Vector2.Dot(_rb.linearVelocity, right);

        float gripStrength = Mathf.Lerp(
            _stats.DriftGrip,
            _stats.NormalGrip,
            _isDrifting ? 0f : 1f
        );

        //How much grip you lose offroad
        gripStrength *= Mathf.Lerp(1f, 1f - _stats.MaxOffRoadTractionLossPercentage, _terrainDetector.OffRoadFactor);

        // Smoothly remove sideways velocity
        lateralVel = Mathf.Lerp(
            lateralVel,
            0f,
            gripStrength * Time.fixedDeltaTime
        );

        _rb.linearVelocity = (currentSpeed * forward) + (lateralVel * right);
    }

    public void ApplyHandBrake(bool handBrakeInput, float steering, float currentSpeed)
    {
        if(_state == VehicleState.Idle)
        {
            return;
        }

        _isHandBrakesActive = handBrakeInput;
        if (handBrakeInput)
        {
            _rb.AddForce(-currentSpeed * _stats.HandBrakePower * transform.up);

            _rb.AddTorque(-steering * _stats.HandBrakeTurnBoost);
        }
    }

    public void HandleSteering(float steering, float currentSpeed)
    {
        if ((_state == VehicleState.Idle))
        {
            return;
        }

        float minSpeedFactor = Mathf.InverseLerp(
            0f,
            _stats.MinSpeedForSteering,
            Mathf.Abs(currentSpeed)
        );

        float maxSpeed = (_state == VehicleState.Reversing) ? _stats.TopReverseSpeed : _stats.TopSpeed;
        float speedFactor = Mathf.InverseLerp(
            0f,
            maxSpeed,
            Mathf.Abs(currentSpeed)
        );

        float highSpeedReduction = Mathf.Lerp(
            1f,
            1f - _stats.MaxSteerStrengthLossPercentage,
            speedFactor
        );

        float steerStrengthFactor = minSpeedFactor * highSpeedReduction;
        float steerForce = steering * _stats.SteerStrength * steerStrengthFactor;
        if (_state == VehicleState.Accelerating)
        {
            _rb.AddTorque(-steerForce);
        }
        else if (_state == VehicleState.Reversing)
        {
            _rb.AddTorque(steerForce);
        }

        _rb.angularVelocity = Mathf.Clamp(
            _rb.angularVelocity,
            -_stats.MaxAnglularVelocity,
            _stats.MaxAnglularVelocity
        );
    }

    public void DetectDriftState(float steering, float currentSpeed)
    {
        float speed = Mathf.Abs(currentSpeed);
        float angularVel = Mathf.Abs(_rb.angularVelocity);
        bool isBraking = _isHandBrakesActive || (_state == VehicleState.Braking);

        bool fastEnough = speed > _stats.MinSpeedToStartDrift;
        bool rotatingFast = angularVel > _stats.MinAngularVelocityToStartDrift;

        if (!_isDrifting)
        {
            if (fastEnough && rotatingFast && isBraking)
            {
                _isDrifting = true;
            }
        }
        else
        {
            bool tooSlow = speed < _stats.MinSpeedToMaintainDrift;
            bool stoppedRotating = angularVel < _stats.MinAngularVelocityToMaintainDrift;

            if (tooSlow || stoppedRotating)
            {
                _isDrifting = false;
            }
        }
    }

    #region Effects

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
            if (Mathf.Abs(currentSpeed) > _stats.MinSpeedForBrakeEffect)
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
        bool isBurningRubber = ((Mathf.Abs(currentSpeed) > _stats.MinSpeedForDriftEffect) &&
            (Mathf.Abs(_rb.angularVelocity) > _stats.MinAngularVelocityForDriftEffect));
        if (_isHandBrakesActive || _isDrifting || isBurningRubber)
        {
            float driftRate = Mathf.InverseLerp(
                0f,
                _stats.MaxAnglularVelocity,
                Mathf.Abs(_rb.angularVelocity)
            );

            _effects.SetDriftRate(driftRate);
            _effects.PlayDriftEffects(true);
        }
        else
        {
            _effects.PlayDriftEffects(false);
        }
    }

    public void AnimateSteering(float steering)
    {
        if (_animLerpSteering != steering)
        {
            _animLerpSteering = Mathf.Lerp(_animLerpSteering, steering, Time.deltaTime * Constants.STEERING_ANIM_LERP_SPEED);
            _anim.SetFloat("Steering", _animLerpSteering);
        }
    }

    #endregion
}
