using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "Init Map Step", menuName = "Levels/InitSteps/InitMapStep")]
    public class InitMapStepSO : LevelInitStepSO
    {
        [SerializeField] private AssetReference sceneReference;

        public override async Task Run(TrackContext context)
        {
            Scene currentActiveScene = SceneManager.GetActiveScene();
            if (currentActiveScene != null)
            {
                Debug.Log($"Current Active Scene is {currentActiveScene.name}");
            }

            AsyncOperationHandle<SceneInstance> handle = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Map scene {sceneReference.RuntimeKey} loaded.");
                context.SceneHandle = handle;
                SceneManager.SetActiveScene(handle.Result.Scene);
            }
            else
            {
                Debug.LogError($"Failed to load map: {sceneReference.RuntimeKey}");
            }

            await SceneManager.UnloadSceneAsync(currentActiveScene);

        }
    }
}

