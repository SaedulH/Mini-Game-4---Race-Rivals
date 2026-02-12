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

        public const float MAX_IDLE_SPEED = 0.5f;
        public const float MIN_THROTTLE = 0.1f;
        public const float MIN_SPEED_FOR_TURN = 5f;
        public const float STEERING_RIGHT_ANGLE = 90f;
        public const float STEERING_LEFT_ANGLE = -90f;
        public const float MAX_TURN_SPEED_LOSS_PERCENT = 0.2f;
        public const float MIN_SPEED_FOR_BRAKE_EFFECTS = 10f;
        public const float MIN_SPEED_FOR_DRIFT_EFFECTS = 20f;
        public const float MIN_ANGULAR_VEL_FOR_DRIFT_EFFECTS = 60f;

        public const float IDLE_EXHAUST_RATE = 1f;
        public const float LOW_EXHAUST_RANGE = 0.4f;
        public const float LOW_EXHAUST_RATE = 5f;
        public const float HIGH_EXHAUST_RANGE = 0.6f;
        public const float HIGH_EXHAUST_RATE = 10f;

        public const float IDLE_DRIFT_RATE = 0f;
        public const float LOW_DRIFT_RANGE = 0.2f;
        public const float LOW_DRIFT_RATE = 5f;
        public const float HIGH_DRIFT_RANGE = 0.6f;
        public const float HIGH_DRIFT_RATE = 10f;

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

    public enum VehicleState
    {
        Idle,
        Accelerating,
        Decelerating,
        Braking,
        Reversing
    }

    public enum EffectRate
    { 
        None,
        Low,
        High
    }
}