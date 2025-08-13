using UnityEditor;
using UnityEngine;

public class ActionEditorWindow : EditorWindow
{
    private AvailableActionsData actionsData;

    private string newKey = "";
    private string newValue = "";

    [MenuItem("Tools/Action Editor")]
    public static void ShowWindow()
    {
        GetWindow<ActionEditorWindow>("Action Editor");
    }

    private void OnGUI()
    {
        actionsData = (AvailableActionsData)EditorGUILayout.ObjectField("Actions Data", actionsData, typeof(AvailableActionsData), false);

        if (actionsData == null)
        {
            EditorGUILayout.HelpBox("Assign an AvailableActionsData asset to begin editing.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Action", EditorStyles.boldLabel);
        newKey = EditorGUILayout.TextField("csv Syntax", newKey);
        newValue = EditorGUILayout.TextField("YarnCommand Call", newValue);

        if (GUILayout.Button("Add Action"))
        {
            if (!string.IsNullOrEmpty(newKey) && !string.IsNullOrEmpty(newValue))
            {
                // Register undo before modifying the object
                Undo.RecordObject(actionsData, "Add Action");

                actionsData.actions.Add(new AvailableActionsData.ActionEntry { key = newKey, value = newValue });
                EditorUtility.SetDirty(actionsData); // Mark as dirty for saving
                newKey = "";
                newValue = "";
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Existing Actions", EditorStyles.boldLabel);

        for (int i = 0; i < actionsData.actions.Count; i++)
        {
            var action = actionsData.actions[i];
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            string newKey = EditorGUILayout.TextField(action.key, GUILayout.Width(150));
            string newValue = EditorGUILayout.TextField(action.value);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(actionsData, "Edit Action");
                action.key = newKey;
                action.value = newValue;
                EditorUtility.SetDirty(actionsData);
            }

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                Undo.RecordObject(actionsData, "Remove Action");
                actionsData.actions.RemoveAt(i);
                EditorUtility.SetDirty(actionsData);

                EditorGUILayout.EndHorizontal(); // ✅ Fix: Close layout block before break
                break;
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}