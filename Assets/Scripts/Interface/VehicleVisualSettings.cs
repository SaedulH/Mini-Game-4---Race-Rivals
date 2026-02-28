using UnityEngine;

[CreateAssetMenu(fileName = "VehicleVisualSettings", menuName = "VehicleVisualSettings")]
public class VehicleVisualSettings : ScriptableObject
{
    [field: SerializeField] public Vector2 ColliderDimensions { get; set; }
    [field: SerializeField] public Vector2 VehicleChassisDimensions { get; set; }
    [field: SerializeField] public Sprite VehicleShadowSprite { get; set; }
    [field: SerializeField] public Sprite VehicleWheelsSprite { get; set; }
    [field: SerializeField] public Vector2 VehicleFrontWheelsPosition { get; set; }
    [field: SerializeField] public float VehicleFrontWheelsScale { get; set; }
    [field: SerializeField] public Vector2 VehicleBackWheelsPosition { get; set; }
    [field: SerializeField] public Vector2 VehicleExhaustPosition { get; set; }
}