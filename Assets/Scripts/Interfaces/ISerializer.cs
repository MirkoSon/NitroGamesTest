/// <summary>
/// Interface for serializing the data whether it is local or remote.
/// </summary>
public interface ISerializer
{
    void Serialize<SavedScores>(SavedScores toSerialize);

    bool Deserialize<SavedScores>(ref SavedScores savedData);

    void Delete();
}