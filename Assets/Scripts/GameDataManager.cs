using System;
using UnityEngine;

/// <summary>
/// This class stores the starting configuration of the game and manages any change to its values.
/// </summary>
public class GameDataManager
{
    [HideInInspector]
    public Configuration configuration;

    public Action OnConfigurationReady;
    public Action OnValuesChanged;
    public Action OnGameOver;

    public int currentLives;
    public int currentPairs;
    public int currentPowerUps;

    private const string _getUrl = "http://localhost:3000/configuration/1";

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        dependencyContainer.Resolve<Main>().
        StartCoroutine(dependencyContainer.Resolve<ServerCommunications>().GetRequest(_getUrl));
    }

    public void SetConfiguration(string json)
    {
        configuration = JsonUtility.FromJson<Configuration>(json);
        SetValuesFromConfiguration();
        OnConfigurationReady?.Invoke();
    }

    void SetValuesFromConfiguration()
    {
        currentPairs = configuration.cardIds.Length / 2;
        currentPowerUps = configuration.powerupsCount;
        currentLives = configuration.livesCount;
    }

    public void UsePowerUp()
    {
        currentPowerUps--;
    }

    public void UpdateValues(bool isCorrect, int id)
    {
        if (isCorrect)
        {
            currentPairs--;
        }
        else
        {
            currentLives--;
        }

        OnValuesChanged?.Invoke();

        if (currentLives == 0 || currentPairs == 0)
            OnGameOver?.Invoke();
    }
}
