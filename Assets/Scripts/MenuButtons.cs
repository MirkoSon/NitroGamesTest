using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Monobehaviour class that handles the Menu scene Buttons. Works independently of the <see cref="IContext"/>
/// </summary>
public class MenuButtons : MonoBehaviour
{
    public Button startGameButton;
    public Button scoreboardButton;
    public Button exitGameButton;

    void OnEnable () {
        startGameButton.onClick.AddListener(() => { SceneManager.LoadSceneAsync(2); });
        scoreboardButton.onClick.AddListener(() => { SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive); });
        exitGameButton.onClick.AddListener(() => { Application.Quit(); });
    }
	
    void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
        scoreboardButton.onClick.RemoveAllListeners();
        exitGameButton.onClick.RemoveAllListeners();
    }
}
