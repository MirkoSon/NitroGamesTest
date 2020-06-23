using UnityEngine;
using NUnit.Framework;
using NSubstitute;

namespace Tests
{
    public class EditModeTests
    {
        [Test]
        public void DependencyContainer_Gets_Initialized_And_Used_On_PairChecker()
        {
            // ARRANGE
            DependencyContainer dependencyContainer = new DependencyContainer();
            ServerCommunications serverCommunications = new ServerCommunications();
            GameDataManager gameData = new GameDataManager();
            IMain main = Substitute.For<IMain>();

            dependencyContainer.Bind<Main>(main as Main);
            dependencyContainer.Bind<GameDataManager>(gameData);
            dependencyContainer.Bind<ServerCommunications>(serverCommunications);

            gameData.currentPowerUps = 2;
            int expected = 1;

            PairChecker pairChecker = new PairChecker();
            pairChecker.Initialize(dependencyContainer);

            // ACT
            pairChecker.UsePowerUp();

            // ASSERT
            Assert.AreEqual(expected, gameData.currentPowerUps);
        }

        [Test]
        public void PairChecker_Comparing_Three_IDs()
        {
            // ARRANGE
            PairChecker pairChecker = new PairChecker();
            string expected = JsonUtility.ToJson(new Request
            {
                cardOneId = 3,
                cardTwoId = 3
            });

            // ACT
            string check = JsonUtility.ToJson(pairChecker.CompareThreeIds(3, 2, 3));

            // ASSERT
            Assert.AreEqual(expected, check);
        }

        [Test]
        public void Score_Gets_Increased_Three_Times_In_A_Row_Then_One_More()
        {
            // ARRANGE
            ScoreCalculator scoreCalculator = new ScoreCalculator();
            int score = 0;
            scoreCalculator.cardValue = 1;
            scoreCalculator.multiplierValue = 3;
            int expectedScore = 22;
            // ACT
            score += scoreCalculator.IncreaseScore();
            scoreCalculator.UpdateScore(true, 0);
            score += scoreCalculator.IncreaseScore();
            scoreCalculator.UpdateScore(true, 0);
            score += scoreCalculator.IncreaseScore();
            scoreCalculator.UpdateScore(false, 0);
            score += scoreCalculator.IncreaseScore();
            // ASSERT
            Assert.AreEqual(expectedScore, score);
        }
    }
}
