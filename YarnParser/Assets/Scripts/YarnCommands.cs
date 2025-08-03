using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class YarnCommands : MonoBehaviour
{
    [SerializeField] private static Image speaker;
    [SerializeField] private Image speakerImage;

    [SerializeField] private Sprite frodoSprite;
    [SerializeField] private Sprite gandalfSprite;

    private static Dictionary<string, Sprite> speakerDictionary = new Dictionary<string, Sprite>();

    private void Awake()
    {
        speakerDictionary.Add("frodo", frodoSprite);
        speakerDictionary.Add("gandalf", gandalfSprite);

        speaker = speakerImage;
    }

    //for documentation on setting up custom commands, callable via .yarn files, see the YarnSpinner documentation
    //https://docs.yarnspinner.dev/yarn-spinner-for-unity/creating-commands-functions 
    [YarnCommand("initialize_line")]
    public static void SetSpeakerInfo(string name, string expression)
    {
        speaker.sprite = speakerDictionary[name];
    }

    [YarnCommand("play_sound")]
    public static void PlayNarrativeSound(string name)
    {
        Debug.Log("Playing " + name + " character sound");
    }
}
