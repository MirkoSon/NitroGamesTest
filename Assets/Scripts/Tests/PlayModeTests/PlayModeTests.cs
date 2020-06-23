using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Tests
{
    public class PlayModeTests
    {
        [UnityTest]
        public IEnumerator Server_HTTP_GET_CallTest()
        {
            // ARRANGE
            ServerCommunications communications = new ServerCommunications();
            Configuration deserialized = new Configuration();
            string url = "http://localhost:3000/configuration/1";
            int expectedRowSize = 4;
            bool isValidated = false;
            communications.OnServerResponse += delegate(string json)
            {
                deserialized = JsonUtility.FromJson<Configuration>(json);
                isValidated = true;
            };
            // ACT
            TestRoutineRunner.Instance.StartCoroutine(communications.GetRequest(url));

            while (!isValidated)
                yield return null;
            // ASSERT
            Assert.AreEqual(expectedRowSize, deserialized.rowSize);
        }

        [UnityTest]
        public IEnumerator Server_HTTP_PUT_CallTest()
        {
            // ARRANGE
            ServerCommunications communications = new ServerCommunications();
            Response response = new Response();
            string url = "http://localhost:3000/play/";
            bool isValidated = false;
            Request request = new Request()
            {
                cardOneId = 5,
                cardTwoId = 5
            };
            string jsonRequest = JsonUtility.ToJson(request);
            communications.OnServerCheck += delegate (string json)
            {
                response = JsonUtility.FromJson<Response>(json);
                isValidated = true;
            };
            // ACT
            TestRoutineRunner.Instance.StartCoroutine(communications.CheckPair(url, jsonRequest));

            while (!isValidated)
                yield return null;
            // ASSERT
            Assert.IsTrue(response.isCorrect);
        }
    }
}
