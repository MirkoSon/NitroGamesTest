using System;
using UnityEngine;

/// <summary>
/// Helper class implementation of <see cref="IPairChecker"/>
/// It communicates with the server to validate the revealed cards.
/// </summary>
public class PairChecker : IPairChecker
{ 
    public Action<bool, int> OnPairEvaluated;

    public  bool hasPowerUp;

    private int _powerUpCardId;
    private int _sameId;
    private Main _main;
    private GameDataManager _gameData;
    private IServerCommunications _serverCommunications;

    private int _previousId;
    private const string url = "http://localhost:3000/play/";

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _main = dependencyContainer.Resolve<Main>();
        _gameData = dependencyContainer.Resolve<GameDataManager>();
        _serverCommunications = dependencyContainer.Resolve<ServerCommunications>();
    }

    public void CheckIfPair(int id)
    {
        if(_previousId != 0 && hasPowerUp)
        {
            _powerUpCardId = id;
            hasPowerUp = false;
        }
        else if(_previousId != 0)
        {
            // Prepare request
            Request request = CompareThreeIds(_previousId, _powerUpCardId, id);
            // Ask the server through the Main MonoBehaviour
            _main.StartCoroutine(_serverCommunications.CheckPair(url, JsonUtility.ToJson(request)));
            // Reset
            _previousId = 0;
        }
        else
        {
            _previousId = id;
        }
    }

    // Creates two formats of Requests to accomodate the usage of a third card.
    public Request CompareThreeIds(int a, int b, int c)
    {
        if (a == b || a == c)
            _sameId = a;
        else if (b == c)
            _sameId = b;
        else
        {
            return new Request()
            {
                cardOneId = a,
                cardTwoId = b
            };
        }

        return new Request()
        {
            cardOneId = _sameId,
            cardTwoId = _sameId
        };
    }

    public void UsePowerUp()
    {
        if (_gameData.currentPowerUps > 0 && hasPowerUp == false)
        {
            _gameData.currentPowerUps--;
            hasPowerUp = true;
        }
    }

    public void PairEvaluated(string json)
    {
        Response response = JsonUtility.FromJson<Response>(json);
        OnPairEvaluated?.Invoke(response.isCorrect, _sameId);
    }
}