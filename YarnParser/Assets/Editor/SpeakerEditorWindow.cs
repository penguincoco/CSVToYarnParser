using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class SpeakerEditorWindow : EditorWindow
{
    public SpeakerData speakerData;

    public string name = "";
    //private string newValue = "";
    public Sprite sprite;
    public AudioClip sfx;

    SerializedObject so;
    SerializedProperty propName;
    SerializedProperty propSprite;
    SerializedProperty propSFX;

    [MenuItem("Tools/Speaker Editor")]
    public static void ShowWindow() => GetWindow<SpeakerEditorWindow>("Speaker Editor");

    private void OnEnable()
    {
        so = new SerializedObject(this);
        propName = so.FindProperty("name");
        propSprite = so.FindProperty("sprite");
        propSFX = so.FindProperty("sfx");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create New Speaker", EditorStyles.boldLabel);

        so.Update();

        EditorGUILayout.PropertyField(propName, new GUIContent("Speaker"));
        EditorGUILayout.PropertyField(propSprite, new GUIContent("Portrait"));
        EditorGUILayout.PropertyField(propSFX, new GUIContent("SFX"));

        so.ApplyModifiedProperties();

        EditorGUILayout.Space();

        // Create button
        GUI.enabled = !string.IsNullOrEmpty(name);
        if (GUILayout.Button("Create SpeakerData", GUILayout.Height(30)))
        {
            CreateSpeakerDataAsset();
        }
        GUI.enabled = true;

        if (string.IsNullOrEmpty(name))
        {
            EditorGUILayout.HelpBox("Please enter a speaker name to create the asset.", MessageType.Warning);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    private void CreateSpeakerDataAsset()
    {
        // Create new SpeakerData instance
        SpeakerData newSpeakerData = ScriptableObject.CreateInstance<SpeakerData>();

        // Assign the data from the editor fields
        newSpeakerData.name = name;
        newSpeakerData.sprite = sprite;
        newSpeakerData.sfx = sfx;

        // Create the asset path
        string folderPath = "Assets/SpeakerData";

        // Create the folder if it doesn't exist
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "SpeakerData");
        }

        // Generate unique filename
        string fileName = name + ".asset";
        string assetPath = Path.Combine(folderPath, fileName);

        // Make sure the path is unique
        assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        // Create the asset
        AssetDatabase.CreateAsset(newSpeakerData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Select and ping the created asset
        Selection.activeObject = newSpeakerData;
        EditorGUIUtility.PingObject(newSpeakerData);

        // Clear the fields after creation
        name = "";
        sprite = null;
        sfx = null;

        // Update the SerializedObject to reflect the cleared fields
        so.Update();
        so.ApplyModifiedProperties();

        Debug.Log($"Created SpeakerData asset: {assetPath}");
    }
}