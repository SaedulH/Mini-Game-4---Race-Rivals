using Utilities;

public struct RaceCompleteDetails
{
    public int PlayerCount;
    public GameMode GameMode;
    public string WinningPlayer;
    public string WinningTime;
    public Medal AwardedMedal;

    public RaceCompleteDetails(int playerCount, GameMode gameMode, string winningPlayer, string winningTime = null, Medal awardedMedal = Medal.None)
    {
        PlayerCount = playerCount;
        GameMode = gameMode;
        WinningPlayer = winningPlayer;
        WinningTime = winningTime;
        AwardedMedal = awardedMedal;
    }
}