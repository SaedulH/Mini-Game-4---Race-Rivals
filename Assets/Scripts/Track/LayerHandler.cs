using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{

    [SerializeReference] private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    [SerializeReference] private List<Collider2D> underPassColliders = new List<Collider2D>();
    [SerializeReference] private List<Collider2D> overPassColliders = new List<Collider2D>();
    private bool isOnOverpass = false;

    private Collider2D carCollider;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderers.Add(spriteRenderer);
        }

        foreach (GameObject overPassCollider in GameObject.FindGameObjectsWithTag("OverpassCollider"))
        {
            overPassColliders.Add(overPassCollider.GetComponent<Collider2D>());
        }
        foreach (GameObject underPassCollider in GameObject.FindGameObjectsWithTag("UnderpassCollider"))
        {
            underPassColliders.Add(underPassCollider.GetComponent<Collider2D>());
        }

        carCollider = gameObject.GetComponent<Collider2D>();
    }


    private void Start()
    {
        ChangeLayer();
    }

    void SetCollisionWithOverpass()
    {

        foreach (Collider2D collider in overPassColliders)
        {
            Physics2D.IgnoreCollision(carCollider, collider, !isOnOverpass);
        }

        foreach (Collider2D collider in underPassColliders)
        {
            if (isOnOverpass)
            {
                Physics2D.IgnoreCollision(carCollider, collider, true);
            }
            else
            {
                Physics2D.IgnoreCollision(carCollider, collider, false);
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
        foreach (SpriteRenderer renderer in spriteRenderers)
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
