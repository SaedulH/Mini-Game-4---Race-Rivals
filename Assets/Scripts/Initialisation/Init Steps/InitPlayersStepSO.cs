using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Players Step", menuName = "Levels/InitSteps/InitPlayersStep")]
    public class InitPlayersStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            // Setup Player One
            await GameManager.Instance.ConfigurePlayer(1, context.VehicleOneIndex);

            // Setup Player Two
            if (context.PlayerCount == 2)
            {
                await GameManager.Instance.ConfigureAI(context.VehicleTwoIndex);
            }
            else
            {
                await GameManager.Instance.ConfigurePlayer(2, context.VehicleTwoIndex);
            }
        }
    }
}

