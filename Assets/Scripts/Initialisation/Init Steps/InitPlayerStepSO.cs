using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Step", menuName = "Levels/InitSteps/InitPlayerStep")]
    public class InitPlayerStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            //PlayerContr controller = context.Player;

            //controller = Instantiate(controller, Vector3.up, Quaternion.identity);
            //controller.transform.SetParent(null);

            ////Set Player Instance
            //GameManager.Instance.Player = controller;
            //context.Player = controller;

            //await GameManager.Instance.PlayerSetup
            //    .ConfigureEntityCoroutine(controller, classData)
            //    .ToTask(GameManager.Instance);

            await Task.CompletedTask;
        }
    }
}

