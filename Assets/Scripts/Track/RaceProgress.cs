using System;

[Serializable]
public class RaceProgress
{
    public int CurrentCheckpoint;
    public int CurrentLap;
    public float BestLapTime;

    public RaceProgress(int currentCheckpoint = 0, int currentLap = 1, float bestLapTime = 0f)
    {
        this.CurrentCheckpoint = currentCheckpoint;
        this.CurrentLap = currentLap;
        this.BestLapTime = bestLapTime;
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
}
