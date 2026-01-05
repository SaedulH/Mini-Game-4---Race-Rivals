using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour
{
    [field: SerializeField] public VisualElement Root { get; set; }
    [field: SerializeField] public VisualElement PauseScreen { get; set; }
    [field: SerializeField] public Button PlayButton { get; set; }
    [field: SerializeField] public Button RestartButton { get; set; }
    [field: SerializeField] public Button QuitButton { get; set; }

    private void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        PauseScreen = Root.Q<VisualElement>("PauseScreen");

        PlayButton = PauseScreen.Q<Button>("Play");
        PlayButton.clicked += OnPlayClicked;

        RestartButton = PauseScreen.Q<Button>("Restart");
        RestartButton.clicked += OnRestartClicked;

        QuitButton = PauseScreen.Q<Button>("Quit");
        QuitButton.clicked += OnQuitClicked;
    }

    private void OnDisable()
    {
        PlayButton.clicked -= OnPlayClicked;
        RestartButton.clicked -= OnRestartClicked;
        QuitButton.clicked -= OnQuitClicked;
    }

    private void OnApplicationPause(bool pause)
    {
        // Automatically show pause menu when application is paused
    }

    private void OnPauseInput()
    {
        GameManager.Instance.PauseGame();
    }

    private void OnPlayClicked()
    {
        GameManager.Instance.UnpauseGame();
    }

    private void OnRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }

    private void OnQuitClicked()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
