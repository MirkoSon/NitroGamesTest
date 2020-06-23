using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Service class that manages the spawning of cards at the start of each game.
/// It reads the game configuration from <see cref="GameDataManager"/>.
/// </summary>
public class CardSpawner : MonoBehaviour
{
    public CardsGraphics cardsGraphics;
    public GameObject cardPrefab;
    public GridLayoutGroup gridLayout;

    private GameDataManager _gameData;
    private IDependencyContainer _dependencyContainer;

    public void Initialize(IDependencyContainer dependencyContainer)
    {
        _dependencyContainer = dependencyContainer;
        _gameData = dependencyContainer.Resolve<GameDataManager>();
    }

    public void SpawnCards()
    {
        // Set Grid Rows Number
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = _gameData.configuration.rowSize;

        // Draw Cards
        for (int i = 0; i < _gameData.configuration.cardIds.Length; i++)
        {
            CardEntity entity = Instantiate(cardPrefab, gridLayout.transform).GetComponent<CardEntity>();
            CardBehaviour cardBehaviour = entity.GetComponent<CardBehaviour>();
            int cardId = _gameData.configuration.cardIds[i];
            entity.Id = cardId;
            entity.Emote = cardsGraphics.emotes[cardId];
            cardBehaviour.Initialize(_dependencyContainer);
        }
    }

    public void DestroyCards()
    {
        foreach (Transform t in gridLayout.transform)
            Destroy(t.gameObject);
    }
}
