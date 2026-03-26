using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{

    [field: SerializeReference] public List<SpriteRenderer> SpriteRenderers { get; private set; } = new();
    [field: SerializeReference] public List<Collider2D> UnderPassColliders { get; private set; } = new();
    [field: SerializeReference] public List<Collider2D> OverPassColliders { get; private set; } = new();
    private bool isOnOverpass = false;

    private CapsuleCollider2D vehicleCollider;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            SpriteRenderers.Add(spriteRenderer);
        }

        foreach (GameObject underPassCollider in GameObject.FindGameObjectsWithTag("UnderpassCollider"))
        {
            UnderPassColliders.Add(underPassCollider.GetComponent<Collider2D>());
        }

        foreach (GameObject overPassCollider in GameObject.FindGameObjectsWithTag("OverpassCollider"))
        {
            OverPassColliders.Add(overPassCollider.GetComponent<Collider2D>());
        }

        vehicleCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }


    private void Start()
    {
        ChangeLayer();
    }

    void SetCollisionWithOverpass()
    {

        foreach (Collider2D collider in OverPassColliders)
        {
            Physics2D.IgnoreCollision(vehicleCollider, collider, !isOnOverpass);
        }

        foreach (Collider2D collider in UnderPassColliders)
        {
            if (isOnOverpass)
            {
                Physics2D.IgnoreCollision(vehicleCollider, collider, true);
            }
            else
            {
                Physics2D.IgnoreCollision(vehicleCollider, collider, false);
            }
        }
    }

    void ChangeLayer()
    {
        if (isOnOverpass)
        {
            SetSortingLayers("OverPass");
        }
        else
        {
            SetSortingLayers("Default");
        }

        SetCollisionWithOverpass();
    }

    void SetSortingLayers(string layerName)
    {
        foreach (SpriteRenderer renderer in SpriteRenderers)
        {
            renderer.sortingLayerName = layerName;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OverpassTrigger"))
        {
            Debug.Log("Overpass Triggered");
            isOnOverpass = true;
            ChangeLayer();
        }
        else if (collision.gameObject.CompareTag("UnderpassTrigger"))
        {
            Debug.Log("Underpass Triggered");
            isOnOverpass = false;
            ChangeLayer();
        }
    }
}
