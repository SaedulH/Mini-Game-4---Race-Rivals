using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CoreSystem
{
    /// <summary>
    /// Bootstrapper is responsible for loading the core scenes of the game.
    /// It first loads the PersistentScene additively to ensure that essential managers and systems remain active throughout the game.
    /// After that, it loads the MainMenu scene, which serves as the entry point for players.
    /// </summary>
    [DefaultExecutionOrder(-100)] // Ensure this runs before other scripts
    public class Bootstrapper : MonoBehaviour
    {
        private async void Start()
        {
            Scene bootstrapScene = SceneManager.GetActiveScene();
            if (bootstrapScene.name != "Bootstrapper")
            {
                Debug.LogError("Bootstrapper script is not in the Bootstrapper scene.");
                return;
            }

            // Load CoreScene
            AsyncOperationHandle<SceneInstance> handlePersistent = Addressables.LoadSceneAsync("CoreScene", LoadSceneMode.Additive);
            await handlePersistent.Task;

            if (handlePersistent.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("CoreScene loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load CoreScene.");
            }

            // Load MainMenu
            AsyncOperationHandle<SceneInstance> handleMenu = Addressables.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            await handleMenu.Task;

            if (handleMenu.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("MainMenu loaded successfully.");
                SceneManager.SetActiveScene(handleMenu.Result.Scene);
            }
            else
            {
                Debug.LogError("Failed to load MainMenu.");
            }

            // 3. Unload the Bootstrapper scene
            await SceneManager.UnloadSceneAsync(bootstrapScene);
        }
    }
}