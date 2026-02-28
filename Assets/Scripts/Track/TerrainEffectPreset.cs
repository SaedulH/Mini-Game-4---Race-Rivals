using UnityEngine;

[CreateAssetMenu(fileName = "EffectPreset", menuName = "Effects/Terrain Effect")]
public class TerrainEffectPreset : ScriptableObject
{
    public Sprite[] effectSpriteSheet;
    public Color startColor;
    public float emissionRate;
    public float startSize;
    public float lifetime;
}