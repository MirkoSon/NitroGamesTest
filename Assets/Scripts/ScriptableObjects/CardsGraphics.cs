using UnityEngine;

/// <summary>
/// Scriptable object used to configure which graphics to assign to cards' IDs.
/// </summary>
[CreateAssetMenu(fileName = "CardsGraphics", menuName = "NitroTest/CardsGraphics", order = 1)]
public class CardsGraphics : ScriptableObject
{
    public Sprite[] emotes;
}