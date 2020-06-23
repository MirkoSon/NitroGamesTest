using System;
using System.IO;
using UnityEngine;

/// <summary>
/// A simple Json implementation of the <see cref="ISerializer"/> that saves the scores locally at the _path.
/// </summary>
public class JsonSerializer : ISerializer
{
    protected readonly string _path;
    private const string _fileName = "Savefile";

    public JsonSerializer(string path)
    {
        _path = path;
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
    }

    public void Serialize<SavedScores>(SavedScores toSerialize)
    {
        string filePath = GetSaveFilePath(_fileName);
        string json = JsonUtility.ToJson(toSerialize);
        File.WriteAllText(filePath, json);
    }

    public bool Deserialize<SavedScores>(ref SavedScores savedData)
    {
        string filePath = GetSaveFilePath(_fileName);
        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            savedData = JsonUtility.FromJson<SavedScores>(json);
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }

        return false;
    }

    public void Delete()
    {
        string filePath = GetSaveFilePath(_fileName);
        File.Delete(filePath);
    }

    private string GetSaveFilePath(string fileName)
    {
        fileName = string.Format("{0}.json", fileName);
        fileName = Path.Combine(_path, fileName);

        return fileName;
    }
}
