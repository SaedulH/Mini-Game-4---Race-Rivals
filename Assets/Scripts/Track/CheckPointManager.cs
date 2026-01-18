using System.Threading.Tasks;
using CoreSystem;
using UnityEngine;
using Utilities;

public class CheckPointManager : NonPersistentSingleton<CheckPointManager>
{
    [field: SerializeField] public CheckpointScript[] Checkpoints { get; set; }
    [field: SerializeField] public RaceProgress PlayerOneProgress { get; set; }
    [field: SerializeField] public RaceProgress PlayerTwoProgress { get; set; }

    private int _maxLaps = 0;
    private GameMode _gameMode;

    private void OnValidate()
    {
        //AssignCheckpoints();
    }

    public void FixedUpdate()
    {
        if(_gameMode != GameMode.Race)
        {
            return;
        }

        //Calculate overall progress and distance to next checkpoint for each player
        //Set player position based on furthest complete
    }

    public async Task SetupCheckpoints(TrackContext context)
    {
        PlayerOneProgress = new RaceProgress(0, 1, 0f);
        PlayerTwoProgress = new RaceProgress(0, 1, 0f);
        _maxLaps = context.LapCount;
        _gameMode = context.GameMode;

        await AssignCheckpoints();
    }

    private async Task AssignCheckpoints()
    {
        Checkpoints = gameObject.GetComponentsInChildren<CheckpointScript>();
        foreach (CheckpointScript checkpoint in Checkpoints)
        {
            checkpoint.ResetCheckpoint();
        }
        await Task.Yield();
    }

    public void CheckpointReached(int checkpointNumber, int playerNumber)
    {
        if (playerNumber == 1)
        {
            if (checkpointNumber == PlayerOneProgress.CurrentCheckpoint + 1)
            {
                PlayerOneProgress.SetCurrentCheckpoint(checkpointNumber);
                UpdateLapNumber(playerNumber, PlayerOneProgress);
            }
            else
            {
                Debug.Log($"Player 1 missed a checkpoint! Current: {PlayerOneProgress.CurrentCheckpoint}, Triggered: {checkpointNumber}");
            }
        }
        else if (playerNumber == 2)
        {
            if (checkpointNumber == PlayerTwoProgress.CurrentCheckpoint + 1)
            {
                PlayerTwoProgress.SetCurrentCheckpoint(checkpointNumber);
                UpdateLapNumber(playerNumber, PlayerTwoProgress);
            }
            else
            {
                Debug.Log($"Player 2 missed a checkpoint! Current: {PlayerTwoProgress.CurrentCheckpoint}, Triggered: {checkpointNumber}");
            }
        }
    }

    private void UpdateLapNumber(int playerNumber, RaceProgress playerProgress)
    {
        if (playerProgress.CurrentCheckpoint == Checkpoints.Length)
        {
            float lapTime = HUDManager.instance.UpdatePlayerLapCount(playerNumber, playerProgress.CurrentLap);
            if (lapTime < playerProgress.BestLapTime || playerProgress.BestLapTime == 0f)
            {
                playerProgress.SetBestLapTime(lapTime);
            }

            if (playerProgress.CurrentLap < _maxLaps)
            {
                playerProgress.SetNextLap();
                playerProgress.SetCurrentCheckpoint(0);
            }
            else
            {
                Debug.Log($"Player {playerNumber} Finished!!");
                GameManager.Instance.CompleteRace(playerNumber);
            }
        }
    }
}
