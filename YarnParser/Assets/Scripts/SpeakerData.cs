using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpeakerData", menuName = "Yarn Data/SpeakerData")]
public class SpeakerData : ScriptableObject
{
    //this is for the default sprite and sfx for each character
    public string name;
    public Sprite sprite;
    public AudioClip sfx;

    /*
    [Serializable]
    public class EmotionEntry
    {
        public string emotion;
        public Sprite sprite;
        public AudioClip sfx;
    }

    public string speakerName;

    // List of all emotion-sprite-sfx combos
    public List<EmotionEntry> emotions = new List<EmotionEntry>();

    // Optional: helper method to get data for a specific emotion
    public EmotionEntry GetEmotion(string emotionName)
    {
        return emotions.Find(e => e.emotion == emotionName);
    }
    */
}