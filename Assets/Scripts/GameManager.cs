using CoreSystem;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class GameManager : NonPersistentSingleton<GameManager>
{

    [field: SerializeField] public GameState CurrentGameState { get; set; }
    [field: SerializeField] public TrackInfo MenuInfo { get; set; }
    [field: SerializeField] public TrackInfo CurrentTrackInfo { get; set; }
    [field: SerializeField] public GameObject PlayerOne { get; set; }
    [field: SerializeField] public GameObject PlayerTwo { get; set; }

    [field: SerializeField] public GameObject PlayerPrefab { get; set; }
    [field: SerializeField] public GameObject AIPrefab { get; set; }

    private List<float> _currentTrackMedalTimes = new();

    protected override void Awake()
    {
        base.Awake();
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

    public void EnterGameState(GameState newState)
    {
        Debug.Log($"Entering Game State: {newState}");
        switch (newState)
        {
            case GameState.Loading:
                break;
            case GameState.Playing:
                // Handle Playing State
                break;
            case GameState.Paused:
                // Handle Paused State
                break;
            case GameState.GameOver:
                // Handle GameOver State
                break;
        }
        CurrentGameState = newState;
    }

    public void PauseGame()
    {
        EnterGameState(GameState.Paused);
    }

    public void UnpauseGame()
    {
        EnterGameState(GameState.Playing);
    }

    public void RestartLevel()
    {
        TrackContext context = new();
        _ = TrackInitialiser.Instance.InitialiseTrack(CurrentTrackInfo, context);
    }

    public void ReturnToMainMenu()
    {
        TrackContext context = new();
        _ = TrackInitialiser.Instance.InitialiseTrack(MenuInfo, context);
    }
}
