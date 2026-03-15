using System;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace CoreSystem
{

    /// <summary>
    /// Initialises the track, including loading screen management and other setup tasks.
    /// </summary>
    public class TrackInitialiser : NonPersistentSingleton<TrackInitialiser>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public async Task InitialiseTrack(TrackInfo trackInfo, TrackContext trackContext)
        {
            float completed = 0f;
            float totalWeight = trackContext.TotalWeight;

            Debug.Log($"InitialiseTrack: {trackInfo.TrackName} - Mode:{trackContext.GameMode}," +
                $" Players:{trackContext.PlayerCount}");
            foreach (LevelInitStepSO step in trackInfo.StepOrder)
            {
                try
                {
                    //Debug.Log($"Starting: {step.name}");
                    await step.Run(trackContext);
                    completed += step.Weight;
                    LoadingScreen.Instance.UpdateLoadingProgress(completed);
                    //Debug.Log($"Completed: {step.name} ({completed}/{totalWeight})");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Init step {step.GetType().Name} failed: {ex}");
                    break;
                }
            }
        }
    }
}

