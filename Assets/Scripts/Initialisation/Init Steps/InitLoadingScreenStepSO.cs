using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Loading Screen Step", menuName = "Levels/InitSteps/InitLoadingScreenStep")]
    public class InitLoadingScreenStepSO : LevelInitStepSO
    {
        [SerializeField] private string TrackTitle;
        [SerializeField] private string TrackDescription;
        [SerializeField] private Sprite TrackImage;

        public override async Task Run(TrackContext context)
        {
            await LoadingScreen.Instance.SetLevelInfo(TrackTitle, TrackDescription, TrackImage, context);

            GameManager.Instance.EnterGameState(GameState.Loading);

            await Task.Yield();
        }
    }
}

