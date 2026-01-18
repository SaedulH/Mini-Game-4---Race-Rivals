using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreSystem;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class GameManager : NonPersistentSingleton<GameManager>
{

    [field: SerializeField] public GameState CurrentGameState { get; set; }
    [field: SerializeField] public TrackInfo MenuInfo { get; set; }
    [field: SerializeField] public TrackInfo CurrentTrackInfo { get; set; }
    [field: SerializeField] public TrackContext CurrentTrackContext { get; set; }
    [field: SerializeField] public GameObject PlayerOne { get; set; }
    [field: SerializeField] public GameObject PlayerTwo { get; set; }
    [field: SerializeField] public PlayerInput Input { get; set; }

    [field: SerializeField] public GameObject PlayerPrefab { get; set; }
    [field: SerializeField] public GameObject AIPrefab { get; set; }

    private InputAction _pauseAction;
    public event Action<GameState> OnGameStateChanged;

    private bool _playerOneCompletedRace = false;
    private bool _playerTwoCompletedRace = false;

    protected override void Awake()
    {
        base.Awake();
        Input = GetComponent<PlayerInput>();
        EnterGameState(GameState.Menu);
    }

    private void OnEnable()
    {
        _pauseAction = Input.actions["Pause"];
        _pauseAction.performed += OnPauseInput;
    }

    private void OnDisable()
    {
        _pauseAction.performed -= OnPauseInput;
    }

    public void EnterGameState(GameState newState)
    {
        Debug.Log($"Entering Game State: {newState}");
        //switch (newState)
        //{
        //    case GameState.Playing:
        //        break;
        //    case GameState.Loading:
        //        //Input.SwitchCurrentActionMap("Gameplay");
        //        break;
        //    case GameState.Menu:
        //    case GameState.Paused:
        //    case GameState.GameOver:
        //        //Input.SwitchCurrentActionMap("UI");
        //        break;
        //}
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public async Task ConfigurePlayer(int playerIndex, Vehicle vehicle)
    {
        GameObject playerPrefab = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        InputHandler input = playerPrefab.GetOrAdd<InputHandler>();
        VehicleSpriteHandler spriteHandler = playerPrefab.GetOrAdd<VehicleSpriteHandler>();
        Movement movement = playerPrefab.GetOrAdd<Movement>();
        movement.AssignVehicleStats(vehicle.Stats);
        if (playerIndex == 1)
        {
            input.AssignInput("PlayerOne");
            spriteHandler.AssignSprite(vehicle.PlayerOneIcon);
            PlayerOne = playerPrefab;
            PlayerOne.name = "PlayerOne";
            AddToCameraTargetGroup(1, PlayerOne);
            _playerOneCompletedRace = false;
        }
        else
        {
            input.AssignInput("PlayerTwo");
            spriteHandler.AssignSprite(vehicle.PlayerTwoIcon);
            PlayerTwo = playerPrefab;
            PlayerTwo.name = "PlayerTwo";
            AddToCameraTargetGroup(2, PlayerTwo);
            _playerTwoCompletedRace = false;
        }

        await Task.CompletedTask;
    }

    public async Task ConfigureAI(Vehicle vehicle)
    {
        PlayerTwo = Instantiate(AIPrefab, Vector3.zero, Quaternion.identity);
        PlayerTwo.name = "CPU";
        AIHandler input = PlayerTwo.GetOrAdd<AIHandler>();
        //input.AssignInput("PlayerTwo");

        VehicleSpriteHandler spriteHandler = PlayerTwo.GetOrAdd<VehicleSpriteHandler>();
        spriteHandler.AssignSprite(vehicle.PlayerTwoIcon);

        Movement movement = PlayerTwo.GetOrAdd<Movement>();
        movement.AssignVehicleStats(vehicle.Stats);

        AddToCameraTargetGroup(2, PlayerTwo);
        _playerTwoCompletedRace = false;
        await Task.CompletedTask;
    }

    private void AddToCameraTargetGroup(int playerIndex, GameObject playerObject)
    {

        return;
        
        CinemachineTargetGroup targetGroup = GameObject.FindGameObjectWithTag("Target").GetComponent<CinemachineTargetGroup>();
        CinemachineTargetGroup.Target target = new();
        target.Object = playerObject.transform;
        target.Radius = 30f;
        target.Weight = 1f;
        Debug.Log("target found");
        targetGroup.AddMember(target.Object, target.Weight, target.Radius);
    }

    public async Task InitialiseHUD(TrackContext context, List<float> medalTimes)
    {
        await HUDManager.Instance.SetupHUD(context, medalTimes);
    }

    private void OnPauseInput(InputAction.CallbackContext ctx)
    {
        switch (CurrentGameState) 
        {
            case GameState.Playing:
                PauseGame();
                break;
            case GameState.Paused:
                UnpauseGame();
                break;
        }
    }

    public void PauseGame()
    {
        EnterGameState(GameState.Paused);
    }

    public void UnpauseGame()
    {
        EnterGameState(GameState.Playing);
    }

    public void StartRace()
    {
        EnterGameState(GameState.Playing);
    }

    public void CompleteRace(int finishedPlayer)
    {
        if(finishedPlayer == 1)
        {
            _playerOneCompletedRace = true;
        }
        else if (finishedPlayer == 2)
        {
            _playerTwoCompletedRace = true;
        }

        if (CurrentTrackContext.GameMode == GameMode.Race || 
            (CurrentTrackContext.GameMode == GameMode.Timed && 
            (CurrentTrackContext.PlayerCount == 1 || 
            (CurrentTrackContext.PlayerCount == 2 && 
            _playerOneCompletedRace && _playerTwoCompletedRace))))
        {
            GetRaceResults();
        }
    }

    private void GetRaceResults()
    {
        RaceCompleteDetails results = HUDManager.instance.GetResults();
        RaceCompleteScreen.Instance.SetRaceCompleteDetails(results);
        EnterGameState(GameState.GameOver);
    }

    public void RestartLevel()
    {
        TrackContext context = new();
        _ = TrackInitialiser.Instance.InitialiseTrack(CurrentTrackInfo, CurrentTrackContext);
    }

    public void ReturnToMainMenu()
    {
        TrackContext context = new();
        _ = TrackInitialiser.Instance.InitialiseTrack(MenuInfo, context);
    }

    public async Task InitialiseScene(TrackInfo trackInfo, TrackContext trackContext)
    {
        CurrentTrackContext = trackContext;
        CurrentTrackInfo = trackInfo;

        await TrackInitialiser.Instance.InitialiseTrack(trackInfo, trackContext);
    }
}
