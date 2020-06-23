using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This Singleton Monobehaviour is needed to run Coroutines from Unit Testing !!!ONLY!!!
/// </summary>
public class TestRoutineRunner : MonoBehaviour
{
    private static TestRoutineRunner _instance;

    #region Singleton
    public static TestRoutineRunner Instance
    {
        get
        {
            if (_instance)
                return _instance;
            else
                _instance = FindObjectOfType<TestRoutineRunner>();

            if (_instance)
                return _instance;
            else
                _instance = new GameObject("TestRoutineRunner").AddComponent<TestRoutineRunner>();

            return _instance;
        }
    }
    #endregion

    // Set a Callback for when the coroutine is complete
    public void TestRoutine(IEnumerator routine, Action whenDone)
    {
        StartCoroutine(RunRoutine(routine, whenDone));
    }

    private IEnumerator RunRoutine(IEnumerator routine, Action whenDone)
    {
        // Runs the coroutine and waits until it's finished.
        yield return routine;

        // Invokes the callback.
        whenDone?.Invoke();
    }
}