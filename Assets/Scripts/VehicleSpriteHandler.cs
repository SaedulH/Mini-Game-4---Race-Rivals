using UnityEngine;

public class VehicleSpriteHandler : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer _chassisSprite;

    [field: SerializeField] private GameObject _frontWheelsParent;
    [field: SerializeField] private SpriteRenderer _frontRightWheelSprite;
    [field: SerializeField] private SpriteRenderer _frontLeftWheelSprite;
    [field: SerializeField] private GameObject _backWheelsParent;

    [field: SerializeField] private GameObject _exhaust;
    private CapsuleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
    }

    public void AssignSprite(VehicleVisualSettings visualSettings, Sprite playerOneVehicleChassisSprite)
    {
        if(_collider != null)
        {
            _collider.size = visualSettings.ColliderDimensions;
        }

        if (_chassisSprite != null)
        {
            _chassisSprite.sprite = playerOneVehicleChassisSprite;
            _chassisSprite.size = visualSettings.VehicleChassisDimensions;
        }

        if (_frontWheelsParent != null)
        {
            _frontWheelsParent.transform.localPosition = visualSettings.VehicleFrontWheelsPosition;
            if (_frontRightWheelSprite != null)
            {
                _frontRightWheelSprite.sprite = visualSettings.VehicleWheelsSprite;
                _frontRightWheelSprite.transform.localPosition = new Vector2(visualSettings.VehicleFrontWheelsDistanceFromCentre, 0f);
                _frontRightWheelSprite.transform.localScale = visualSettings.VehicleFrontWheelsScale * Vector3.one;
            }
            if (_frontLeftWheelSprite != null)
            {
                _frontLeftWheelSprite.sprite = visualSettings.VehicleWheelsSprite;
                _frontLeftWheelSprite.transform.localPosition = new Vector2(-visualSettings.VehicleFrontWheelsDistanceFromCentre, 0f);
                _frontLeftWheelSprite.transform.localScale = visualSettings.VehicleFrontWheelsScale * Vector3.one;
            }
        }

        if (_backWheelsParent != null)
        {
            _backWheelsParent.transform.localPosition = visualSettings.VehicleBackWheelsPosition;
        }

        if (_exhaust != null)
        {
            _exhaust.transform.localPosition = visualSettings.VehicleExhaustPosition;
        }
    }
}