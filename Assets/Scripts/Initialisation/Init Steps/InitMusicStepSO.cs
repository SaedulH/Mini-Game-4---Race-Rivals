using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Step", menuName = "Levels/InitSteps/InitMusicStep")]
    public class InitMusicStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            await Task.CompletedTask;
        }
    }
}

