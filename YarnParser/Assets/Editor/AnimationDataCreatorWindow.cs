using UnityEngine;
using UnityEditor;

/// <summary>
/// WIP
/// </summary>

public class AnimationDataCreatorWindow : EditorWindow
{
    /*
    private string speakerName = "";
    private AnimationClip animationClip;

    //[MenuItem("Tools/Create Animation Data")]
    public static void ShowWindow()
    {
        GetWindow<AnimationDataCreatorWindow>("Animation Data Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Create Dialogue Data", EditorStyles.boldLabel);

        speakerName = EditorGUILayout.TextField("Speaker", speakerName);
        animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", animationClip, typeof(AnimationClip), false);

        GUILayout.Space(10);

        GUI.enabled = !string.IsNullOrEmpty(speakerName) && animationClip != null;
        if (GUILayout.Button("Create DialogueData Asset"))
        {
            CreateDialogueData(speakerName, animationClip);
        }
        GUI.enabled = true;
    }

    void CreateDialogueData(string name, AnimationClip clip)
    {
        DialogueData newDialogueAsset = ScriptableObject.CreateInstance<DialogueData>();
        newDialogueAsset.name = name;
        newDialogueAsset.animationClip = clip;

        string path = EditorUtility.SaveFilePanelInProject("Save Dialogue Data", $"New_{name}", "asset", "Specify where to save the AnimationData asset.");
        if (string.IsNullOrEmpty(path))
            return;

        AssetDatabase.CreateAsset(newDialogueAsset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newDialogueAsset;

        Debug.Log($"DialogueData asset created at {path}");
    }
    */
}