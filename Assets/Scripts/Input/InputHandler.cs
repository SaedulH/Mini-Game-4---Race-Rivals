using UnityEngine;
using UnityEngine.InputSystem;

<<<<<<< HEAD
public class InputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private PlayerInput Input;
    public float Throttle { get; set; }
    public float Steering { get; set; }
    public bool HandBrake { get; set; }

    private InputAction throttleAction;
    private InputAction steeringAction;
    private InputAction handBrakeAction;

    private void Update()
    {
        Throttle = throttleAction.ReadValue<float>();
        Steering = steeringAction.ReadValue<float>();
        HandBrake = handBrakeAction.ReadValue<bool>();
    }

    public void AssignInput(string actionMapName)
    {
        Input = GetComponent<PlayerInput>();
        Input.SwitchCurrentActionMap(actionMapName);

        throttleAction = Input.actions["Vertical"];
        steeringAction = Input.actions["Horizontal"];
        handBrakeAction = Input.actions["HandBrake"];
    }
=======
public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputActionMap InputActions;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float drive;
    [SerializeField] private float steering;
    private Movement movement;
    private string throttleKeys { get; set; }
    private string steeringKeys { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.Instance.isRaceStart)
        //{
        //    detectInput();
        //}
    }

    //Sets the input controls for each player
    public void AssignInput(InputActionMap inputActions)
    {
        InputActions = inputActions;
    }

    //Detect control input for movement
    void detectInput()
    {
        drive = Input.GetAxis(throttleKeys);
        steering = Input.GetAxis(steeringKeys);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.HandBrakes();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            movement.handBrakesActive = false;
        }
        //Detect throttle
        if (drive != 0)
        {
            movement.Throttle(drive);
        }

        //Detect Steering
        if (steering != 0 && rb.linearVelocity != Vector2.zero)
        {

            movement.Turn(drive, -steering);
        }
        movement.AnimateTyres(steering);
        movement.DetectDrift();
    }

>>>>>>> origin/master
}
