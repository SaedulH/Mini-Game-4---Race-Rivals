using UnityEngine;

public class VehicleSpriteHandler : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer _chassisSprite;
    [field: SerializeField] private SpriteRenderer _shadowSprite;

    [field: SerializeField] private GameObject _frontWheelsParent;
    [field: SerializeField] private SpriteRenderer _frontRightWheelSprite;
    [field: SerializeField] private SpriteRenderer _frontLeftWheelSprite;
    [field: SerializeField] private GameObject _backWheelsParent;
    [field: SerializeField] private GameObject _backRightWheelParent;
    [field: SerializeField] private GameObject _backLeftWheelParent;

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

        if (_shadowSprite != null)
        {
            _shadowSprite.sprite = visualSettings.VehicleShadowSprite;
            _shadowSprite.size = visualSettings.VehicleChassisDimensions;
        }

        if (_frontWheelsParent != null)
        {
            _frontWheelsParent.transform.localPosition = new Vector2(0f, visualSettings.VehicleFrontWheelsPosition.y);
            if (_frontRightWheelSprite != null)
            {
                _frontRightWheelSprite.sprite = visualSettings.VehicleWheelsSprite;
                _frontRightWheelSprite.transform.localPosition = new Vector2(visualSettings.VehicleFrontWheelsPosition.x, 0f);
                _frontRightWheelSprite.transform.localScale = visualSettings.VehicleFrontWheelsScale * Vector3.one;
            }
            if (_frontLeftWheelSprite != null)
            {
                _frontLeftWheelSprite.sprite = visualSettings.VehicleWheelsSprite;
                _frontLeftWheelSprite.transform.localPosition = new Vector2(-visualSettings.VehicleFrontWheelsPosition.x, 0f);
                _frontLeftWheelSprite.transform.localScale = visualSettings.VehicleFrontWheelsScale * Vector3.one;
            }
        }

        if (_backWheelsParent != null)
        {
            _backWheelsParent.transform.localPosition = new Vector2(0f, visualSettings.VehicleBackWheelsPosition.y);
            if (_backRightWheelParent != null)
            {
                _backRightWheelParent.transform.localPosition = new Vector2(visualSettings.VehicleBackWheelsPosition.x, 0f);
            }
            if (_backLeftWheelParent != null)
            {
                _backLeftWheelParent.transform.localPosition = new Vector2(-visualSettings.VehicleBackWheelsPosition.x, 0f);
            }
        }

        if (_exhaust != null)
        {
            _exhaust.transform.localPosition = visualSettings.VehicleExhaustPosition;
        }
    }
}