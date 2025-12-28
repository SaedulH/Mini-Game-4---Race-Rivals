using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init HUD Step", menuName = "Levels/InitSteps/InitHUDStep")]
    public class InitHUDStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            //GameManager manager = context.Manager;

            ////await Task.CompletedTask;
            //await GameManager.Instance.PlayerSetup
            //    .InitializeHUD(controller.PlayerStats);

            await Task.CompletedTask;
        }
    }
}

