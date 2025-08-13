using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SpeakerDatabase
{
    private static Dictionary<string, SpeakerData> speakerDictionary = new Dictionary<string, SpeakerData>();

    public static Dictionary<string, SpeakerData> PopulateDictionary()
    {
        speakerDictionary.Clear();

#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:SpeakerData");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var data = AssetDatabase.LoadAssetAtPath<SpeakerData>(path);
            if (data != null) speakerDictionary[data.name.ToLower()] = data;
        }
#else
        var allSpeakers = Resources.LoadAll<SpeakerData>("");
        foreach (var data in allSpeakers)
        {
            if (data != null) speakerDictionary[data.name.ToLower()] = data;
        }
#endif
        return speakerDictionary;
    }

    public static SpeakerData GetSpeaker(string name)
    {
        speakerDictionary.TryGetValue(name.ToLower(), out var data);
        return data;
    }

    public static string[] GetAllSpeakerNames()
    {
        var keys = new string[speakerDictionary.Count];
        speakerDictionary.Keys.CopyTo(keys, 0);
        return keys;
    }
}