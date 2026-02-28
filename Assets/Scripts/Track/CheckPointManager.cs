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
    public GameObject _playerOneGO;
    public GameObject _playerTwoGO;

    private void OnValidate()
    {
        //AssignCheckpoints();
    }

    public void FixedUpdate()
    {
        if (_gameMode != GameMode.Race || _playerOneGO == null || _playerTwoGO == null)
        {
            return;
        }

        if (PlayerOneProgress.CurrentCheckpoint >= Checkpoints.Length)
        {
            return;
        }

        Vector3 nextCheckpointPos = Checkpoints[PlayerOneProgress.CurrentCheckpoint].transform.position;
        if (PlayerOneProgress.TotalProgress == PlayerTwoProgress.TotalProgress)
        {
            float playerOneDistanceToCheckpoint = Vector3.Distance(_playerOneGO.transform.position, nextCheckpointPos);
            float playerTwoDistanceToCheckpoint = Vector3.Distance(_playerTwoGO.transform.position, nextCheckpointPos);
            if (playerOneDistanceToCheckpoint < playerTwoDistanceToCheckpoint && PlayerOneProgress.CurrentPosition != 1)
            {
                PlayerOneProgress.SetCurrentPosition(1);
                PlayerTwoProgress.SetCurrentPosition(2);
                HUDManager.instance.UpdatePlayerPositions(1);
            }
            else if (playerTwoDistanceToCheckpoint < playerOneDistanceToCheckpoint && PlayerTwoProgress.CurrentPosition != 1)
            {
                PlayerOneProgress.SetCurrentPosition(2);
                PlayerTwoProgress.SetCurrentPosition(1);
                HUDManager.instance.UpdatePlayerPositions(2);
            }
        }
        else if ((PlayerOneProgress.TotalProgress > PlayerTwoProgress.TotalProgress) && PlayerOneProgress.CurrentPosition != 1)
        {
            PlayerOneProgress.SetCurrentPosition(1);
            PlayerTwoProgress.SetCurrentPosition(2);
            HUDManager.instance.UpdatePlayerPositions(1);
        }
        else if ((PlayerTwoProgress.TotalProgress > PlayerOneProgress.TotalProgress) && PlayerTwoProgress.CurrentPosition != 1)
        {
            PlayerOneProgress.SetCurrentPosition(1);
            PlayerTwoProgress.SetCurrentPosition(2);
            HUDManager.instance.UpdatePlayerPositions(2);
        }
        //Calculate overall progress and distance to next checkpoint for each player
        //Set player position based on furthest complete
    }

    public async Task SetupCheckpoints(TrackContext context)
    {
        PlayerOneProgress = new RaceProgress(1, 0, 1, 0f);
        PlayerTwoProgress = new RaceProgress(2, 0, 1, 0f);
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

    public void AddPlayerToCheckpointTarget(int playerIndex, GameObject playerObject)
    {
        if (playerIndex == 1)
        {
            _playerOneGO = playerObject;
        }
        else if (playerIndex == 2)
        {
            _playerTwoGO = playerObject;
        }
    }

    public void CheckpointReached(int checkpointNumber, int playerNumber)
    {
        if (playerNumber == 1)
        {
            if (checkpointNumber == PlayerOneProgress.CurrentCheckpoint + 1)
            {
                PlayerOneProgress.SetCurrentCheckpoint(checkpointNumber);
                UpdateProgress(playerNumber, PlayerOneProgress);

                PlayerOneProgress.IncrementTotalProgress();
            }
            else
            {
               // Debug.Log($"Player 1 missed a checkpoint! Current: {PlayerOneProgress.CurrentCheckpoint}, Triggered: {checkpointNumber}");
            }
        }
        else if (playerNumber == 2)
        {
            if (checkpointNumber == PlayerTwoProgress.CurrentCheckpoint + 1)
            {
                PlayerTwoProgress.SetCurrentCheckpoint(checkpointNumber);
                UpdateProgress(playerNumber, PlayerTwoProgress);

                PlayerTwoProgress.IncrementTotalProgress();
            }
            else
            {
               // Debug.Log($"Player 2 missed a checkpoint! Current: {PlayerTwoProgress.CurrentCheckpoint}, Triggered: {checkpointNumber}");
            }
        }
    }

    private void UpdateProgress(int playerNumber, RaceProgress playerProgress)
    {
        if (playerProgress.CurrentCheckpoint == Checkpoints.Length)
        {
            float lapTime = HUDManager.instance.UpdatePlayerLapCount(playerNumber, playerProgress.CurrentLap);
            if (lapTime < playerProgress.BestLapTime || playerProgress.BestLapTime == 0f)
            {
                playerProgress.SetBestLapTime(lapTime);
                HUDManager.instance.UpdateBestLapTime(playerNumber, lapTime);
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
