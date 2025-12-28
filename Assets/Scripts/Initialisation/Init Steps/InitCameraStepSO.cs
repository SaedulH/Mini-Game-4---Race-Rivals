using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Camera Step", menuName = "Levels/InitSteps/InitCameraStep")]
    public class InitCameraStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            //PlayerController Player = context.Player;

            //await CinemachineController.Instance.SetTrackingTarget(Player.transform);

            //await CameraZoom.Instance.ResetCameraZoom();

            //await CameraShake.Instance.ResetCameraShake();
            await Task.CompletedTask;
        }
    }
}

