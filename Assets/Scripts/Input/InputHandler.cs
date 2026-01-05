using UnityEngine;
using UnityEngine.InputSystem;

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
}
