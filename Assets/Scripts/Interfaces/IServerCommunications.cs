using System.Collections;

/// <summary>
/// Interface for specific Server communications about the game
/// </summary>
public interface IServerCommunications
{
    /// <summary>
    /// It gets the game configuration from server
    /// </summary>
    IEnumerator GetRequest(string url);

    /// <summary>
    /// Validates the revealed cards
    /// </summary>
    IEnumerator CheckPair(string url, string json);
}
