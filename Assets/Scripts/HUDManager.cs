using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Monobehaviour class that handles all of the UI functions of the game. It listens to the changes that occur to the
/// data through the <see cref="GameDataManager"/> and updates accordingly.
/// </summary>
public class HUDManager : MonoBehaviour
{
    public Canvas canvas;
    public Text livesValue, scoreValue, multiplierValue, powerupsValue;
    public Button powerUpWidget;
    public GameObject loadingAnimation;

    [Space]
    public GameObject winOrLosePanel;
    public Text header, body;

    private GameDataManager _gameData;
    private PairChecker _pairChecker;
    private ScoreCalculator _scoreCalculator;

    private const string youWin = "You Win!";
    private const string youLose = "You Lose";
    private const string scoreIs = "Your Score is: ";

    private ColorBlock _block;

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _pairChecker = dependencyContainer.Resolve<PairChecker>();
        _gameData = dependencyContainer.Resolve<GameDataManager>();
        _scoreCalculator = dependencyContainer.Resolve<ScoreCalculator>();
    }

    public void InitializeUI()
    {
        livesValue.text = _gameData.configuration.livesCount.ToString();
        powerupsValue.text = _gameData.configuration.powerupsCount.ToString();
        multiplierValue.text = "1";
        _block = powerUpWidget.colors;
        canvas.enabled = true;
    }

    public void UpdateLivesAndPowerUps()
    {

        livesValue.text = _gameData.currentLives.ToString();
        powerupsValue.text = _gameData.currentPowerUps.ToString();

        if (_gameData.currentPowerUps == 0)
            powerUpWidget.interactable = false;

        _block.colorMultiplier = _pairChecker.hasPowerUp ? 2 : 1;
        powerUpWidget.colors = _block;
    }

    public void UpdateScoreAndMultipliers()
    {
        scoreValue.text = _scoreCalculator.CurrentScore.ToString();
        multiplierValue.text = Mathf.Max(1, _scoreCalculator.CurrentMultiplier).ToString();
    }

    public void SwitchAnimationState(bool state)
    {
        if (loadingAnimation != null)
            loadingAnimation.SetActive(state);
    }

    public IEnumerator OpenScorePanel()
    {
        int previousScore = _scoreCalculator.CurrentScore;

        if (_gameData.currentLives != 0)
            yield return new WaitUntil(() => _scoreCalculator.CurrentScore != previousScore);

        header.text = _gameData.currentLives == 0 ? youLose : youWin;
        body.text = string.Format("{0}{1}", scoreIs, _scoreCalculator.CurrentScore);
        winOrLosePanel.SetActive(true);
    }
}
