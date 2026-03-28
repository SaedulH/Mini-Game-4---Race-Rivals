using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{

    [field: SerializeReference] public List<SpriteRenderer> SpriteRenderers { get; private set; } = new();
    [field: SerializeReference] public List<ParticleSystemRenderer> ParticleSystems { get; private set; } = new();
    [field: SerializeReference] public List<TrailRenderer> TrailRenderers { get; private set; } = new();

    void Awake()
    {
        foreach (SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            SpriteRenderers.Add(spriteRenderer);
        }

        foreach (ParticleSystemRenderer particleSystem in gameObject.GetComponentsInChildren<ParticleSystemRenderer>())
        {
            ParticleSystems.Add(particleSystem);
        }

        foreach (TrailRenderer trailRenderer in gameObject.GetComponentsInChildren<TrailRenderer>())
        {
            TrailRenderers.Add(trailRenderer);
        }
    }


    private void Start()
    {
        SetBridgeSortingLayer(true);
    }

    public void SetBridgeSortingLayer(bool isUnderPass)
    {
        string layerName = isUnderPass ? "Default" : "OverPass";
        foreach (SpriteRenderer sr in SpriteRenderers)
        {
            sr.sortingLayerName = layerName;
        }
        foreach (ParticleSystemRenderer ps in ParticleSystems)
        {
            ps.sortingLayerName = layerName;
        }
        foreach (TrailRenderer tr in TrailRenderers)
        {
            tr.sortingLayerName = layerName;
        }
    }
}
