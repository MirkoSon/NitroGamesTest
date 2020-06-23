using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Monobehaviour class that handles the Scoreboard's scene UI. Works independently of the <see cref="IContext"/>
/// </summary>
public class ScoreboardUI : MonoBehaviour
{
    public Text[] scoreTexts;
    public Button backButton;
    public Button resetButton;

    private ISerializer _serializer;

	void OnEnable ()
    {
        _serializer = new JsonSerializer(Context.saveFolder);
        backButton.onClick.AddListener(() => { SceneManager.UnloadSceneAsync(3); });
        resetButton.onClick.AddListener(EraseScores);
        LoadScores();
    }
	
	void OnDisable()
    {
        backButton.onClick.RemoveAllListeners();
    }

    void LoadScores()
    {
        SavedScores savedScores = new SavedScores();
        if (_serializer.Deserialize(ref savedScores))
        {
            for (int i = 0; i < savedScores.scores.Count; i++)
            {
                scoreTexts[i].text = savedScores.scores[i].ToString();
            }
        }
    }

    void EraseScores()
    {
        _serializer.Delete();
        foreach (Text text in scoreTexts)
            text.text = "-";
    }
}
