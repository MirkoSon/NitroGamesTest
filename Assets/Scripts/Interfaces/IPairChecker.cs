/// <summary>
/// Interface that handles the card's pair validation
/// </summary>
public interface IPairChecker
{
    void CheckIfPair(int id);

    void PairEvaluated(string json);
}
