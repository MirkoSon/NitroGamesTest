using System.Collections;
/// <summary>
/// Interface for Score management.
/// </summary>
public interface IScoreboardManager
{
    IEnumerator AddScore();

    void Load();

    void Save();

    void Delete();
}
