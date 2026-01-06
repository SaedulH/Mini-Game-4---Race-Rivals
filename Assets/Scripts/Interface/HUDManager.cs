using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreSystem;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

public class HUDManager : NonPersistentSingleton<HUDManager>
{
    [field: SerializeField] public VisualElement Root { get; set; }
    [field: SerializeField] public VisualElement HUDElement { get; set; }

    // Timer Elements
    [field: SerializeField] public VisualElement TimerElement { get; set; }
    [field: SerializeField] public Label CentralTimer { get; set; }
    [field: SerializeField] public Label MedalText { get; set; }
    [field: SerializeField] public VisualElement CountdownPopup { get; set; }
    [field: SerializeField] public Label CountdownValue { get; set; }

    // Player One HUD Elements
    [field: SerializeField] public VisualElement PlayerOneHUD { get; set; }
    [field: SerializeField] public VisualElement PlayerOneHUDBackground { get; set; }
    [field: SerializeField] public Label PlayerOneName { get; set; }
    [field: SerializeField] public Label PlayerOnePosition { get; set; }
    [field: SerializeField] public Label PlayerOnePositionOrdinal { get; set; }
    [field: SerializeField] public Label PlayerOneLapCount { get; set; }
    [field: SerializeField] public Label PlayerOneLapTimer { get; set; }
    // Player Two HUD Elements
    [field: SerializeField] public VisualElement PlayerTwoHUD { get; set; }
    [field: SerializeField] public VisualElement PlayerTwoHUDBackground { get; set; }
    [field: SerializeField] public Label PlayerTwoName { get; set; }
    [field: SerializeField] public Label PlayerTwoPosition { get; set; }
    [field: SerializeField] public Label PlayerTwoPositionOrdinal { get; set; }
    [field: SerializeField] public Label PlayerTwoLapCount { get; set; }
    [field: SerializeField] public Label PlayerTwoLapTimer { get; set; }

    private int _playerOneCurrentPosition = 1;
    private int _playerOneCurrentLapCount = 0;
    private float _playerOneCurrentLapTime = 0f;

    private int _playerTwoCurrentPosition = 2;
    private int _playerTwoCurrentLapCount = 0;
    private float _playerTwoCurrentLapTime = 0f;

    private int _totalLapCount = 0;
    private Medal _currentMedal = Medal.None;
    private List<float> _currentTrackMedalTimes = new();
    private bool _isTimerRunning = false;

    protected override void Awake()
    {
        base.Awake();

        Root = GetComponent<UIDocument>().rootVisualElement;
        HUDElement = Root.Q<VisualElement>("HUD");
        HUDElement.AddToClassList("hideUI");

        TimerElement = HUDElement.Q<VisualElement>("TimerElement");
        CentralTimer = TimerElement.Q<Label>("Timer");
        MedalText = TimerElement.Q<Label>("MedalText");
        CountdownPopup = HUDElement.Q<VisualElement>("CountdownPopup");
        CountdownValue = CountdownPopup.Q<Label>("CountdownValue");

        PlayerOneHUD = HUDElement.Q<VisualElement>("PlayerOneHUD");
        PlayerOneHUDBackground = HUDElement.Q<VisualElement>("PlayerOneBackground");
        PlayerOneName = PlayerOneHUD.Q<Label>("PlayerOne");
        PlayerOnePosition = PlayerOneHUD.Q<Label>("PlayerOnePosition");
        PlayerOnePositionOrdinal = PlayerOneHUD.Q<Label>("PlayerOnePositionOrdinal");
        PlayerOneLapCount = PlayerOneHUD.Q<Label>("PlayerOneLapCount");
        PlayerOneLapTimer = PlayerOneHUD.Q<Label>("PlayerOneLapTimer");

        PlayerTwoHUD = HUDElement.Q<VisualElement>("PlayerTwoHUD");
        PlayerTwoHUDBackground = HUDElement.Q<VisualElement>("PlayerTwoBackground");
        PlayerTwoName = PlayerTwoHUD.Q<Label>("PlayerTwo");
        PlayerTwoPosition = PlayerTwoHUD.Q<Label>("PlayerTwoPosition");
        PlayerTwoPositionOrdinal = PlayerTwoHUD.Q<Label>("PlayerTwoPositionOrdinal");
        PlayerTwoLapCount = PlayerTwoHUD.Q<Label>("PlayerTwoLapCount");
        PlayerTwoLapTimer = PlayerTwoHUD.Q<Label>("PlayerTwoLapTimer");
    }

    public async Task SetupHUD(TrackContext trackContext, List<float> medalTimes)
    {
        _totalLapCount = trackContext.LapCount;
        UpdatePlayerLapCount(1, 0);
        UpdatePlayerLapCount(2, 0);
        UpdatePlayerPositions(1);
        SetupTimers(trackContext, medalTimes);

        HUDElement.RemoveFromClassList("hideUI");
        await Task.CompletedTask;
    }

    #region Player Position Management
    public void UpdatePlayerPositions(int playerOnePosition)
    {
        _playerOneCurrentPosition = playerOnePosition;
        _playerTwoCurrentPosition = playerOnePosition == 1 ? 2 : 1;
        SetPlayerPosition(1, _playerOneCurrentPosition);
        SetPlayerPosition(2, _playerTwoCurrentPosition);
    }

    private void SetPlayerPosition(int playerNumber, int position)
    {
        if(playerNumber == 1)
        {
            PlayerOnePosition.text = position.ToString();
            PlayerOnePositionOrdinal.text = GetOrdinal(position);
        }
        else
        {
            PlayerTwoPosition.text = position.ToString();
            PlayerTwoPositionOrdinal.text = GetOrdinal(position);
        }
    }

    private string GetOrdinal(int position)
    {
        return position switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th",
        };
    }
    #endregion

    #region Lap Count Management

    public void UpdatePlayerLapCount(int playerNumber, int lapCount)
    {
        if(playerNumber == 1)
        {
            _playerOneCurrentLapCount = lapCount;
            PlayerOneLapCount.text = $"{_playerOneCurrentLapCount}/{_totalLapCount}";
        }
        else
        {
            _playerTwoCurrentLapCount = lapCount;
            PlayerTwoLapCount.text = $"{_playerTwoCurrentLapCount}/{_totalLapCount}";
        }
    }

    #endregion

    #region Timer Management

    private void SetupTimers(TrackContext trackContext, List<float> medalTimes)
    {
        _currentTrackMedalTimes = medalTimes;
        if (trackContext.GameMode == GameMode.Race)
        {
            TimerElement.AddToClassList("hideUI");
            PlayerOneLapTimer.AddToClassList("hideUI");
            PlayerTwoLapTimer.AddToClassList("hideUI");
        }
        else
        {
            UpdatePlayerLapTimer(1, 0f);
            PlayerOneLapTimer.RemoveFromClassList("hideUI");

            if (trackContext.PlayerCount == 1)
            {
                UpdateCurrentMedal(Medal.Gold, _currentTrackMedalTimes[0]);
                UpdateTimer(0);
            }
            else
            {
                PlayerTwoLapTimer.RemoveFromClassList("hideUI");
                UpdatePlayerLapTimer(2, 0f);
                SetTimeToBeat(0, null);
            }
            TimerElement.RemoveFromClassList("hideUI");
        }
    }

    public void UpdateCurrentMedal(Medal medal, float medalTime)
    {
        _currentMedal = medal;
        SetMedalText(medal, medalTime);
    }

    private void SetMedalText(Medal medal, float medalTime, string playerName = null)
    {
        switch (medal) 
        {
            case Medal.Bronze:
                MedalText.text = $"Bronze: {medalTime}";
                MedalText.style.color = new Color(0.804f, 0.498f, 0.196f); // Bronze color
                break;
            case Medal.Silver:
                MedalText.text = $"Silver: {medalTime}";
                MedalText.style.color = new Color(0.753f, 0.753f, 0.753f); // Silver color
                break;
            case Medal.Gold:
                MedalText.text = $"Gold: {medalTime}";
                MedalText.style.color = new Color(1f, 0.843f, 0f); // Gold color
                break;
            default:
                MedalText.text = $"Best Lap Time: {playerName}";
                MedalText.style.color = Color.white;
                break;
        }
    }

    public void UpdateTimer(float timer)
    {
        CentralTimer.text = FormatTime(timer);
    }

    public void SetTimeToBeat(float timeToBeat, string playerName)
    {
        SetMedalText(Medal.None, timeToBeat, playerName);
        UpdateTimer(timeToBeat);
    }

    public void UpdatePlayerLapTimer(int playerNumber, float lapTime)
    {
        if (playerNumber == 1)
        {
            _playerOneCurrentLapTime = lapTime;
            PlayerOneLapTimer.text = FormatTime(lapTime);
        }
        else
        {
            _playerTwoCurrentLapTime = lapTime;
            PlayerTwoLapTimer.text = FormatTime(lapTime);
        }
    }

    private string FormatTime(float lapTime)
    {
        string formattedTime = TimeSpan.FromSeconds(lapTime).ToString(@"mm\:ss\.fff");

        return formattedTime;
    }

    public async Task BeginCountdown(float duration)
    {
        await Task.Delay(250);
        CountdownValue.text = duration.ToString();
        await ShowCountdownPopup();

        await PerformCountdown(duration);

        await Task.Delay(200);

        await HideCountdownPopup();
    }

    public async Task ShowCountdownPopup()
    {
        CountdownPopup.style.display = DisplayStyle.Flex;
        await Task.Yield();
        CountdownPopup.RemoveFromClassList("hideUI");

        await Task.Delay(200);
    }

    private async Task PerformCountdown(float duration)
    {
        int secondsRemaining = Mathf.CeilToInt(duration);

        while (secondsRemaining > 0)
        {
            CountdownValue.text = secondsRemaining.ToString();

            await Task.Delay(1000);

            secondsRemaining--;
        }

        CountdownValue.text = "GO!";
        GameManager.Instance.StartRace();
    }

    public async Task HideCountdownPopup()
    {
        CountdownPopup.AddToClassList("hideUI");
        await Task.Delay(200);
        CountdownPopup.style.display = DisplayStyle.None;
    }

    #endregion
}
