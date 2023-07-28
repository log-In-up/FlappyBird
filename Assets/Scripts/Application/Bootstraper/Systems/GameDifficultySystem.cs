using DifficultySystem;

public class GameDifficultySystem
{
    #region Fields
    private DifficultyData _currentDifficulty = null;
    private readonly DifficultyDataLoader _difficultyDataLoader = null;
    #endregion

    public GameDifficultySystem(DifficultyDataLoader difficultyDataLoader)
    {
        _difficultyDataLoader = difficultyDataLoader;
    }

    #region Public API
    public void SetDifficulty(Difficulties difficulties)
    {
        _currentDifficulty = _difficultyDataLoader.GetDificulty(difficulties);
    }

    public string GetDifficultyName(Difficulties difficulties)
    {
        return _difficultyDataLoader.GetDificulty(difficulties).DifficultyName.ToString();
    }

    public DifficultyData GetDifficultyData() => _currentDifficulty;
    #endregion
}
