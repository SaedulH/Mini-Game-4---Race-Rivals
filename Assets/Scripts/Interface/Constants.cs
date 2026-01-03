namespace Utilities
{
    public static class Constants
    {
        public enum GameState
        {
            Menu,
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
            Bronze,
            Silver,
            Gold
        }
    }
}