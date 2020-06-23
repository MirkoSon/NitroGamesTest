using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Monobehaviour Class that handles the end game's popup buttons functionality.
/// </summary>
public class EndGameUI : MonoBehaviour
{
    public Button tryAgainButton;
    public Button backToMenuButton;

    private Main _main;

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _main = dependencyContainer.Resolve<Main>();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        Register();
    }

    void Register()
    { 
        tryAgainButton.onClick.AddListener(() => { _main.EndGame();  SceneManager.LoadSceneAsync(2); });
        backToMenuButton.onClick.AddListener(() => { _main.EndGame(); SceneManager.LoadSceneAsync(0); });
    }

    public void Unregister()
    {
        tryAgainButton.onClick.RemoveListener(() => { _main.EndGame(); SceneManager.LoadSceneAsync(2); });
        backToMenuButton.onClick.RemoveListener(() => { _main.EndGame(); SceneManager.LoadSceneAsync(0); });
    }

    public void UnloadLoadingScene(string json)
    {
        SceneManager.UnloadSceneAsync(1);
    }
}
