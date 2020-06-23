using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Service class that manages adding, saving and loading the Top 5 scores. (Possibly also deleting them from memory)
/// </summary>
public class ScoreboardManager : IScoreboardManager
{
    private List<int> _scores;
    private IScoreCalculator _scoreCalculator;
    private GameDataManager _gameData;

    protected readonly ISerializer _serializer;

    public ScoreboardManager(GameDataManager gameData, IScoreCalculator scoreCalculator, ISerializer serializer)
    {
        _gameData = gameData;
        _serializer = serializer;
        _scoreCalculator = scoreCalculator;
        Load();
    }

    public IEnumerator AddScore()
    {
        int previousScore = _scoreCalculator.CurrentScore;

        if (_gameData.currentLives != 0)
            yield return new WaitUntil(() => _scoreCalculator.CurrentScore != previousScore);

        int newScore = _scoreCalculator.CurrentScore;

        if (_scores.Count < 5)
        {
            _scores.Add(newScore);
        }
        else
        {
            int minScore = _scores.Min(t => t);
            if (newScore > minScore)
            {
                _scores[4] = newScore;
            }
        }
        _scores.Sort();
        _scores.Reverse();
        Save();
    }

    public void Load()
    {
        SavedScores savedScores = new SavedScores();
        _scores = new List<int>();
        if (_serializer.Deserialize(ref savedScores))
        {
            _scores = savedScores.scores;
        }
    }

    public void Save()
    {
        SavedScores savedScores = new SavedScores()
        {
            scores = _scores
        };
        _serializer.Serialize(savedScores);
    }

    public void Delete()
    {
        _serializer.Delete();
    }
}
