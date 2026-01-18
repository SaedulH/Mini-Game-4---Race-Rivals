using UnityEngine;

public class VehicleSpriteHandler : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AssignSprite(Sprite vehicleSprite)
    {
        if (_spriteRenderer != null)
        {
            Debug.Log($"SpriteRenderer component found on {gameObject.name}");
            _spriteRenderer.sprite = vehicleSprite;
        }
        else
        {
            Debug.LogWarning($"SpriteRenderer component not found on {gameObject.name}");
        }
    }
}