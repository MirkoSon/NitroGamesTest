using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that stores each card's data. Id and Graphics.
/// </summary>
public class CardEntity : MonoBehaviour
{
    public Image emoteImage;
    private int _id;
    
    public int Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    public Sprite Emote
    {
        set
        {
            emoteImage.sprite = value;
        }
    }     
}
