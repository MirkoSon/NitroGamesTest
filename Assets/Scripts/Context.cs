using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Context is the core of the application: 
/// Uses Event Aggregation Pattern to have better control over each listener.
/// Every dependency of the game is binded here and made accessible through the <see cref="DependencyContainer"/>'s dictionary.
/// </summary>
public class Context : MonoBehaviour, IContext
{
    public const string fileName = "GameScores";

    // Helpers & Services
    protected ISerializer _serializer;
    protected IPairChecker _pairChecker;
    protected IScoreboardManager _scoreboardManager;
    protected IScoreCalculator _scoreCalculator;
    protected IServerCommunications _serverCommunication;

    // Data Management
    protected GameDataManager _gameData;

    // Monobehaviours
    protected CardSpawner _cardSpawner;
    protected EndGameUI _sceneHandler;
    protected HUDManager _hudManager;

    public static string saveFolder
    {
        get { return Path.Combine(Application.persistentDataPath, fileName); }
    }

    // Dependency Container for Injections
    protected IDependencyContainer _container;
    public IDependencyContainer dependencyContainer
    {
        get
        {
            if (_container == null)
            {
                _container = new DependencyContainer();
            }

            return _container;
        }
    }

    public void Bind()
    {
        InitializeServices();
        BindServices();
        RegisterEvents();
    }

    public void UnBind()
    {
        UnregisterEvents();
        UnbindServices();
    }

    public void InitializeServices()
    {
        _cardSpawner = null ?? GetComponent<CardSpawner>();

        _sceneHandler = null ?? GetComponent<EndGameUI>();

        _hudManager = null ?? GetComponent<HUDManager>();

        _gameData = null ?? new GameDataManager();

        _pairChecker = null ?? new PairChecker();

        _serverCommunication = null ?? new ServerCommunications();

        _serializer = null ?? new JsonSerializer(saveFolder);

        _scoreCalculator = null ?? new ScoreCalculator();

        _scoreboardManager = null ?? new ScoreboardManager(_gameData, _scoreCalculator, _serializer);

    }

    public void BindServices()
    {
        dependencyContainer.Bind<GameDataManager>(_gameData);
        dependencyContainer.Bind<CardSpawner>(_cardSpawner);
        dependencyContainer.Bind<EndGameUI>(_sceneHandler);
        dependencyContainer.Bind<HUDManager>(_hudManager);
        dependencyContainer.Bind<PairChecker>(_pairChecker);
        dependencyContainer.Bind<JsonSerializer>(_serializer);
        dependencyContainer.Bind<ScoreboardManager>(_scoreboardManager);
        dependencyContainer.Bind<ScoreCalculator>(_scoreCalculator);
        dependencyContainer.Bind<ServerCommunications>(_serverCommunication);
    }

    public void UnbindServices()
    {
        dependencyContainer.UnBind<GameDataManager>();
        dependencyContainer.UnBind<CardSpawner>();
        dependencyContainer.UnBind<EndGameUI>();
        dependencyContainer.UnBind<HUDManager>();
        dependencyContainer.UnBind<PairChecker>();
        dependencyContainer.UnBind<JsonSerializer>();
        dependencyContainer.UnBind<ScoreboardManager>();
        dependencyContainer.UnBind<ScoreCalculator>();
        dependencyContainer.UnBind<ServerCommunications>();
    }

    public void RegisterEvents()
    {
        dependencyContainer.Resolve<GameDataManager>().OnConfigurationReady += dependencyContainer.Resolve<CardSpawner>().SpawnCards;
        dependencyContainer.Resolve<GameDataManager>().OnConfigurationReady += dependencyContainer.Resolve<HUDManager>().InitializeUI;
        dependencyContainer.Resolve<GameDataManager>().OnConfigurationReady += dependencyContainer.Resolve<ScoreCalculator>().SetValues;
        dependencyContainer.Resolve<GameDataManager>().OnValuesChanged += dependencyContainer.Resolve<HUDManager>().UpdateLivesAndPowerUps;
        dependencyContainer.Resolve<GameDataManager>().OnGameOver += OpenScoreCoroutine;
        dependencyContainer.Resolve<GameDataManager>().OnGameOver += AddScoreCoroutine;
        dependencyContainer.Resolve<ScoreCalculator>().OnValuesChanged += dependencyContainer.Resolve<HUDManager>().UpdateScoreAndMultipliers;
        dependencyContainer.Resolve<PairChecker>().OnPairEvaluated += dependencyContainer.Resolve<GameDataManager>().UpdateValues;
        dependencyContainer.Resolve<PairChecker>().OnPairEvaluated += dependencyContainer.Resolve<ScoreCalculator>().UpdateScore;
        dependencyContainer.Resolve<HUDManager>().powerUpWidget.onClick.AddListener(dependencyContainer.Resolve<PairChecker>().UsePowerUp);
        dependencyContainer.Resolve<HUDManager>().powerUpWidget.onClick.AddListener(dependencyContainer.Resolve<HUDManager>().UpdateLivesAndPowerUps);
        dependencyContainer.Resolve<ServerCommunications>().OnServerResponse += dependencyContainer.Resolve<GameDataManager>().SetConfiguration;
        dependencyContainer.Resolve<ServerCommunications>().OnServerResponse += dependencyContainer.Resolve<EndGameUI>().UnloadLoadingScene;
        dependencyContainer.Resolve<ServerCommunications>().OnServerWait += dependencyContainer.Resolve<HUDManager>().SwitchAnimationState;
        dependencyContainer.Resolve<ServerCommunications>().OnServerCheck += dependencyContainer.Resolve<PairChecker>().PairEvaluated;
        CardBehaviour.OnCardFlipped += dependencyContainer.Resolve<PairChecker>().CheckIfPair;

    }

    public void UnregisterEvents()
    {
        dependencyContainer.Resolve<GameDataManager>().OnConfigurationReady -= dependencyContainer.Resolve<CardSpawner>().SpawnCards;
        dependencyContainer.Resolve<GameDataManager>().OnConfigurationReady -= dependencyContainer.Resolve<HUDManager>().InitializeUI;
        dependencyContainer.Resolve<GameDataManager>().OnValuesChanged -= dependencyContainer.Resolve<HUDManager>().UpdateLivesAndPowerUps;
        dependencyContainer.Resolve<GameDataManager>().OnGameOver -= OpenScoreCoroutine;
        dependencyContainer.Resolve<GameDataManager>().OnGameOver -= AddScoreCoroutine;
        dependencyContainer.Resolve<ScoreCalculator>().OnValuesChanged -= dependencyContainer.Resolve<HUDManager>().UpdateScoreAndMultipliers;
        dependencyContainer.Resolve<HUDManager>().powerUpWidget.onClick.RemoveAllListeners();
        dependencyContainer.Resolve<ServerCommunications>().OnServerResponse -= dependencyContainer.Resolve<GameDataManager>().SetConfiguration;
        dependencyContainer.Resolve<ServerCommunications>().OnServerResponse -= dependencyContainer.Resolve<EndGameUI>().UnloadLoadingScene;
        dependencyContainer.Resolve<ServerCommunications>().OnServerWait -= dependencyContainer.Resolve<HUDManager>().SwitchAnimationState;
        dependencyContainer.Resolve<ServerCommunications>().OnServerCheck -= dependencyContainer.Resolve<PairChecker>().PairEvaluated;
        CardBehaviour.OnCardFlipped -= dependencyContainer.Resolve<PairChecker>().CheckIfPair;
    }

    private void OpenScoreCoroutine()
    {
        StartCoroutine(dependencyContainer.Resolve<HUDManager>().OpenScorePanel());
    }

    private void AddScoreCoroutine()
    {
        StartCoroutine(dependencyContainer.Resolve<ScoreboardManager>().AddScore());
    }

    public void ResetUser()
    {
        if (Directory.Exists(saveFolder))
            Directory.Delete(saveFolder, true);
    }
}
