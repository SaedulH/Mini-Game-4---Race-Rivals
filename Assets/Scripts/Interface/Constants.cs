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
        public const float STEERING_ANIM_LERP_SPEED = 10f;
        public const float EMISSION_MOVE_TOWARDS_RATE = 10f;
        public const float IDLE_EXHAUST_RATE = 1f;
        public const float IDLE_DRIFT_RATE = 0f;

        public const float COLLISION_EFFECT_COOLDOWN_TIME = 0.3f;
        public const float COLLISION_VOLUME_COEFFICIENT = 0.8f;
        public const float COLLISION_DURATION_COEFFICIENT = 0.5f;
        public const float COLLISION_INTENSITY_COEFFICIENT = 0.8f;

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

    public enum PresetVehicle : int
    {
        AllRounder = 0,
        Drifter = 1,
        Muscle = 2,
        Racer = 3
    }
}