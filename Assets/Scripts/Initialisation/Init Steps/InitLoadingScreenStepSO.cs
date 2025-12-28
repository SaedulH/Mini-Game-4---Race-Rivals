using System.Threading.Tasks;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Loading Screen Step", menuName = "Levels/InitSteps/InitLoadingScreenStep")]
    public class InitLoadingScreenStepSO : LevelInitStepSO
    {
        [SerializeField] private Sprite LevelImage;
        [SerializeField] private string LevelTitle;
        [SerializeField] private string LevelSummary;

        public override async Task Run(TrackContext context)
        {
            //await LoadingScreen.Instance.SetLevelInfo(LevelImage, LevelTitle, LevelSummary, context.TotalWeight);

            //await LoadingScreen.Instance.ShowLoadingScreen();

            await Task.CompletedTask;
        }
    }
}

