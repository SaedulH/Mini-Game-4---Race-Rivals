using System;

namespace Utilities
{
    public static class Constants
    {
        public const int DEFAULT_LAP_COUNT = 3;
        public const int MIN_LAP_COUNT = 1;
        public const int MAX_LAP_COUNT = 10;

        public const string SOLO_RACE_WIN = "YOU WIN!";
        public const string SOLO_RACE_LOSE = "YOU LOSE!";
        public const string SOLO_TIMED_WIN = "AWARD: ";
        public const string SOLO_TIMED_LOSE = "FAILED!";
        public const string VERSUS_WINNER_TEXT = "WINNER: ";
        public const string BEST_LAP_TEXT = "BEST LAP TIME: ";
        public const string RACE_FINISHED = "FINISHED";

        //public const string  = "";
        public static string FormatTime(float lapTime)
        {
            string formattedTime = TimeSpan.FromSeconds(lapTime).ToString(@"mm\:ss\.ff");

            return formattedTime;
        }
    }


    public enum GameState
    {
        Menu,
        Loading,
        Playing,
        Paused,
        GameOver
    }

    public enum MenuScreenType
    {
        Home,
        Selection,
        Vehicle
    }

    public enum GameMode
    {
        Race,
        Timed
    }

    public enum Medal
    {
        None,
        Failed,
        Bronze,
        Silver,
        Gold
    }
}