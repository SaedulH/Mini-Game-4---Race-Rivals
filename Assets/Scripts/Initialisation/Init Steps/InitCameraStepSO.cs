using System;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Camera Step", menuName = "Levels/InitSteps/InitCameraStep")]
    public class InitCameraStepSO : LevelInitStepSO
    {
        public override async Task Run(TrackContext context)
        {
            string cameraMode = PlayerPrefs.GetString("Camera");
            await CameraZoom.Instance.SetupCameraMode(cameraMode);

            string screenShakeSetting = PlayerPrefs.GetString("ScreenShake");
            await CameraShake.Instance.SetupScreenShake(screenShakeSetting);
        }
    }
}

