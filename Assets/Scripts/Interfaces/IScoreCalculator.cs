/// <summary>
/// Interface for the for score calculation helper class.
/// </summary>
public interface IScoreCalculator
{
    int CurrentScore { get; }
    int CurrentMultiplier { get; }

    int IncreaseScore();
}