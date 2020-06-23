/// <summary>
/// Structs used for server communications and scores serialization.
/// </summary>
[System.Serializable]
public struct Configuration
{
    public int cardScore;
    public int multiplier;
    public int livesCount;
    public int powerupsCount;
    public int[] cardIds;
    public int rowSize;
}

[System.Serializable]
public struct Request
{
    public int cardOneId;
    public int cardTwoId;
}

[System.Serializable]
public struct Response
{
    public bool isCorrect;
}

[System.Serializable]
public struct SavedScores
{
    public System.Collections.Generic.List<int> scores;
}

