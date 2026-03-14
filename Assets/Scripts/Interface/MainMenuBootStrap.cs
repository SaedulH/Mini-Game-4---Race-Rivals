using UnityEngine;

namespace CoreSystem
{
    public class MainMenuBootStrap : MonoBehaviour
    {
        [SerializeField] private MenuManager menuManager;
        [SerializeField] private SettingsManager settingsManager;

        private void Awake()
        {
            if (menuManager == null || settingsManager == null)
            {
                Debug.LogError("MenuUIBootstrap is missing references.");
                return;
            }

            menuManager.Initialize(settingsManager);
            settingsManager.Initialize(menuManager);
        }
    }
}