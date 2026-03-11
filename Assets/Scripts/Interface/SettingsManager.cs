using System;
using AudioSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class SettingsManager : NonPersistentSingleton<SettingsManager>
{
    [field: SerializeField] public VisualElement Root { get; set; }
    [field: SerializeField] public Button GameButton { get; set; }
    [field: SerializeField] public Button AudioButton { get; set; }
    [field: SerializeField] public Button ControlsButton { get; set; }

    // Game Settings
    [field: SerializeField] public VisualElement GameSettings { get; set; }
    [field: SerializeField] public Button EasyButton { get; set; }
    [field: SerializeField] public Button HardButton { get; set; }
    [field: SerializeField] public Button FixedButton { get; set; }
    [field: SerializeField] public Button DynamicButton { get; set; }
    [field: SerializeField] public Button OffButton { get; set; }
    [field: SerializeField] public Button LowButton { get; set; }
    [field: SerializeField] public Button HighButton { get; set; }

    // Audio Settings
    [field: SerializeField] public VisualElement AudioSettings { get; set; }
    [field: SerializeField] public SliderInt MasterVolumeSlider { get; set; }
    [field: SerializeField] public SliderInt MusicVolumeSlider { get; set; }
    [field: SerializeField] public SliderInt UIVolumeSlider { get; set; }
    [field: SerializeField] public SliderInt VfxVolumeSlider { get; set; }

    // Control Settings
    [field: SerializeField] public VisualElement ControlsSettings { get; set; }
    // Player One
    [field: SerializeField] public TextField PlayerOneThrottleInput { get; set; }
    [field: SerializeField] public TextField PlayerOneReverseInput { get; set; }
    [field: SerializeField] public TextField PlayerOneLeftInput { get; set; }
    [field: SerializeField] public TextField PlayerOneRightInput { get; set; }
    [field: SerializeField] public TextField PlayerOneHandbrakeInput { get; set; }
    // Player Two
    [field: SerializeField] public TextField PlayerTwoThrottleInput { get; set; }
    [field: SerializeField] public TextField PlayerTwoReverseInput { get; set; }
    [field: SerializeField] public TextField PlayerTwoLeftInput { get; set; }
    [field: SerializeField] public TextField PlayerTwoRightInput { get; set; }
    [field: SerializeField] public TextField PlayerTwoHandbrakeInput { get; set; }

    // Footer
    [field: SerializeField] public Button BackButton { get; set; }
    [field: SerializeField] public Button ResetButton { get; set; }
    [field: SerializeField] public Action HideSettingsAction { get; set; }
    [field: SerializeField] public SettingScreenType CurrentScreen { get; private set; }

    [field: Header("Audio")]
    [field: SerializeField] public AudioData SelectAudio { get; set; }
    [field: SerializeField] public AudioData BackAudio { get; set; }
    [field: SerializeField] public AudioData HoverAudio { get; set; }
    [field: SerializeField] public float ScreenTransitionTime { get; private set; } = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        Root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void Start()
    {
        InitialiseSettings();
    }

    private void OnEnable()
    {
        VisualElement Settings = Root.Q<VisualElement>("Settings");
        //Settings Screen
        GameSettings = Settings.Q<VisualElement>("GameSettings");
        AudioSettings = Settings.Q<VisualElement>("AudioSettings");
        ControlsSettings = Settings.Q<VisualElement>("ControlsSettings");

        GameButton = Settings.Q<Button>("Game");
        GameButton.clicked += () => OnGameClicked();

        AudioButton = Settings.Q<Button>("Audio");
        AudioButton.clicked += () => OnAudioClicked();

        ControlsButton = Settings.Q<Button>("Controls");
        ControlsButton.clicked += () => OnControlsClicked();

        //Game Settings
        EasyButton = GameSettings.Q<Button>("Easy");
        EasyButton.clicked += () => OnEasyClicked();

        HardButton = GameSettings.Q<Button>("Hard");
        HardButton.clicked += () => OnHardClicked();

        FixedButton = GameSettings.Q<Button>("Fixed");
        FixedButton.clicked += () => OnFixedClicked();

        DynamicButton = GameSettings.Q<Button>("Dynamic");
        DynamicButton.clicked += () => OnDynamicClicked();

        OffButton = GameSettings.Q<Button>("Off");
        OffButton.clicked += () => OnOffClicked();

        LowButton = GameSettings.Q<Button>("Low");
        LowButton.clicked += () => OnLowClicked();

        HighButton = GameSettings.Q<Button>("High");
        HighButton.clicked += () => OnHighClicked();

        //Audio Settings
        MasterVolumeSlider = AudioSettings.Q<SliderInt>("MasterVolume");
        MasterVolumeSlider.RegisterValueChangedCallback((e) => OnMasterVolumeChanged(e.newValue));

        MusicVolumeSlider = AudioSettings.Q<SliderInt>("MusicVolume");
        MusicVolumeSlider.RegisterValueChangedCallback((e) => OnMusicVolumeChanged(e.newValue));

        UIVolumeSlider = AudioSettings.Q<SliderInt>("UIVolume");
        UIVolumeSlider.RegisterValueChangedCallback((e) => OnUIVolumeChanged(e.newValue));

        VfxVolumeSlider = AudioSettings.Q<SliderInt>("VfxVolume");
        VfxVolumeSlider.RegisterValueChangedCallback((e) => OnVfxVolumeChanged(e.newValue));

        //Control Settings
        PlayerOneThrottleInput = ControlsSettings.Q<TextField>("PlayerOneThrottleInput");
        PlayerOneThrottleInput.RegisterValueChangedCallback((e) => OnPlayerOneThrottleInputChanged(e.newValue));

        PlayerOneReverseInput = ControlsSettings.Q<TextField>("PlayerOneReverseInput");
        PlayerOneReverseInput.RegisterValueChangedCallback((e) => OnPlayerOneReverseInputChanged(e.newValue));

        PlayerOneLeftInput = ControlsSettings.Q<TextField>("PlayerOneLeftInput");
        PlayerOneLeftInput.RegisterValueChangedCallback((e) => OnPlayerOneLeftInputChanged(e.newValue));

        PlayerOneRightInput = ControlsSettings.Q<TextField>("PlayerOneRightInput");
        PlayerOneRightInput.RegisterValueChangedCallback((e) => OnPlayerOneRightInputChanged(e.newValue));

        PlayerOneHandbrakeInput = ControlsSettings.Q<TextField>("PlayerOneHandbrakeInput");
        PlayerOneHandbrakeInput.RegisterValueChangedCallback((e) => OnPlayerOneHandbrakeInputChanged(e.newValue));

        PlayerTwoThrottleInput = ControlsSettings.Q<TextField>("PlayerTwoThrottleInput");
        PlayerTwoThrottleInput.RegisterValueChangedCallback((e) => OnPlayerTwoThrottleInputChanged(e.newValue));

        PlayerTwoReverseInput = ControlsSettings.Q<TextField>("PlayerTwoReverseInput");
        PlayerTwoReverseInput.RegisterValueChangedCallback((e) => OnPlayerTwoReverseInputChanged(e.newValue));

        PlayerTwoLeftInput = ControlsSettings.Q<TextField>("PlayerTwoLeftInput");
        PlayerTwoLeftInput.RegisterValueChangedCallback((e) => OnPlayerTwoLeftInputChanged(e.newValue));

        PlayerTwoRightInput = ControlsSettings.Q<TextField>("PlayerTwoRightInput");
        PlayerTwoRightInput.RegisterValueChangedCallback((e) => OnPlayerTwoRightInputChanged(e.newValue));

        PlayerTwoHandbrakeInput = ControlsSettings.Q<TextField>("PlayerTwoHandbrakeInput");
        PlayerTwoHandbrakeInput.RegisterValueChangedCallback((e) => OnPlayerTwoHandbrakeInputChanged(e.newValue));

        // Footer
        BackButton = Settings.Q<Button>("Back");
        BackButton.clicked += OnBackClicked;

        ResetButton = Settings.Q<Button>("Reset");
        ResetButton.clicked += OnResetClicked;

        MenuManager.Instance.ShowSettingsAction += ShowSettingsScreen;
    }
    private void OnDisable()
    {
        GameButton.clicked -= () => OnGameClicked();
        AudioButton.clicked -= () => OnAudioClicked();
        ControlsButton.clicked -= () => OnControlsClicked();
        EasyButton.clicked -= () => OnEasyClicked();
        HardButton.clicked -= () => OnHardClicked();
        FixedButton.clicked -= () => OnFixedClicked();
        DynamicButton.clicked -= () => OnDynamicClicked();
        OffButton.clicked -= () => OnOffClicked();
        LowButton.clicked -= () => OnLowClicked();
        HighButton.clicked -= () => OnHighClicked();

        MasterVolumeSlider.UnregisterValueChangedCallback((e) => OnMasterVolumeChanged(e.newValue));
        MusicVolumeSlider.UnregisterValueChangedCallback((e) => OnMusicVolumeChanged(e.newValue));
        UIVolumeSlider.UnregisterValueChangedCallback((e) => OnUIVolumeChanged(e.newValue));
        VfxVolumeSlider.UnregisterValueChangedCallback((e) => OnVfxVolumeChanged(e.newValue));
        PlayerOneThrottleInput.UnregisterValueChangedCallback((e) => OnPlayerOneThrottleInputChanged(e.newValue));
        PlayerOneReverseInput.UnregisterValueChangedCallback((e) => OnPlayerOneReverseInputChanged(e.newValue));
        PlayerOneLeftInput.UnregisterValueChangedCallback((e) => OnPlayerOneLeftInputChanged(e.newValue));
        PlayerOneRightInput.UnregisterValueChangedCallback((e) => OnPlayerOneRightInputChanged(e.newValue));
        PlayerOneHandbrakeInput.UnregisterValueChangedCallback((e) => OnPlayerOneHandbrakeInputChanged(e.newValue));
        PlayerTwoThrottleInput.UnregisterValueChangedCallback((e) => OnPlayerTwoThrottleInputChanged(e.newValue));
        PlayerTwoReverseInput.UnregisterValueChangedCallback((e) => OnPlayerTwoReverseInputChanged(e.newValue));
        PlayerTwoLeftInput.UnregisterValueChangedCallback((e) => OnPlayerTwoLeftInputChanged(e.newValue));
        PlayerTwoRightInput.UnregisterValueChangedCallback((e) => OnPlayerTwoRightInputChanged(e.newValue));
        PlayerTwoHandbrakeInput.UnregisterValueChangedCallback((e) => OnPlayerTwoHandbrakeInputChanged(e.newValue));

        BackButton.clicked -= OnBackClicked;
        ResetButton.clicked -= OnResetClicked;
        MenuManager.Instance.ShowSettingsAction -= ShowSettingsScreen;
    }

    private void InitialiseSettings()
    {
        CurrentScreen = SettingScreenType.Game;
        GameSettings.RemoveFromClassList("hideUI");
        AudioSettings.AddToClassList("hideUI");
        ControlsSettings.AddToClassList("hideUI");
    }

    #region Audio 
    private void SetupHoverAudio()
    {
        Root.Query<Button>().ForEach(button =>
        {
            bool hovered = false;
            button.RegisterCallback<PointerEnterEvent>(_ =>
            {
                if (hovered || !button.enabledSelf)
                    return;

                hovered = true;

                PlayHoverAudio();
            });

            button.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                hovered = false;
            });
        });
    }

    private void PlayBackAudio(bool playSound = true)
    {
        if (!playSound) return;
        AudioManager.Instance.CreateAudioBuilder()
            .Play(BackAudio);
    }

    private void PlaySelectAudio(bool playSound = true)
    {
        if (!playSound) return;
        AudioManager.Instance.CreateAudioBuilder()
            .WithRandomPitch(-0.05f, 0.05f)
            .Play(SelectAudio);
    }

    private void PlayHoverAudio(bool playSound = true)
    {
        if (!playSound) return;
        AudioManager.Instance.CreateAudioBuilder()
            .Play(HoverAudio);
    }
    #endregion

    #region Screen Transitions

    private void OnGameClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        Debug.Log($"Screen: Game");
        AudioSettings.SetEnabled(true);
        ControlsButton.SetEnabled(true);
        GameButton.SetEnabled(false);
    }

    private void OnAudioClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        Debug.Log($"Screen: Audio");
        ControlsButton.SetEnabled(true);
        GameButton.SetEnabled(true);
        AudioSettings.SetEnabled(false);
    }

    private void OnControlsClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        Debug.Log($"Screen: Controls");
        AudioSettings.SetEnabled(false);
        GameButton.SetEnabled(false);
        ControlsButton.SetEnabled(true);

        CurrentScreen = SettingScreenType.Game;
    }

    private void OnBackClicked()
    {
        throw new NotImplementedException();
    }

    private void OnResetClicked()
    {
        throw new NotImplementedException();
    }

    private void ShowSettingsScreen()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Game Settings

    private void OnEasyClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    private void OnHardClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    private void OnFixedClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    private void OnDynamicClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    private void OnOffClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    private void OnLowClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    private void OnHighClicked(bool playSound = true)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Audio Settings

    private void OnMasterVolumeChanged(int newValue)
    {
        throw new NotImplementedException();
    }

    private void OnMusicVolumeChanged(int newValue)
    {
        throw new NotImplementedException();
    }

    private void OnUIVolumeChanged(int newValue)
    {
        throw new NotImplementedException();
    }

    private void OnVfxVolumeChanged(int newValue)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Controls Settings

    private void OnPlayerOneThrottleInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerOneReverseInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerOneLeftInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerOneRightInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerOneHandbrakeInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerTwoThrottleInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerTwoReverseInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerTwoLeftInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerTwoRightInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    private void OnPlayerTwoHandbrakeInputChanged(string newValue)
    {
        throw new NotImplementedException();
    }

    #endregion

}
