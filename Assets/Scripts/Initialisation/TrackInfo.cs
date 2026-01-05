using System.Collections.Generic;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "TrackInfo", menuName = "Track/TrackInfo")]
    public class TrackInfo : ScriptableObject
    {
        [field: SerializeField] public string TrackName { get; private set; }
        [field: SerializeField] public int TrackIndex { get; private set; }
        [field: SerializeField] public Sprite TrackImage { get; private set; }
        [field: SerializeField] public List<LevelInitStepSO> StepOrder { get; private set; }
    }
}

