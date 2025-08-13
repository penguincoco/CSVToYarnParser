using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class is for functions that the .yarn files will be calling. 
/// It's highly recommended that you 
/// </summary>

public class YarnCommands : MonoBehaviour
{
    [SerializeField] private static Image speaker;
    [SerializeField] private Image speakerImage;
    //[SerializeField] private static Animator frogAnimator;
    [SerializeField] private static AudioSource audioSource; // Add this if you want to play SFX

    private static Dictionary<string, SpeakerData> speakerDictionary = new Dictionary<string, SpeakerData>();

    private void Awake()
    {
        speakerDictionary = SpeakerDatabase.PopulateDictionary();
        speaker = speakerImage;

        // Set up audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    private void PopulateSpeakerDictionary()
    {
        speakerDictionary.Clear();

#if UNITY_EDITOR
        // Editor mode: Use AssetDatabase to find all SpeakerData assets
        string[] guids = AssetDatabase.FindAssets("t:SpeakerData");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SpeakerData speakerData = AssetDatabase.LoadAssetAtPath<SpeakerData>(path);
            if (speakerData != null && !string.IsNullOrEmpty(speakerData.name))
            {
                speakerDictionary[speakerData.name.ToLower()] = speakerData;
                Debug.Log($"Added speaker to dictionary: {speakerData.name}");
            }
        }
#else
        // Runtime mode: Load from Resources folder
        SpeakerData[] allSpeakers = Resources.LoadAll<SpeakerData>("");
        foreach (SpeakerData speakerData in allSpeakers)
        {
            if (speakerData != null && !string.IsNullOrEmpty(speakerData.name))
            {
                speakerDictionary[speakerData.name.ToLower()] = speakerData;
                Debug.Log($"Added speaker to dictionary: {speakerData.name}");
            }
        }
#endif

        Debug.Log($"Populated speaker dictionary with {speakerDictionary.Count} speakers");
    }

    //for documentation on setting up custom commands, callable via .yarn files, see the YarnSpinner documentation
    //https://docs.yarnspinner.dev/yarn-spinner-for-unity/creating-commands-functions 
    [YarnCommand("initialize_line")]
    public static void SetSpeakerInfo(string name, string expression = "")
    {
        if (speakerDictionary.TryGetValue(name.ToLower(), out SpeakerData speakerData))
        {
            // Set the sprite
            if (speaker != null && speakerData.sprite != null)
                speaker.sprite = speakerData.sprite;

            // Play SFX if available
            if (audioSource != null && speakerData.sfx != null)
                audioSource.PlayOneShot(speakerData.sfx);
        }
        else
            Debug.LogWarning($"Speaker '{name}' not found in dictionary. Available speakers: {string.Join(", ", speakerDictionary.Keys)}");
    }

    [YarnCommand("play_sound")]
    public static void PlayNarrativeSound(string soundName)
    {
        Debug.Log("Playing " + soundName + " sound");

        // Optional: You could also look up sounds by speaker name
        if (speakerDictionary.TryGetValue(soundName.ToLower(), out SpeakerData speakerData) && speakerData.sfx != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(speakerData.sfx);
            }
        }
    }

/*
    [YarnCommand("play_animation")]
    public static void PlayAnimation(string name)
    {
        if (frogAnimator != null)
        {
            frogAnimator.Play(name);
        }
    } */

    // Utility method to refresh the dictionary at runtime if needed
    [YarnCommand("refresh_speakers")]
    public void RefreshSpeakerDictionary()
    {
        PopulateSpeakerDictionary();
    }

    // Helper method to get available speakers (useful for debugging)
    public static string[] GetAvailableSpeakers()
    {
        string[] speakers = new string[speakerDictionary.Count];
        speakerDictionary.Keys.CopyTo(speakers, 0);
        return speakers;
    }

    [YarnCommand("customWait")]
    public static void WaitForTime(float waitTime)
    {
        StaticHelper.ExecuteCoroutine(StaticHelper.Wait(waitTime));
    }
}