using UnityEngine;

public class VehicleSpriteHandler : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer _chassisSprite;
    [field: SerializeField] private SpriteRenderer _frontWheelsSprite;
    [field: SerializeField] private SpriteRenderer _backWheelsSprite;
    [field: SerializeField] private GameObject _exhaust;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void AssignSprite(VehicleVisualSettings visualSettings, Sprite playerOneVehicleChassisSprite)
    {
        if(_collider != null)
        {
            _collider.offset = visualSettings.ColliderDimensions / 2;
            _collider.transform.localScale = visualSettings.ColliderDimensions;
        }

        if (_chassisSprite != null)
        {
            _chassisSprite.sprite = playerOneVehicleChassisSprite;
            _chassisSprite.size = visualSettings.VehicleChassisDimensions;
        }

        if (_frontWheelsSprite != null)
        {
            _frontWheelsSprite.sprite = visualSettings.VehicleWheelsSprite;
            _frontWheelsSprite.transform.localPosition = visualSettings.VehicleFrontWheelsPosition;
        }

        if (_backWheelsSprite != null)
        {
            _backWheelsSprite.sprite = visualSettings.VehicleWheelsSprite;
            _backWheelsSprite.transform.localPosition = visualSettings.VehicleBackWheelsPosition;
        }

        if (_exhaust != null)
        {
            _exhaust.transform.localPosition = visualSettings.VehicleExhaustPosition;
        }
    }
}