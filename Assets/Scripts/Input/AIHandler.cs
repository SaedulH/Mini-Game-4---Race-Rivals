using System;
using UnityEngine;
using Utilities;

public class AIHandler : MonoBehaviour, IInputHandler
{
    public float Throttle { get; set; }
    public float Steering { get; set; }
    public bool HandBrake { get; set; }

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;
    [SerializeField] private float angleToTarget;
    [SerializeField] private float throttleAI;
    [SerializeField] private float steeringAI;

    [field: SerializeReference] private WaypointNode CurrentNode { get; set; }
    [field: SerializeField] private Difficulty Difficulty { get; set; }

    // Update is called once per frame
    void FixedUpdate()
    {

        //if (GameManager.Instance.isRaceStart)
        //{
        //    throttleAI = ApplyThrottleOrBrakes(steeringAI);
        //    steeringAI = TurnTowardsTarget();


        //    //ChasePlayer();
        //    FollowWaypoints();

        //    detectInput(1f, steeringAI);
        //}
    }

    void FollowWaypoints()
    {


        //if(currentNode != null) 
        //{ 
        //    targetPosition = currentNode.transform.position;

        //    float distanceToWaypoint = (targetPosition - transform.position).magnitude;

        //    if(distanceToWaypoint <= currentNode.minDistanceToReachWaypoint)
        //    {
        //        currentNode = currentNode.nextWaypointNode[UnityEngine.Random.Range(0, currentNode.nextWaypointNode.Length)];
        //    }
        //}
    }

    void ChasePlayer()
    {
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }
    }

    //WaypointNode FindClosestWaypoint()
    //{
    //    return allWaypointNodes
    //        .OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    //}

    float TurnTowardsTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = Mathf.Clamp(angleToTarget, -1, 1);
        if (angleToTarget > 30)
        {
            steerAmount = 1;
        }
        else if (angleToTarget < -30)
        {
            steerAmount = -1;
        }


        return steerAmount;

    }

    float ApplyThrottleOrBrakes(float steeringAI)
    {
        float angle = Mathf.Abs(angleToTarget);
        float drive = 0;
        if (angle >= 60)
        {
            drive = 0;
        }
        else if (angle >= 45 && angle < 60)
        {
            drive = 0.33f;
        }
        else if (angle >= 30 && angle < 45)
        {
            drive = 0.66f;
        }
        else if (angle < 30)
        {
            drive = 1f;
        }

        return drive;
    }

    void detectInput(float drive, float steering)
    {
        //Detect throttle
        if (drive != 0)
        {
            //movement.HandleThrottle(drive);

        }

        //Detect Steering
        //if (steering != 0 && rb.linearVelocity != Vector2.zero)
        //{

        //    //movement.HandleSteering(steering);
        //}
        //movement.AnimateTyres(steering);
        //.movement.DetectDrift();
    }

    public void SetNextWaypoint(WaypointNode currentWaypoint)
    {
        throw new NotImplementedException();
    }

    public void SetDifficulty(string difficulty)
    {
        if (Enum.TryParse(difficulty, out Difficulty parsedDifficulty))
        {
            Difficulty = parsedDifficulty;
        }
        else
        {
            Difficulty = Difficulty.Easy;
        }
    }
}
