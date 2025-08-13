using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "DialogueData", menuName = "Yarn Data/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class SpeakerData
    {
        public string name;
        [TextArea]
        public Sprite sprite;
        public AnimationClip animationClip;
    }

    public List<SpeakerData> speakers = new List<SpeakerData>();
}