using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreSystem;
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

    private List<float> _currentTrackMedalTimes = new();
    public event Action<GameState> OnGameStateChanged;
    private InputAction _pauseAction;

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
        switch (newState)
        {
            case GameState.Playing:
            case GameState.Loading:
                //Input.SwitchCurrentActionMap("Gameplay");
                //Time.timeScale = 1f;
                break;
            case GameState.Menu:
            case GameState.Paused:
            case GameState.GameOver:
                //Input.SwitchCurrentActionMap("UI");
                //Time.timeScale = 0f;
                break;
        }
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public async Task ConfigurePlayer(int playerIndex, int vehicleIndex)
    {
        GameObject playerPrefab = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        InputHandler input = playerPrefab.GetOrAdd<InputHandler>();
        if (playerIndex == 1)
        {
            input.AssignInput("PlayerOne");
            PlayerOne = playerPrefab;
            PlayerOne.name = "PlayerOne";
        }
        else
        {
            input.AssignInput("PlayerTwo");
            PlayerTwo = playerPrefab;
            PlayerTwo.name = "PlayerTwo";
        }
        //playerManager.addTargets(vehicleIndex, playerObject);
        await Task.CompletedTask;
    }

    public async Task ConfigureAI(int vehicleIndex)
    {
        PlayerTwo = Instantiate(AIPrefab, Vector3.zero, Quaternion.identity);
        AIHandler input = PlayerTwo.GetOrAdd<AIHandler>();
        PlayerTwo.name = "PlayerAI";
        //input.AssignInput("PlayerTwo");

        //playerManager.addTargets(vehicleIndex, playerObject);
        await Task.CompletedTask;
    }

    public async Task InitialiseHUD(TrackContext context, List<float> medalTimes)
    {
        _currentTrackMedalTimes = medalTimes;
        await HUDManager.Instance.SetupHUD(context, medalTimes);
    }

    //IEnumerator GetRaceType()
    //{
    //    //isRaceStart = false;
    //    //yield return new WaitForSeconds(3);
    //    //isRaceStart = true;
    //    //if (playerCount == 2)
    //    //{  
    //    //    raceType = VERSUS;
    //    //}
    //    //else
    //    //{
    //    //    raceType = TIME_ATTACK;
    //    //}   
    //}

    public void GetRaceResults()
    {

    }

    public void TimeAttack()
    {
        //timeToBeat -= Time.deltaTime;
        //timer.text = timeToBeat.ToString("F2");

        //if(timeToBeat <= 0)
        //{
        //    Debug.Log("YOU LOSE!");
        //    isRaceComplete = true;

        //}
    }

    private void OnPauseInput(InputAction.CallbackContext ctx)
    {
        if(CurrentGameState == GameState.Playing)
        {
            PauseGame();
        }
        else if(CurrentGameState == GameState.Paused)
        {
            UnpauseGame();
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
        //EnterGameState(GameState.Playing);
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
