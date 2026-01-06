using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private PlayerInput Input;
    public float Throttle { get; set; }
    public float Steering { get; set; }
    public bool HandBrake { get; set; }

    private InputAction _throttleAction;
    private InputAction _steeringAction;
    private InputAction _handBrakeAction;
    private bool _active = false;
    private void Update()
    {
        if (!_active) return;
        Throttle = _throttleAction.ReadValue<float>();
        Steering = _steeringAction.ReadValue<float>();
        HandBrake = _handBrakeAction.ReadValue<bool>();
    }

    public void AssignInput(string actionMapName)
    {
        Input = GetComponent<PlayerInput>();
        Input.SwitchCurrentActionMap(actionMapName);

        _throttleAction = Input.actions["Vertical"];
        _steeringAction = Input.actions["Horizontal"];
        _handBrakeAction = Input.actions["HandBrake"];

        _active = true;
    }
}
