using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AudioSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Utilities;

namespace CoreSystem
{
    public class SettingsManager : MonoBehaviour
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
        [field: SerializeField] public SliderInt EffectsVolumeSlider { get; set; }
        [field: SerializeField] public List<SliderInt> VolumeSliders { get; set; }
        [field: SerializeField] public AudioMixer AudioMixer { get; set; }

        // Control Settings
        [field: SerializeField] public VisualElement ControlsSettings { get; set; }
        [field: SerializeField] public VisualElement InputPopup { get; set; }
        [field: SerializeField] public Label InputPlayerLabel { get; set; }
        [field: SerializeField] public Label InputButtonLabel { get; set; }
        [field: SerializeField] public InputActionAsset PlayerInputActions { get; set; }

        // Player One
        [field: SerializeField] public Button PlayerOneThrottleInput { get; set; }
        [field: SerializeField] public Button PlayerOneReverseInput { get; set; }
        [field: SerializeField] public Button PlayerOneLeftInput { get; set; }
        [field: SerializeField] public Button PlayerOneRightInput { get; set; }
        [field: SerializeField] public Button PlayerOneHandbrakeInput { get; set; }
        // Player Two
        [field: SerializeField] public Button PlayerTwoThrottleInput { get; set; }
        [field: SerializeField] public Button PlayerTwoReverseInput { get; set; }
        [field: SerializeField] public Button PlayerTwoLeftInput { get; set; }
        [field: SerializeField] public Button PlayerTwoRightInput { get; set; }
        [field: SerializeField] public Button PlayerTwoHandbrakeInput { get; set; }

        // Footer
        [field: SerializeField] public Button BackButton { get; set; }
        [field: SerializeField] public Button ResetButton { get; set; }
        [field: SerializeField] public Action HideSettingsAction { get; set; }
        [field: SerializeField] public SettingScreenType CurrentScreen { get; private set; }
        [field: SerializeField] public SettingScreenType CachedScreen { get; private set; }

        [field: Header("Audio")]
        [field: SerializeField] public AudioData SelectAudio { get; set; }
        [field: SerializeField] public AudioData BackAudio { get; set; }
        [field: SerializeField] public AudioData HoverAudio { get; set; }
        [field: SerializeField] public float ScreenTransitionTime { get; private set; } = 0.1f;
        [field: SerializeField] public float SliderLerpSpeed { get; private set; } = 50f;
        [field: SerializeField] public MenuManager MenuManager { get; private set; }

        private void Awake() 
        { 
            Root = GetComponent<UIDocument>().rootVisualElement;
        }

        private void Start()
        {
            InitialiseSettings();
        }

        private void OnEnable()
        {
            SettingsScreen = Root.Q<VisualElement>("Settings");
            if (MenuManager != null)
            {
                MenuManager.ShowSettingsAction += ShowSettingsScreen;
            }
            GameManager.Instance.OnBackAction += HandleBackAction;

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

            EffectsVolumeSlider = AudioSettings.Q<SliderInt>("EffectsVolume");
            EffectsVolumeSlider.RegisterValueChangedCallback((e) => OnEffectsVolumeChanged(e.newValue));

            VolumeSliders = AudioSettings.Query<SliderInt>().ToList();
            SetupSliders(VolumeSliders);

            //Control Settings
            PlayerOneThrottleInput = ControlsSettings.Q<Button>("PlayerOneThrottleInput");
            PlayerOneThrottleInput.clicked += () => OnPlayerOneThrottleInputChanged();

            PlayerOneReverseInput = ControlsSettings.Q<Button>("PlayerOneReverseInput");
            PlayerOneReverseInput.clicked += () => OnPlayerOneReverseInputChanged();

            PlayerOneLeftInput = ControlsSettings.Q<Button>("PlayerOneLeftInput");
            PlayerOneLeftInput.clicked += () => OnPlayerOneLeftInputChanged();

            PlayerOneRightInput = ControlsSettings.Q<Button>("PlayerOneRightInput");
            PlayerOneRightInput.clicked += () => OnPlayerOneRightInputChanged();

            PlayerOneHandbrakeInput = ControlsSettings.Q<Button>("PlayerOneHandbrakeInput");
            PlayerOneHandbrakeInput.clicked += () => OnPlayerOneHandbrakeInputChanged();

            PlayerTwoThrottleInput = ControlsSettings.Q<Button>("PlayerTwoThrottleInput");
            PlayerTwoThrottleInput.clicked += () => OnPlayerTwoThrottleInputChanged();

            PlayerTwoReverseInput = ControlsSettings.Q<Button>("PlayerTwoReverseInput");
            PlayerTwoReverseInput.clicked += () => OnPlayerTwoReverseInputChanged();

            PlayerTwoLeftInput = ControlsSettings.Q<Button>("PlayerTwoLeftInput");
            PlayerTwoLeftInput.clicked += () => OnPlayerTwoLeftInputChanged();

            PlayerTwoRightInput = ControlsSettings.Q<Button>("PlayerTwoRightInput");
            PlayerTwoRightInput.clicked += () => OnPlayerTwoRightInputChanged();

            PlayerTwoHandbrakeInput = ControlsSettings.Q<Button>("PlayerTwoHandbrakeInput");
            PlayerTwoHandbrakeInput.clicked += () => OnPlayerTwoHandbrakeInputChanged();

            InputPopup = SettingsScreen.Q<VisualElement>("InputPopup");
            InputPlayerLabel = InputPopup.Q<Label>("InputPlayerLabel");
            InputButtonLabel = InputPopup.Q<Label>("InputButtonLabel");

            // Footer
            BackButton = SettingsScreen.Q<Button>("Back");
            BackButton.clicked += OnBackClicked;

            ResetButton = SettingsScreen.Q<Button>("Reset");
            ResetButton.clicked += OnResetClicked;
        }

        private void OnDisable()
        {
            if (MenuManager != null)
            {
                MenuManager.ShowSettingsAction -= ShowSettingsScreen;
            }
            GameManager.Instance.OnBackAction -= HandleBackAction;

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
            EffectsVolumeSlider.UnregisterValueChangedCallback((e) => OnEffectsVolumeChanged(e.newValue));

            PlayerOneThrottleInput.clicked -= () => OnPlayerOneThrottleInputChanged();
            PlayerOneReverseInput.clicked -= () => OnPlayerOneReverseInputChanged();
            PlayerOneLeftInput.clicked -= () => OnPlayerOneLeftInputChanged();
            PlayerOneRightInput.clicked -= () => OnPlayerOneRightInputChanged();
            PlayerOneHandbrakeInput.clicked -= () => OnPlayerOneHandbrakeInputChanged();
            PlayerTwoThrottleInput.clicked -= () => OnPlayerTwoThrottleInputChanged();
            PlayerTwoReverseInput.clicked -= () => OnPlayerTwoReverseInputChanged();
            PlayerTwoLeftInput.clicked -= () => OnPlayerTwoLeftInputChanged();
            PlayerTwoRightInput.clicked -= () => OnPlayerTwoRightInputChanged();
            PlayerTwoHandbrakeInput.clicked -= () => OnPlayerTwoHandbrakeInputChanged();

            BackButton.clicked -= OnBackClicked;
            ResetButton.clicked -= OnResetClicked;

        }

        public void Initialize(MenuManager menuManager)
        {
            this.MenuManager = menuManager;
            MenuManager.ShowSettingsAction += ShowSettingsScreen;
        }

        private void InitialiseSettings()
        {
            CurrentScreen = SettingScreenType.Game;
            SettingsScreen.AddToClassList("hideUI");
            GameSettings.RemoveFromClassList("hideUI");
            AudioSettings.AddToClassList("hideUI");
            ControlsSettings.AddToClassList("hideUI");
            InputPopup.AddToClassList("hideUI");

            SetupHoverAudio();
            SetupAudioMixer();
        }

        private void HandleBackAction()
        {
            switch (CurrentScreen)
            {
                case SettingScreenType.Game:
                case SettingScreenType.Audio:
                case SettingScreenType.Controls:
                    OnBackClicked();
                    break;
                case SettingScreenType.InputPopup:
                    HideInputPopup();
                    break;
            }
        }

        #region Audio 

        private void SetupAudioMixer()
        {
            SetMasterVolumeSetting(GetMasterVolumeSetting());
            SetMusicVolumeSetting(GetMusicVolumeSetting());
            SetUIVolumeSetting(GetUIVolumeSetting());
            SetEffectsVolumeSetting(GetEffectsVolumeSetting());
        }

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

            switch (CachedScreen)
            {
                case SettingScreenType.Game:
                        OnGameClicked(false);
                        break;
                    case SettingScreenType.Audio:
                        OnAudioClicked(false);
                        break;
                    case SettingScreenType.Controls:
                        OnControlsClicked(false);
                        break;
            }

            await Task.Delay(200);
        }

        private async void HideSettingsScreen()
        {
            CachedScreen = CurrentScreen;
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
            AudioButton.SetEnabled(true);
            ControlsButton.SetEnabled(true);

            StartCoroutine(ShowGameSettingsScreen());
        }

        private void OnAudioClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            Debug.Log($"Screen: Audio");
            GameButton.SetEnabled(true);
            AudioButton.SetEnabled(false);
            ControlsButton.SetEnabled(true);

            StartCoroutine(ShowAudioSettingsScreen());
        }

        private void OnControlsClicked(bool playSound = true)
        {
            PlaySelectAudio(playSound);
            Debug.Log($"Screen: Controls");
            GameButton.SetEnabled(true);
            AudioButton.SetEnabled(true);
            ControlsButton.SetEnabled(false);

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
            OnEffectsVolumeChanged(100);

            GetPlayerAudioSettings();
        }

        private void ResetControlsSettingsToDefault()
        {
            //OnPlayerOneThrottleInputChanged("W", false);
            //OnPlayerOneReverseInputChanged("S", false);
            //OnPlayerOneLeftInputChanged("A", false);
            //OnPlayerOneRightInputChanged("D", false);
            //OnPlayerOneHandbrakeInputChanged("Space", false);

            //OnPlayerTwoThrottleInputChanged("UpArrow", false);
            //OnPlayerTwoReverseInputChanged("DownArrow", false);
            //OnPlayerTwoLeftInputChanged("LeftArrow", false);
            //OnPlayerTwoRightInputChanged("RightArrow", false);
            //OnPlayerTwoHandbrakeInputChanged(".", false);
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
            StartCoroutine(SetSliderValue(VolumeSliders[3], GetEffectsVolumeSetting()));
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
                if (slider.name.Equals("EffectsVolume"))
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

        private void OnEffectsVolumeChanged(int newValue)
        {
            SetEffectsVolumeSetting(newValue);
        }

        private void SetMasterVolumeSetting(int masterVolume)
        {
            PlayerPrefs.SetInt(Constants.MASTER_AUDIO_MIXER, masterVolume);
            SetMixerVolume(Constants.MASTER_AUDIO_MIXER, masterVolume);
        }

        private int GetMasterVolumeSetting()
        {
            return PlayerPrefs.GetInt(Constants.MASTER_AUDIO_MIXER, 100);
        }

        private void SetMusicVolumeSetting(int musicVolume)
        {
            PlayerPrefs.SetInt(Constants.MUSIC_AUDIO_MIXER, musicVolume);
            SetMixerVolume(Constants.MUSIC_AUDIO_MIXER, musicVolume);
        }

        private int GetMusicVolumeSetting()
        {
            return PlayerPrefs.GetInt(Constants.MUSIC_AUDIO_MIXER, 100);
        }

        private void SetUIVolumeSetting(int uiVolume)
        {
            PlayerPrefs.SetInt(Constants.UI_AUDIO_MIXER, uiVolume);
            SetMixerVolume(Constants.UI_AUDIO_MIXER, uiVolume);
        }

        private int GetUIVolumeSetting()
        {
            return PlayerPrefs.GetInt(Constants.UI_AUDIO_MIXER, 100);
        }

        private void SetEffectsVolumeSetting(int effectsVolume)
        {
            PlayerPrefs.SetInt(Constants.EFFECTS_AUDIO_MIXER, effectsVolume);
            SetMixerVolume(Constants.EFFECTS_AUDIO_MIXER, effectsVolume);
        }

        private int GetEffectsVolumeSetting()
        {
            return PlayerPrefs.GetInt(Constants.EFFECTS_AUDIO_MIXER, 100);
        }

        private void SetMixerVolume(string mixerGroup, int channelVolume)
        {
            if (AudioMixer == null) return;

            float channel = channelVolume / 100f;

            if (channel <= 0.0001f)
            {
                AudioMixer.SetFloat(mixerGroup, -80f);
                return;
            }

            float db = Mathf.Log10(channel) * 20f;
            AudioMixer.SetFloat(mixerGroup, db);
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

        private void OnPlayerOneThrottleInputChanged(bool playSound = true)
        {
            ShowInputPopup(1, ControlInput.Throttle);
        }

        private void OnPlayerOneReverseInputChanged(bool playSound = true)
        {
            ShowInputPopup(1, ControlInput.Reverse);
        }

        private void OnPlayerOneLeftInputChanged(bool playSound = true)
        {
            ShowInputPopup(1, ControlInput.Left);
        }

        private void OnPlayerOneRightInputChanged(bool playSound = true)
        {
            ShowInputPopup(1, ControlInput.Right);
        }

        private void OnPlayerOneHandbrakeInputChanged(bool playSound = true)
        {
            ShowInputPopup(1, ControlInput.Handbrake);
        }

        private void OnPlayerTwoThrottleInputChanged(bool playSound = true)
        {
            ShowInputPopup(2, ControlInput.Throttle);
        }

        private void OnPlayerTwoReverseInputChanged(bool playSound = true)
        {
            ShowInputPopup(2, ControlInput.Reverse);
        }

        private void OnPlayerTwoLeftInputChanged(bool playSound = true)
        {
            ShowInputPopup(2, ControlInput.Left);
        }

        private void OnPlayerTwoRightInputChanged( bool playSound = true)
        {
            ShowInputPopup(2, ControlInput.Right);
        }

        private void OnPlayerTwoHandbrakeInputChanged(bool playSound = true)
        {
            ShowInputPopup(2, ControlInput.Handbrake);
        }

        private async void ShowInputPopup(int playerIndex, ControlInput controlInput)
        {
            CurrentScreen = SettingScreenType.InputPopup;
            string playerNumber = playerIndex == 1 ? "One" : "Two";
            InputPlayerLabel.text = $"Player {playerNumber}";
            InputButtonLabel.text = controlInput.ToString();
            InputPopup.style.display = DisplayStyle.Flex;

            await Task.Yield();
            InputPopup.RemoveFromClassList("hideUI");
            await Task.Delay(200);
        }

        private async void HideInputPopup()
        {
            PlayBackAudio();
            InputPopup.AddToClassList("hideUI");
            await Task.Delay(200);
            CurrentScreen = SettingScreenType.Controls;
            InputPopup.style.display = DisplayStyle.None;
        }

        private void SetControlInput(int playerIndex, ControlInput controlInput, string newKey)
        {
            // Save the new key for the specified control input and player index
            // Update the corresponding button text in the UI to reflect the new key binding
        }

        private void GetControlInput(int playerIndex, ControlInput controlInput)
        {
            // Retrieve the current key binding for the specified control input and player index
            // Update the corresponding button text in the UI to reflect the current key binding
        }

        #endregion
    }
}