using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Step", menuName = "Levels/InitSteps/InitLightingStep")]
    public class InitLightingStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            await Task.CompletedTask;
        }
    }
}

