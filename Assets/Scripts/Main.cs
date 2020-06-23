using UnityEngine;

/// <summary>
/// Monobehaviour class that starts and ends each game. Works in bundle with the <see cref="IContext"/>
/// to call the Binding and the Unbinding of dependencies, so that no listener or dependency would persist accidentally between games.
/// </summary>
[RequireComponent(typeof(IContext))]
public class Main : MonoBehaviour, IMain
{
    private IContext _context;
    protected IContext context => _context ?? (_context = GetComponent<IContext>());
    protected IDependencyContainer dependencyContainer => context.dependencyContainer;

    void Start()
    {
        Init();
        StartGame();
    }

    protected void Init()
    {
        dependencyContainer.Bind<Main>(this);
        context.Bind();
    }

    public void StartGame()
    {
        dependencyContainer.Resolve<GameDataManager>().Initialize(dependencyContainer);
        dependencyContainer.Resolve<EndGameUI>().Initialize(dependencyContainer);
        dependencyContainer.Resolve<CardSpawner>().Initialize(dependencyContainer);
        dependencyContainer.Resolve<PairChecker>().Initialize(dependencyContainer);
        dependencyContainer.Resolve<HUDManager>().Initialize(dependencyContainer);
        dependencyContainer.Resolve<ScoreCalculator>().Initialize(dependencyContainer);
    }

    public void EndGame()
    {
        dependencyContainer.Resolve<CardSpawner>().DestroyCards();
        dependencyContainer.Resolve<EndGameUI>().Unregister();
        context.UnBind();
    }
}