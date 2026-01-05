using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Step", menuName = "Levels/InitSteps/InitObjectivesStep")]
    public class InitObjectivesStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            await Task.CompletedTask;
        }
    }
}

