using UnityEngine;
using Utilities;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] public float throttlePower;
    [SerializeField] public float brakePower;
    [SerializeField] private float reversePower;

    [SerializeField] private float topSpeed;
    [SerializeField] private float topReverseSpeed;
    [SerializeField] private float steerStrength;

    [SerializeField] private bool isReversing;
    [SerializeField] private float brakeDamping;
    [SerializeField] private float driftDamper;
    [SerializeField] private float multiplier;
    [SerializeField] private float maxTurn;
    [SerializeField] private float turnDetection;
    [SerializeField] private float reductionAmount;
    [SerializeField] private float speedDamper;
    [SerializeField] private float maxSpeedDamper;

    private Animator steeringAnim;
    private bool _isHandBrakesActive = false;
    private float[] turnDampingFactor = new float[2];

    private Vector2 forward = new Vector2(0.0f, 0.75f);

    [SerializeField] private IInputHandler inputHandler;
    private bool _isActive = false;
    private Vector3 _pausedVelocity;
    private float _pausedAngularVelocity;

    void Start()
    {
        inputHandler = GetComponent<IInputHandler>();
        steeringAnim = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.centerOfMass = new Vector2(0f, 0.75f);
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
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;
            _rigidBody.linearVelocity = _pausedVelocity;
            _rigidBody.angularVelocity = _pausedAngularVelocity;
        }
        else
        {
            _isActive = false;
            _rigidBody.bodyType = RigidbodyType2D.Static;
            _pausedVelocity = _rigidBody.linearVelocity;
            _pausedAngularVelocity = _rigidBody.angularVelocity;
        }
    }

    private void FixedUpdate()
    {
        if (inputHandler != null && _isActive)
        {
            HandBrakes(inputHandler.HandBrake);
            Throttle(inputHandler.Throttle);
            Turn(inputHandler.Throttle, -inputHandler.Steering);
            AnimateTyres(inputHandler.Steering);
            DetectDrift();
        }
    }

    //Applies forward force
    public void Throttle(float drive)
    {
        if (!_isHandBrakesActive)
        {
            if (drive > 0)
            {
                speedDamper = SpeedReduction()[0];
                maxSpeedDamper = topSpeed + SpeedReduction()[1];
                if (_rigidBody.linearVelocity.magnitude < maxSpeedDamper)
                {
                    _rigidBody.AddForce(throttlePower * drive * speedDamper * transform.up);

                }
                else
                {
                    _rigidBody.linearVelocity = _rigidBody.linearVelocity.normalized * maxSpeedDamper;
                }
            }
            else if (drive < 0)
            {
                ApplyBrakes(drive);
            }
        }
    }

    //Slows to stop / Reverse
    public void ApplyBrakes(float drive)
    {
        isReversing = Vector2.Dot(_rigidBody.linearVelocity, transform.up) < 0f;

        if (isReversing)
        {
            Reverse();
        }
        else
        {
            _rigidBody.AddForce(brakePower * drive * brakeDamping * transform.up);
        }
    }

    //Quick Stop
    public void HandBrakes(bool handBrakeInput)
    {
        if (!handBrakeInput && _isHandBrakesActive)
        {
            _isHandBrakesActive = false;
            return;
        }

        if (handBrakeInput && !_isHandBrakesActive)
        {
            _isHandBrakesActive = true;
        }

        if (Vector2.Dot(_rigidBody.linearVelocity, transform.up) < 0f)
            _rigidBody.AddForce(-brakePower * multiplier * Time.deltaTime * transform.up);
    }

    //Reverse
    public void Reverse()
    {
        if (_rigidBody.linearVelocity.magnitude < topReverseSpeed)
        {
            _rigidBody.AddForce(-reversePower * transform.up);
        }
        else
        {
            _rigidBody.linearVelocity = _rigidBody.linearVelocity.normalized * topReverseSpeed;
        }
    }

    //Steering
    public void Turn(float throttle, float steering)
    {
        if (_rigidBody.angularVelocity <= maxTurn && _rigidBody.angularVelocity >= -maxTurn && _rigidBody.linearVelocity.magnitude > 5)
        {
            if (throttle > 0)
            {
                _rigidBody.AddTorque(steering * steerStrength);
            }
            else if (throttle < 0 && isReversing)
            {
                _rigidBody.AddTorque(steering * -steerStrength);
            }
        }
    }

    //Returns dampened top speed when steering
    public float[] SpeedReduction()
    {
        float reduction = 1;
        float damper = 0;
        if (_rigidBody.angularVelocity > turnDetection)
        {
            damper = -(_rigidBody.angularVelocity / (reductionAmount));
            reduction = (1 - damper) / reductionAmount;
        }
        else if (_rigidBody.angularVelocity < -turnDetection)
        {
            damper = (_rigidBody.angularVelocity / reductionAmount);
            reduction = (1 - damper) / reductionAmount;
        }
        turnDampingFactor[0] = reduction;
        turnDampingFactor[1] = damper / 3;
        return turnDampingFactor;
    }

    //animates the turning of tyres
    public void AnimateTyres(float steering)
    {
        if (steering != 0)
        {
            steeringAnim.SetFloat("Steering", steering);
        }
    }

    //Counterforce for "drift", reduces the slideness of cars
    public void DetectDrift()
    {
        float steeringRightAngle;
        if (_rigidBody.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;

        float driftForce = Vector2.Dot(_rigidBody.linearVelocity, _rigidBody.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)_rigidBody.position, (Vector3)_rigidBody.GetRelativePoint(relativeForce), Color.red);

        _rigidBody.AddForce(_rigidBody.GetRelativeVector(relativeForce * driftDamper));
    }

    public void AssignVehicleStats(VehicleStats stats)
    {
        throttlePower = stats.ThrottlePower;
        brakePower = stats.BrakePower;
        reversePower = stats.ReversePower;
        topSpeed = stats.TopSpeed;
        topReverseSpeed = stats.TopReverseSpeed;
        steerStrength = stats.SteerStrength;
        brakeDamping = stats.BrakeDamping;
        driftDamper = stats.DriftDamper;
        multiplier = stats.Multiplier;
        maxTurn = stats.MaxTurn;
        turnDetection = stats.TurnDetection;
        reductionAmount = stats.ReductionAmount;

        isReversing = false;
        speedDamper = 0f;
        maxSpeedDamper = 0f;
    }
}
