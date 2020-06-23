using System;
/// <summary>
/// Helper class to calculate score and multipliers
/// </summary>
public class ScoreCalculator : IScoreCalculator
{
    public int cardValue;
    public int multiplierValue;

    public Action OnValuesChanged;

    private IDependencyContainer _dependencyContainer;
    private int _currentScore;
    private int _currentMultiplier;

    public int CurrentScore
    {
        get
        {
            return _currentScore;
        }
    }

    public int CurrentMultiplier
    {
        get
        {
            return _currentMultiplier;
        }
    }

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _dependencyContainer = dependencyContainer;
    }

    public void SetValues()
    {
        GameDataManager gameData = _dependencyContainer.Resolve<GameDataManager>();
        cardValue = gameData.configuration.cardScore;
        multiplierValue = gameData.configuration.multiplier;
    }

    public int IncreaseScore()
    {
        int increment;

        increment = cardValue * 2 * Math.Max(1,_currentMultiplier);
        return increment;
    }

    public void UpdateScore(bool isCorrect, int id)
    {
        if (isCorrect)
        {
            _currentScore += IncreaseScore();
            _currentMultiplier += multiplierValue;
        }
        else
        {
            _currentMultiplier = 0;
        }

        OnValuesChanged?.Invoke();
    }
}
