using System;
using System.Drawing;

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
        public const float COLLISION_VOLUME_COEFFICIENT = 0.5f;
        public const float COLLISION_DURATION_COEFFICIENT = 0.5f;
        public const float COLLISION_INTENSITY_COEFFICIENT = 0.8f;

        public const float ROAD_TERRAIN_FACTOR = 0f;
        public const float GRASS_TERRAIN_FACTOR = 0.6f;
        public const float DIRT_TERRAIN_FACTOR = 0.8f;
        public const float GRAVEL_TERRAIN_FACTOR = 1f;
        public const float OFFROAD_VOLUME_COEFFICIENT = 0.75f;

        public const string MASTER_AUDIO_MIXER = "Master";
        public const string MUSIC_AUDIO_MIXER = "Music";
        public const string UI_AUDIO_MIXER = "UI";
        public const string EFFECTS_AUDIO_MIXER = "Effects";

        public const float AUDIO_MUSIC_FADE_IN_TIME = 0.5f;
        public const float AUDIO_EFFECTS_FADE_IN_TIME = 0.25f;
        public const float AUDIO_EFFECTS_FADE_OUT_TIME = 0.5f;

        public const float DYNAMIC_VOLUME_LERP_SPEED = 5f;
        public const float DYNAMIC_PITCH_LERP_SPEED = 5f;
        public const float THROTTLE_LERP_SPEED = 5f;
        public const float RPM_LERP_SPEED = 0.15f;
        public const float RPM_STEERING_FACTOR_THRESHOLD = 0.65f;

        public const float ACCEL_LOW_VOLUME_COEFFICIENT = 0.7f;
        public const float ACCEL_HIGH_VOLUME_COEFFICIENT = 0.5f;
        public const float DECEL_LOW_VOLUME_COEFFICIENT = 0.8f;
        public const float DECEL_HIGH_VOLUME_COEFFICIENT = 0.6f;

        public const float ACCEL_LOW_PITCH_COEFFICIENT = 0.8f;
        public const float ACCEL_HIGH_PITCH_COEFFICIENT = 0.8f;
        public const float DECEL_LOW_PITCH_COEFFICIENT = 0.8f;
        public const float DECEL_HIGH_PITCH_COEFFICIENT = 0.6f;

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
        Vehicle,
        Settings
    }

    public enum SettingScreenType
    {
        Game,
        Audio,
        Controls,
        InputPopup
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

    public enum TerrainType : int
    {
        Road = 0,
        Grass = 1,
        Dirt = 2,
        Gravel = 3,
    }

    public enum CameraMode
    {
        Fixed,
        Dynamic
    }

    public enum ScreenShake
    {
        Off,
        Low,
        High
    }

    public enum Difficulty
    {
        Easy,
        Hard
    }

    public enum ControlInput
    {
        Throttle,
        Reverse,
        Left,
        Right,
        Handbrake
    }
}