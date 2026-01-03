using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AudioSystem;
using CoreSystem;
using UnityEngine;
using UnityEngine.UIElements;
using static Utilities.Constants;

public class MenuManager : MonoBehaviour
{
    [field: SerializeField] public VisualElement Root { get; set; }
    // Home Screen
    [field: SerializeField] public VisualElement HomeScreen { get; set; }
    [field: SerializeField] public VisualElement SetupScreens { get; set; }
    [field: SerializeField] public Button PlayButton { get; set; }
    [field: SerializeField] public Button QuitButton { get; set; }

    // Game Selection
    [field: SerializeField] public VisualElement SelectionScreen { get; set; }
    [field: SerializeField] public Button TimedModeButton { get; set; }
    [field: SerializeField] public Button RaceModeButton { get; set; }
    [field: SerializeField] public Button OnePlayerButton { get; set; }
    [field: SerializeField] public Button TwoPlayerButton { get; set; }
    [field: SerializeField] public Button TrackOneButton { get; set; }
    [field: SerializeField] public Button TrackTwoButton { get; set; }
    [field: SerializeField] public Button TrackThreeButton { get; set; }
    [field: SerializeField] public Button StartButton { get; set; }
    [field: SerializeField] public Button BackButton { get; set; }
    [field: SerializeField] public VisualElement SelectedMapImage { get; set; }

    // Vehicle Selection
    [field: Header("Vehicle Selection")]
    [field: SerializeField] public VisualElement VehicleScreen { get; set; }
    [field: SerializeField] public VehicleSelector VehicleOneSelector { get; set; }
    [field: SerializeField] public Vehicle SelectedVehicleOne { get; set; }
    [field: SerializeField] public VisualElement VehicleOneImage { get; set; }

    private int _currentVehicleOneIndex = 0;
    [field: SerializeField] public VehicleSelector VehicleTwoSelector { get; set; }
    [field: SerializeField] public Vehicle SelectedVehicleTwo { get; set; }
    [field: SerializeField] public VisualElement VehicleTwoImage { get; set; }

    private int _currentVehicleTwoIndex = 0;

    [field: SerializeField] public GroupBox PlayerOneVehicleSelection { get; set; }
    [field: SerializeField] public List<Slider> PlayerOneSliders { get; set; }
    [field: SerializeField] public Button VehicleOneLeft { get; set; }
    [field: SerializeField] public Button VehicleOneRight { get; set; }
    [field: SerializeField] public Label VehicleOneName { get; set; }
    [field: SerializeField] public GroupBox PlayerTwoVehicleSelection { get; set; }
    [field: SerializeField] public List<Slider> PlayerTwoSliders { get; set; }
    [field: SerializeField] public Button VehicleTwoLeft { get; set; }
    [field: SerializeField] public Button VehicleTwoRight { get; set; }
    [field: SerializeField] public Label VehicleTwoName { get; set; }

    [field: Header("Audio")]
    [field: SerializeField] public AudioData PlayAudio { get; set; }
    [field: SerializeField] public AudioData SelectAudio { get; set; }
    [field: SerializeField] public AudioData BackAudio { get; set; }
    [field: SerializeField] public AudioData HoverAudio { get; set; }

    [field: SerializeField] public MenuScreenType CurrentScreen { get; private set; }

    private GameMode _gameMode;
    private int _playerCount;
    private int _trackNum;
    private Coroutine _setPlayerOneSlidersCoroutine;
    private Coroutine _setPlayerTwoSlidersCoroutine;

    [field: SerializeField] public float ScreenTransitionTime { get; private set; } = 0.1f;
    [field: SerializeField] public float SliderLerpSpeed { get; private set; } = 50f;
    [field: SerializeField] public List<TrackInfo> TrackInfo { get; private set; }

    private void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
    }
    private void Start()
    {
        InitialiseMenu();
    }

    private void OnEnable()
    {
        VisualElement MainMenu = Root.Q<VisualElement>("MainMenu");
        // Home Screen
        HomeScreen = Root.Q<VisualElement>("HomeScreen");
        SetupScreens = Root.Q<VisualElement>("SetupScreens");

        PlayButton = MainMenu.Q<Button>("Play");
        PlayButton.clicked += OnPlayClicked;

        QuitButton = MainMenu.Q<Button>("Quit");
        QuitButton.clicked += OnQuitClicked;
        // Game Selection
        SelectionScreen = MainMenu.Q<VisualElement>("SelectionScreen");

        RaceModeButton = SelectionScreen.Q<Button>("Race");
        RaceModeButton.clicked += () => OnRaceModeClicked();

        TimedModeButton = SelectionScreen.Q<Button>("Timed");
        TimedModeButton.clicked += () => OnTimedModeClicked();

        OnePlayerButton = SelectionScreen.Q<Button>("OnePlayer");
        OnePlayerButton.clicked += () => OnOnePlayerClicked();

        TwoPlayerButton = SelectionScreen.Q<Button>("TwoPlayer");
        TwoPlayerButton.clicked += () => OnTwoPlayerClicked();

        TrackOneButton = SelectionScreen.Q<Button>("TrackOne");
        TrackOneButton.clicked += () => OnTrackOneClicked();

        TrackTwoButton = SelectionScreen.Q<Button>("TrackTwo");
        TrackTwoButton.clicked += () => OnTrackTwoClicked();

        TrackThreeButton = SelectionScreen.Q<Button>("TrackThree");
        TrackThreeButton.clicked += () => OnTrackThreeClicked();

        SelectedMapImage = SelectionScreen.Q<VisualElement>("MapImage");

        StartButton = MainMenu.Q<Button>("Start");
        StartButton.clicked += OnStartClicked;

        BackButton = MainMenu.Q<Button>("Back");
        BackButton.clicked += OnBackClicked;

        // Vehicle Selection
        VehicleScreen = MainMenu.Q<VisualElement>("VehicleScreen");
        PlayerOneVehicleSelection = VehicleScreen.Q<GroupBox>("PlayerOneVehicle");
        PlayerTwoVehicleSelection = VehicleScreen.Q<GroupBox>("PlayerTwoVehicle");

        VehicleOneLeft = PlayerOneVehicleSelection.Q<Button>("VehicleOneLeft");
        VehicleOneLeft.clicked += OnVehicleOneLeftClicked;

        VehicleOneRight = PlayerOneVehicleSelection.Q<Button>("VehicleOneRight");
        VehicleOneRight.clicked += OnVehicleOneRightClicked;

        VehicleOneName = PlayerOneVehicleSelection.Q<Label>("VehicleOneName");
        VehicleOneImage = PlayerOneVehicleSelection.Q<VisualElement>("VehicleOneImage");
        PlayerOneSliders = PlayerOneVehicleSelection.Query<Slider>().ToList();
        SetupSliders(PlayerOneSliders, "One");

        VehicleTwoLeft = PlayerTwoVehicleSelection.Q<Button>("VehicleTwoLeft");
        VehicleTwoLeft.clicked += OnVehicleTwoLeftClicked;

        VehicleTwoRight = PlayerTwoVehicleSelection.Q<Button>("VehicleTwoRight");
        VehicleTwoRight.clicked += OnVehicleTwoRightClicked;

        VehicleTwoName = PlayerTwoVehicleSelection.Q<Label>("VehicleTwoName");
        VehicleTwoImage = PlayerTwoVehicleSelection.Q<VisualElement>("VehicleTwoImage");
        PlayerTwoSliders = PlayerTwoVehicleSelection.Query<Slider>().ToList();
        SetupSliders(PlayerTwoSliders, "Two");

        SetupHoverAudio();
    }

    private void OnDisable()
    {
        PlayButton.clicked -= OnPlayClicked;
        QuitButton.clicked -= OnQuitClicked;
        RaceModeButton.clicked -= () => OnRaceModeClicked();
        TimedModeButton.clicked -= () => OnTimedModeClicked();
        OnePlayerButton.clicked -= () => OnOnePlayerClicked();
        TwoPlayerButton.clicked -= () => OnTwoPlayerClicked();
        TrackOneButton.clicked -= () => OnTrackOneClicked();
        TrackTwoButton.clicked -= () => OnTrackTwoClicked();
        StartButton.clicked -= OnStartClicked;
        BackButton.clicked -= OnBackClicked;

        VehicleOneLeft.clicked -= OnVehicleOneLeftClicked;
        VehicleOneRight.clicked -= OnVehicleOneRightClicked;
        VehicleTwoLeft.clicked -= OnVehicleTwoLeftClicked;
        VehicleTwoRight.clicked -= OnVehicleTwoRightClicked;
    }

    private void InitialiseMenu()
    {
        CurrentScreen = MenuScreenType.Home;
        HomeScreen.RemoveFromClassList("hideUI");
        SetupScreens.AddToClassList("hideUI");
        SelectionScreen.AddToClassList("hideUI");
        VehicleScreen.AddToClassList("hideUI");
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

    private void PlayForwardAudio(bool playSound = true)
    {
        if (!playSound) return;
        AudioManager.Instance.CreateAudioBuilder()
            .Play(PlayAudio);
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
    private IEnumerator ShowHomeScreen()
    {
        CurrentScreen = MenuScreenType.Home;
        SelectionScreen.AddToClassList("hideUI");
        yield return new WaitForSeconds(ScreenTransitionTime);
        SetupScreens.AddToClassList("hideUI");
        HomeScreen.RemoveFromClassList("hideUI");
    }

    private IEnumerator ShowSelectionScreen()
    {
        if (CurrentScreen == MenuScreenType.Vehicle)
        {
            VehicleScreen.AddToClassList("hideUI");
        }
        else
        {
            HomeScreen.AddToClassList("hideUI");
        }
        CurrentScreen = MenuScreenType.Selection;
        yield return new WaitForSeconds(ScreenTransitionTime);
        StartButton.text = "Next";
        SetupScreens.RemoveFromClassList("hideUI");
        SelectionScreen.RemoveFromClassList("hideUI");
    }
    private IEnumerator ShowVehicleScreen()
    {
        CurrentScreen = MenuScreenType.Vehicle;
        SelectionScreen.AddToClassList("hideUI");
        yield return new WaitForSeconds(ScreenTransitionTime);
        StartButton.text = "Start";
        VehicleScreen.RemoveFromClassList("hideUI");
    }

    private void OnPlayClicked()
    {
        //Debug.Log("Go to Selection Screen");
        PlayForwardAudio();
        ResetSelections();
        ResetVehicleSelections();
        StartCoroutine(ShowSelectionScreen());
    }

    public void OnQuitClicked()
    {
        PlayBackAudio();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnStartClicked()
    {
        PlayForwardAudio();
        if (CurrentScreen == MenuScreenType.Selection)
        {
            StartCoroutine(ShowVehicleScreen());
        }
        else if (CurrentScreen == MenuScreenType.Vehicle)
        {
            OnStartGame();
        }
    }

    public void OnBackClicked()
    {
        PlayBackAudio();
        if (CurrentScreen == MenuScreenType.Vehicle)
        {
            StartCoroutine(ShowSelectionScreen());
        }
        else if (CurrentScreen == MenuScreenType.Selection)
        {
            //Debug.Log("Back to Home Screen");
            StartCoroutine(ShowHomeScreen());
        }
    }

    #endregion

    #region Race Selection Handlers

    public void OnRaceModeClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"Mode: Race");
        RaceModeButton.SetEnabled(false);
        TimedModeButton.SetEnabled(true);

        _gameMode = GameMode.Race;
    }

    public void OnTimedModeClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"Mode: Timed");
        RaceModeButton.SetEnabled(true);
        TimedModeButton.SetEnabled(false);

        _gameMode = GameMode.Timed;
    }

    public void OnOnePlayerClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"PlayerCount: 1");

        PlayerTwoVehicleSelection.SetEnabled(false);
        SetVehicleTwo(0);

        OnePlayerButton.SetEnabled(false);
        TwoPlayerButton.SetEnabled(true);

        _playerCount = 1;
    }

    public void OnTwoPlayerClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"PlayerCount: 2");

        PlayerTwoVehicleSelection.SetEnabled(true);

        OnePlayerButton.SetEnabled(true);
        TwoPlayerButton.SetEnabled(false);

        _playerCount = 2;
    }

    public void OnTrackOneClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"Track: 1");
        TrackOneButton.SetEnabled(false);
        TrackTwoButton.SetEnabled(true);
        TrackThreeButton.SetEnabled(true);
        SelectedMapImage.style.backgroundImage = new StyleBackground(TrackInfo[0].TrackImage);
        _trackNum = 1;
    }

    public void OnTrackTwoClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"Track: 2");
        TrackOneButton.SetEnabled(true);
        TrackTwoButton.SetEnabled(false);
        TrackThreeButton.SetEnabled(true);
        SelectedMapImage.style.backgroundImage = new StyleBackground(TrackInfo[1].TrackImage);
        _trackNum = 2;
    }

    public void OnTrackThreeClicked(bool playSound = true)
    {
        PlaySelectAudio(playSound);
        //Debug.Log($"Track: 3");
        TrackOneButton.SetEnabled(true);
        TrackTwoButton.SetEnabled(true);
        TrackThreeButton.SetEnabled(false);
        SelectedMapImage.style.backgroundImage = new StyleBackground(TrackInfo[2].TrackImage);
        _trackNum = 3;
    }

    private void ResetSelections()
    {
        OnRaceModeClicked(false);
        OnOnePlayerClicked(false);
        OnTrackOneClicked(false);
    }

    #endregion

    #region Vehicle Selection Handlers

    public void OnVehicleOneLeftClicked()
    {
        if (VehicleOneSelector.AvailableVehicles.Length == 0) return;

        PlaySelectAudio();
        if (_currentVehicleOneIndex <= 0)
        {
            SetVehicleOne(VehicleOneSelector.AvailableVehicles.Length - 1);
        }
        else
        {
            SetVehicleOne(_currentVehicleOneIndex - 1);
        }
    }

    public void OnVehicleOneRightClicked()
    {
        if (VehicleOneSelector.AvailableVehicles.Length == 0) return;

        PlaySelectAudio();
        if (_currentVehicleOneIndex >= (VehicleOneSelector.AvailableVehicles.Length - 1))
        {
            SetVehicleOne(0);
        }
        else
        {
            SetVehicleOne(_currentVehicleOneIndex + 1);
        }
    }

    public void OnVehicleTwoLeftClicked()
    {
        if (VehicleTwoSelector.AvailableVehicles.Length == 0) return;

        PlaySelectAudio();
        if (_currentVehicleTwoIndex <= 0)
        {
            SetVehicleTwo(VehicleTwoSelector.AvailableVehicles.Length - 1);
        }
        else
        {
            SetVehicleTwo(_currentVehicleTwoIndex - 1);
        }
    }

    public void OnVehicleTwoRightClicked()
    {
        if (VehicleTwoSelector.AvailableVehicles.Length == 0) return;

        PlaySelectAudio();
        if (_currentVehicleTwoIndex >= (VehicleTwoSelector.AvailableVehicles.Length - 1))
        {
            SetVehicleTwo(0);
        }
        else
        {
            SetVehicleTwo(_currentVehicleTwoIndex + 1);
        }
    }

    public void SetVehicleOne(int index)
    {
        if (index < 0 || index >= VehicleOneSelector.AvailableVehicles.Length) return;

        _currentVehicleOneIndex = index;
        SelectedVehicleOne = VehicleOneSelector.AvailableVehicles[_currentVehicleOneIndex];
        VehicleOneName.text = SelectedVehicleOne.Name;
        VehicleOneImage.style.backgroundImage = new StyleBackground(SelectedVehicleOne.Icon);

        if (_setPlayerOneSlidersCoroutine != null)
        {
            StopCoroutine(_setPlayerOneSlidersCoroutine);
        }
        _setPlayerOneSlidersCoroutine = StartCoroutine(SetVehicleStats(PlayerOneSliders, SelectedVehicleOne));
    }

    public void SetVehicleTwo(int index)
    {
        if (index < 0 || index >= VehicleTwoSelector.AvailableVehicles.Length) return;

        _currentVehicleTwoIndex = index;
        SelectedVehicleTwo = VehicleTwoSelector.AvailableVehicles[_currentVehicleTwoIndex];
        VehicleTwoName.text = SelectedVehicleTwo.Name;
        VehicleTwoImage.style.backgroundImage = new StyleBackground(SelectedVehicleTwo.Icon);

        if (_setPlayerTwoSlidersCoroutine != null)
        {
            StopCoroutine(_setPlayerTwoSlidersCoroutine);
        }
        _setPlayerTwoSlidersCoroutine = StartCoroutine(SetVehicleStats(PlayerTwoSliders, SelectedVehicleTwo));
    }

    public IEnumerator SetVehicleStats(List<Slider> statSliders, Vehicle vehicle)
    {
        List<Coroutine> running = new()
        {
            // index 0 : Speed
            StartCoroutine(SetSliderValue(statSliders[0], vehicle.Speed)),
            // index 1 : Acceleration
            StartCoroutine(SetSliderValue(statSliders[1], vehicle.Acceleration)),
            // index 2 : Handling
            StartCoroutine(SetSliderValue(statSliders[2], vehicle.Handling)),
            // index 3 : Braking
            StartCoroutine(SetSliderValue(statSliders[3], vehicle.Braking))
        };
        foreach (Coroutine c in running)
        {
            yield return c;
        }
    }

    private void SetupSliders(List<Slider> sliders, string number)
    {
        List<Slider> orderedSliders = new(new Slider[4]);
        foreach (Slider slider in sliders)
        {
            slider.AddToClassList("statSlider");
            slider.SetEnabled(false);
            slider.style.opacity = 1f;
            if (slider.name.Equals($"Vehicle{number}Speed"))
            {
                orderedSliders[0] = slider;
            }
            if (slider.name.Equals($"Vehicle{number}Acceleration"))
            {
                orderedSliders[1] = slider;
            }
            if (slider.name.Equals($"Vehicle{number}Handling"))
            {
                orderedSliders[2] = slider;
            }
            if (slider.name.Equals($"Vehicle{number}Braking"))
            {
                orderedSliders[3] = slider;
            }
        }

        if (number == "One")
        {
            PlayerOneSliders = orderedSliders;
        }
        else
        {
            PlayerTwoSliders = orderedSliders;
        }
    }

    private IEnumerator SetSliderValue(Slider slider, int value)
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

    public void ResetVehicleSelections()
    {
        SetVehicleOne(0);
        SetVehicleTwo(0);
    }

    #endregion

    public async void OnStartGame()
    {
        TrackInfo trackInfo = TrackInfo[_trackNum - 1];
        int totalWeight = (int)trackInfo.StepOrder.Sum(s => s.Weight);

        if(_playerCount == 1 && _gameMode != GameMode.Timed)
        {
            _currentVehicleTwoIndex = _currentVehicleOneIndex;
        }

        TrackContext trackContext = new()
        {
            Manager = GameManager.Instance,
            SceneHandle = default,
            GameMode = _gameMode,
            PlayerCount = _playerCount,
            VehicleOneIndex = _currentVehicleOneIndex,
            VehicleTwoIndex = _currentVehicleTwoIndex,
            TotalWeight = totalWeight
        };

        await TrackInitialiser.Instance.InitialiseTrack(trackInfo, trackContext);
    }
}
