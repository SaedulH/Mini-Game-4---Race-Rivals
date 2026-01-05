<<<<<<< HEAD
=======
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
>>>>>>> origin/master
using UnityEngine;

public class Movement : MonoBehaviour
{
<<<<<<< HEAD
=======

>>>>>>> origin/master
    [SerializeField] private Rigidbody2D rb;
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

<<<<<<< HEAD
=======

>>>>>>> origin/master
    private Animator steeringAnim;
    public bool handBrakesActive { get; set; }
    private float[] turnDampingFactor = new float[2];


    private Vector2 forward = new Vector2(0.0f, 0.75f);

    // Start is called before the first frame update
    void Start()
    {
        steeringAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = new Vector2(0f, 0.75f);
    }

    //Applies forward force
    public void Throttle(float drive)
    {
        if (!handBrakesActive)
        {
            if (drive > 0)
            {
                speedDamper = SpeedReduction()[0];
                maxSpeedDamper = topSpeed + SpeedReduction()[1];
                if (rb.linearVelocity.magnitude < maxSpeedDamper)
                {
                    rb.AddForce(throttlePower * drive * speedDamper * transform.up);

                }
                else
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeedDamper;
                }
<<<<<<< HEAD
            }
            else if (drive < 0)
=======
            }else if(drive < 0)
>>>>>>> origin/master
            {
                ApplyBrakes(drive);
            }
        }
    }

    //Slows to stop / Reverse
    public void ApplyBrakes(float drive)
    {
        isReversing = Vector2.Dot(rb.linearVelocity, transform.up) < 0f;
<<<<<<< HEAD

=======
        
>>>>>>> origin/master
        if (isReversing)
        {
            Reverse();
        }
        else
        {
<<<<<<< HEAD
            rb.AddForce(brakePower * drive * brakeDamping * transform.up);
=======
            rb.AddForce(brakePower * drive  * brakeDamping * transform.up);
>>>>>>> origin/master
        }
    }

    //Quick Stop
    public void HandBrakes()
    {
        handBrakesActive = true;
        if (Vector2.Dot(rb.linearVelocity, transform.up) < 0f)
<<<<<<< HEAD
            rb.AddForce(-brakePower * multiplier * Time.deltaTime * transform.up);
=======
        rb.AddForce(-brakePower * multiplier * Time.deltaTime * transform.up);
>>>>>>> origin/master
    }

    //Reverse
    public void Reverse()
    {

        if (rb.linearVelocity.magnitude < topReverseSpeed)
        {
            rb.AddForce(-reversePower * transform.up);
        }
        else
        {
            rb.linearVelocity = rb.linearVelocity.normalized * topReverseSpeed;
<<<<<<< HEAD
        }
=======
        }   
>>>>>>> origin/master
    }

    //Steering
    public void Turn(float drive, float direction)
    {
<<<<<<< HEAD
        if (rb.angularVelocity <= maxTurn && rb.angularVelocity >= -maxTurn && rb.linearVelocity.magnitude > 5)
        {
            if (drive > 0)
=======
        if(rb.angularVelocity <= maxTurn && rb.angularVelocity >= -maxTurn && rb.linearVelocity.magnitude > 5)
        {
            if(drive > 0)
>>>>>>> origin/master
            {
                rb.AddTorque(direction * steerStrength);
            }
            else if (drive < 0 && isReversing)
            {
                rb.AddTorque(direction * -steerStrength);
            }
<<<<<<< HEAD

        }

=======
            
        }
        
>>>>>>> origin/master
    }
    //Returns dampened top speed when steering
    public float[] SpeedReduction()
    {
        float reduction = 1;
        float damper = 0;
<<<<<<< HEAD
        if (rb.angularVelocity > turnDetection)
        {
            damper = -(rb.angularVelocity / (reductionAmount));
            reduction = (1 - damper) / reductionAmount;
        }
        else if (rb.angularVelocity < -turnDetection)
=======
        if(rb.angularVelocity > turnDetection)
        {
            damper = -(rb.angularVelocity / (reductionAmount));
            reduction = (1 - damper) / reductionAmount;
        }else if(rb.angularVelocity < -turnDetection)
>>>>>>> origin/master
        {
            damper = (rb.angularVelocity / reductionAmount);
            reduction = (1 - damper) / reductionAmount;
        }
        turnDampingFactor[0] = reduction;
<<<<<<< HEAD
        turnDampingFactor[1] = damper / 3;
=======
        turnDampingFactor[1] = damper/3;
>>>>>>> origin/master
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
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;

        float driftForce = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

        rb.AddForce(rb.GetRelativeVector(relativeForce * driftDamper));
    }
}
