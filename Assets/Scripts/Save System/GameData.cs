namespace SaveSystem
{
    [System.Serializable]
    public sealed class GameData
    {
        public float MasterVolume;
        public int DifficultyLevel;

        public GameData()
        {
            MasterVolume = 0.0f;
            DifficultyLevel = 0;
        }
    }
}