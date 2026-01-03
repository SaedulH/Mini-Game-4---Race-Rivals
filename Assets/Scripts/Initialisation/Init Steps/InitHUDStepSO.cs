using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init HUD Step", menuName = "Levels/InitSteps/InitHUDStep")]
    public class InitHUDStepSO : LevelInitStepSO
    {
        [field: SerializeField] public float GoldTime { get; private set; }
        [field: SerializeField] public float SilverTime { get; private set; }
        [field: SerializeField] public float BronzeTime { get; private set; }

        public override async Task Run(TrackContext context)
        {
            List<float> medalTimes = new()
            {
                GoldTime,
                SilverTime,
                BronzeTime
            };

            await GameManager.Instance.InitialiseHUD(context, medalTimes);
        }
    }
}

