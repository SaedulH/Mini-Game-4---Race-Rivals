using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace CoreSystem
{
    public class SettingsManager : NonPersistentSingleton<SettingsManager>
    {
        [field: SerializeField] public VisualElement Root { get; set; }
        [field: SerializeField] public VisualElement SettingsScreen { get; set; }
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
        [field: SerializeField] public List<SliderInt> VolumeSliders { get; set; }

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
        [field: SerializeField] public float SliderLerpSpeed { get; private set; } = 50f;

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
            SettingsScreen = Root.Q<VisualElement>("Settings");
            //Settings Screen
            GameSettings = SettingsScreen.Q<VisualElement>("GameSettings");
            AudioSettings = SettingsScreen.Q<VisualElement>("AudioSettings");
            ControlsSettings = SettingsScreen.Q<VisualElement>("ControlSettings");

            GameButton = SettingsScreen.Q<Button>("Game");
            GameButton.clicked += () => OnGameClicked();

            AudioButton = SettingsScreen.Q<Button>("Audio");
            AudioButton.clicked += () => OnAudioClicked();

            ControlsButton = SettingsScreen.Q<Button>("Controls");
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

            VolumeSliders = AudioSettings.Query<SliderInt>().ToList();
            SetupSliders(VolumeSliders);

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
            BackButton = SettingsScreen.Q<Button>("Back");
            BackButton.clicked += OnBackClicked;

            ResetButton = SettingsScreen.Q<Button>("Reset");
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
            SettingsScreen.AddToClassList("hideUI");
            GameSettings.RemoveFromClassList("hideUI");
            AudioSettings.AddToClassList("hideUI");
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

        private IEnumerator ShowGameSettingsScreen()
        {
            CurrentScreen = SettingScreenType.Game;
            GetPlayerGameSettings();

            AudioSettings.AddToClassList("hideUI");
            ControlsSettings.AddToClassList("hideUI");
            yield return new WaitForSeconds(ScreenTransitionTime);
            GameSettings.SetEnabled(true);
            GameSettings.RemoveFromClassList("hideUI");
        }

        private IEnumerator ShowAudioSettingsScreen()
        {
            CurrentScreen = SettingScreenType.Audio;
            GetPlayerAudioSettings();

            GameSettings.AddToClassList("hideUI");
            ControlsSettings.AddToClassList("hideUI");
            yield return new WaitForSeconds(ScreenTransitionTime);
            AudioSettings.SetEnabled(true);
            AudioSettings.RemoveFromClassList("hideUI");
        }

        private IEnumerator ShowControlsSettingsScreen()
        {
            CurrentScreen = SettingScreenType.Controls;
            GetPlayerControlsSettings();

            AudioSettings.AddToClassList("hideUI");
            GameSettings.AddToClassList("hideUI");
            yield return new WaitForSeconds(ScreenTransitionTime);
            ControlsSettings.SetEnabled(true);
            ControlsSettings.RemoveFromClassList("hideUI");
        }

        private async void ShowSettingsScreen()
        {
            SettingsScreen.style.display = DisplayStyle.Flex;
            await Task.Yield();
            SettingsScreen.RemoveFromClassList("hideUI");
            OnGameClicked(false);

            await Task.Delay(200);
        }

        private async void HideSettingsScreen()
        {
            SettingsScreen.AddToClassList("hideUI");

            await Task.Delay((int)ScreenTransitionTime);
            SettingsScreen.style.display = DisplayStyle.None;
            HideSettingsAction?.Invoke();
        }

        private void OnGameClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            Debug.Log($"Screen: Game");
            GameButton.SetEnabled(false);
            AudioSettings.SetEnabled(true);
            ControlsButton.SetEnabled(true);

            StartCoroutine(ShowGameSettingsScreen());
        }

        private void OnAudioClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            Debug.Log($"Screen: Audio");
            GameButton.SetEnabled(true);
            AudioSettings.SetEnabled(false);
            ControlsButton.SetEnabled(true);

            StartCoroutine(ShowAudioSettingsScreen());
        }

        private void OnControlsClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            Debug.Log($"Screen: Controls");
            GameButton.SetEnabled(true);
            AudioSettings.SetEnabled(false);
            ControlsButton.SetEnabled(true);

            StartCoroutine(ShowControlsSettingsScreen());
        }

        private void OnBackClicked()
        {
            PlayBackAudio();
            HideSettingsScreen();
        }

        private void OnResetClicked()
        {
            switch (CurrentScreen)
            {
                case SettingScreenType.Game:
                    ResetGameSettingsToDefault();
                    break;
                case SettingScreenType.Audio:
                    ResetAudioSettingsToDefault();
                    break;
                case SettingScreenType.Controls:
                    ResetControlsSettingsToDefault();
                    break;
            }
        }

        private void ResetGameSettingsToDefault()
        {
            OnEasyClicked(false);
            OnFixedClicked(false);
            OnLowClicked(false);
        }

        private void ResetAudioSettingsToDefault()
        {
            OnMasterVolumeChanged(100);
            OnMusicVolumeChanged(100);
            OnUIVolumeChanged(100);
            OnVfxVolumeChanged(100);

            GetPlayerAudioSettings();
        }

        private void ResetControlsSettingsToDefault()
        {
            OnPlayerOneThrottleInputChanged("W", false);
            OnPlayerOneReverseInputChanged("S", false);
            OnPlayerOneLeftInputChanged("A", false);
            OnPlayerOneRightInputChanged("D", false);
            OnPlayerOneHandbrakeInputChanged("Space", false);

            OnPlayerTwoThrottleInputChanged("UpArrow", false);
            OnPlayerTwoReverseInputChanged("DownArrow", false);
            OnPlayerTwoLeftInputChanged("LeftArrow", false);
            OnPlayerTwoRightInputChanged("RightArrow", false);
            OnPlayerTwoHandbrakeInputChanged(".", false);
        }

        #endregion

        #region Game Settings

        private void GetPlayerGameSettings()
        {
            switch (GetDifficultySetting()) 
            {
                case "Easy":
                default:
                    OnEasyClicked(false);
                    break;
                case "Hard":
                    OnHardClicked(false);
                    break;
            }
            switch (GetCameraSetting()) 
            { 
                case "Fixed":
                default:
                    OnFixedClicked(false);
                    break;
                case "Dynamic":
                    OnDynamicClicked(false);
                    break;

            }
            switch (GetScreenShakeSetting()) 
            { 
                case "Off":
                    OnOffClicked(false);
                    break;
                case "Low":
                default:
                    OnLowClicked(false);
                    break;
                case "High":
                    OnHighClicked(false);
                    break;
            }
        }

        private void OnEasyClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            EasyButton.SetEnabled(false);
            HardButton.SetEnabled(true);
            SetDifficultySetting("Easy");
        }

        private void OnHardClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            EasyButton.SetEnabled(true);
            HardButton.SetEnabled(false);
            SetDifficultySetting("Hard");
        }

        private void OnFixedClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            FixedButton.SetEnabled(false);
            DynamicButton.SetEnabled(true);
            SetCameraSetting("Fixed");
        }

        private void OnDynamicClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            FixedButton.SetEnabled(true);
            DynamicButton.SetEnabled(false);
            SetCameraSetting("Dynamic");
        }

        private void OnOffClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            OffButton.SetEnabled(false);
            LowButton.SetEnabled(true);
            HighButton.SetEnabled(true);
            SetScreenShakeSetting("Off");
        }

        private void OnLowClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            OffButton.SetEnabled(true);
            LowButton.SetEnabled(false);
            HighButton.SetEnabled(true);
            SetScreenShakeSetting("Low");
        }

        private void OnHighClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            OffButton.SetEnabled(true);
            LowButton.SetEnabled(true);
            HighButton.SetEnabled(false);
            SetScreenShakeSetting("High");
        }

        private void SetDifficultySetting(string difficultySetting)
        {
            PlayerPrefs.SetString("Difficulty", difficultySetting);
        }

        private string GetDifficultySetting()
        {
            return PlayerPrefs.GetString("Difficulty");
        }

        private void SetCameraSetting(string cameraSetting)
        {
            PlayerPrefs.SetString("Camera", cameraSetting);

        }

        private string GetCameraSetting()
        {
            return PlayerPrefs.GetString("Camera");
        }

        private void SetScreenShakeSetting(string screenShakeSetting)
        {
            PlayerPrefs.SetString("ScreenShake", screenShakeSetting);
        }

        private string GetScreenShakeSetting()
        {
            return PlayerPrefs.GetString("ScreenShake");
        }

        #endregion

        #region Audio Settings

        private void GetPlayerAudioSettings()
        {
            StartCoroutine(SetSliderValue(VolumeSliders[0], GetMasterVolumeSetting()));
            StartCoroutine(SetSliderValue(VolumeSliders[1], GetMusicVolumeSetting()));
            StartCoroutine(SetSliderValue(VolumeSliders[2], GetUIVolumeSetting()));
            StartCoroutine(SetSliderValue(VolumeSliders[3], GetVfxVolumeSetting()));
        }

        private void SetupSliders(List<SliderInt> sliders)
        {
            List<SliderInt> orderedSliders = new(new SliderInt[4]);
            foreach (SliderInt slider in sliders)
            {
                slider.AddToClassList("audioSlider");
                slider.SetEnabled(true);
                slider.style.opacity = 1f;
                if (slider.name.Equals("MasterVolume"))
                {
                    orderedSliders[0] = slider;
                }
                if (slider.name.Equals("MusicVolume"))
                {
                    orderedSliders[1] = slider;
                }
                if (slider.name.Equals("UIVolume"))
                {
                    orderedSliders[2] = slider;
                }
                if (slider.name.Equals("VfxVolume"))
                {
                    orderedSliders[3] = slider;
                }
            }

            VolumeSliders = orderedSliders; 
        }

        private void OnMasterVolumeChanged(int newValue)
        {
            SetMasterVolumeSetting(newValue);
        }

        private void OnMusicVolumeChanged(int newValue)
        {
            SetMusicVolumeSetting(newValue);
        }

        private void OnUIVolumeChanged(int newValue)
        {
            SetUIVolumeSetting(newValue);
        }

        private void OnVfxVolumeChanged(int newValue)
        {
            SetVfxVolumeSetting(newValue);
        }

        private void SetMasterVolumeSetting(int masterVolume)
        {
            PlayerPrefs.SetInt("MasterVolume", masterVolume);
        }

        private int GetMasterVolumeSetting()
        {
            return PlayerPrefs.GetInt("MasterVolume");
        }

        private void SetMusicVolumeSetting(int musicVolume)
        {
            PlayerPrefs.SetInt("MusicVolume", musicVolume);
        }

        private int GetMusicVolumeSetting()
        {
            return PlayerPrefs.GetInt("MusicVolume");
        }

        private void SetUIVolumeSetting(int uiVolume)
        {
            PlayerPrefs.SetInt("UIVolume", uiVolume);
        }

        private int GetUIVolumeSetting()
        {
            return PlayerPrefs.GetInt("UIVolume");
        }

        private void SetVfxVolumeSetting(int vfxVolume)
        {
            PlayerPrefs.SetInt("VfxVolume", vfxVolume);
        }

        private int GetVfxVolumeSetting()
        {
            return PlayerPrefs.GetInt("VfxVolume");
        }

        private IEnumerator SetSliderValue(SliderInt slider, int value)
        {
            float current = slider.value;

            while (!Mathf.Approximately(current, value))
            {
                current = Mathf.Lerp(current, value, Time.deltaTime * SliderLerpSpeed);
                slider.SetValueWithoutNotify(Mathf.RoundToInt(current));
                yield return null;
            }

            slider.SetValueWithoutNotify(value);
        }

        #endregion

        #region Controls Settings

        private void GetPlayerControlsSettings()
        {
            //throw new NotImplementedException();
        }

        private void OnPlayerOneThrottleInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerThrottleInput(1, newValue);
        }

        private void OnPlayerOneReverseInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerReverseInput(1, newValue);
        }

        private void OnPlayerOneLeftInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerLeftInput(1, newValue);
        }

        private void OnPlayerOneRightInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerRightInput(1, newValue);
        }

        private void OnPlayerOneHandbrakeInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerHandbrakeInput(1, newValue);
        }

        private void OnPlayerTwoThrottleInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerThrottleInput(2, newValue);
        }

        private void OnPlayerTwoReverseInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerReverseInput(2, newValue);
        }

        private void OnPlayerTwoLeftInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerLeftInput(2, newValue);
        }

        private void OnPlayerTwoRightInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerRightInput(2, newValue);
        }

        private void OnPlayerTwoHandbrakeInputChanged(string newValue, bool playSound = true)
        {
            SetPlayerHandbrakeInput(2, newValue);
        }

        private void SetPlayerThrottleInput(int playerIndex, string input)
        {
            if (playerIndex == 1)
            {
                PlayerPrefs.SetString("PlayerOneThrottle", input);

            } 
            else if (playerIndex == 2)
            {
                PlayerPrefs.SetString("PlayerTwoThrottle", input);
            }
        }

        private void SetPlayerReverseInput(int playerIndex, string input)
        {
            if (playerIndex == 1)
            {
                PlayerPrefs.SetString("PlayerOneReverse", input);

            }
            else if (playerIndex == 2)
            {
                PlayerPrefs.SetString("PlayerTwoReverse", input);
            }
        }

        private void SetPlayerLeftInput(int playerIndex, string input)
        {
            if (playerIndex == 1)
            {
                PlayerPrefs.SetString("PlayerOneLeft", input);

            }
            else if (playerIndex == 2)
            {
                PlayerPrefs.SetString("PlayerTwoLeft", input);
            }
        }

        private void SetPlayerRightInput(int playerIndex, string input)
        {
            if (playerIndex == 1)
            {
                PlayerPrefs.SetString("PlayerOneRight", input);

            }
            else if (playerIndex == 2)
            {
                PlayerPrefs.SetString("PlayerTwoRight", input);
            }
        }

        private void SetPlayerHandbrakeInput(int playerIndex, string input)
        {
            if (playerIndex == 1)
            {
                PlayerPrefs.SetString("PlayerOneHandbrake", input);
            }
            else if (playerIndex == 2)
            {
                PlayerPrefs.SetString("PlayerTwoHandbrake", input);
            }
        }

        #endregion

    }
}