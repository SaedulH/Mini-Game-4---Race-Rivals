using System;

[Serializable]
public class RaceProgress
{
    public int CurrentPosition;
    public int CurrentCheckpoint;
    public int CurrentLap;
    public float BestLapTime;
    public int TotalProgress;

    public RaceProgress(int currentPosition = 0, int currentCheckpoint = 0, int currentLap = 1, float bestLapTime = 0f, int totalProgress = 0)
    {
        this.CurrentPosition = currentPosition;
        this.CurrentCheckpoint = currentCheckpoint;
        this.CurrentLap = currentLap;
        this.BestLapTime = bestLapTime;
        this.TotalProgress = totalProgress; 
    }

    public void SetCurrentPosition(int posuition)
    {
        this.CurrentPosition = posuition;
    }

    public void SetCurrentCheckpoint(int checkpoint)
    {
        this.CurrentCheckpoint = checkpoint;
    }

    public void SetNextLap()
    {
        this.CurrentLap++;
    }

    public void SetBestLapTime(float lapTime)
    {
        this.BestLapTime = lapTime;
    }

    public void IncrementTotalProgress()
    {
        this.TotalProgress++;
    }
}
