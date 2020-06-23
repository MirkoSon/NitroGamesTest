using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Helper class implementation of <see cref="IServerCommunications"/>
/// It handles all of the communications with the server.
/// </summary>
public class ServerCommunications : IServerCommunications
{
    public Action<bool> OnServerWait;
    public Action<string> OnServerResponse;
    public Action<string> OnServerCheck;

    public IEnumerator GetRequest(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            OnServerResponse?.Invoke(www.downloadHandler.text);
        }
    }

    public IEnumerator CheckPair(string url, string request)
    {
        OnServerWait?.Invoke(true);
        UnityWebRequest www = UnityWebRequest.Put(url, request);
        www.uploadHandler.contentType = "application/json";
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            OnServerWait?.Invoke(false);
            OnServerCheck?.Invoke(www.downloadHandler.text);
        }
    }
}
