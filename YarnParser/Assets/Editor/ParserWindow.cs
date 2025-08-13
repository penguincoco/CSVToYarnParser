using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ParserWindow : EditorWindow
{
    private string csvFilePath = "";
    private string outputFolderPath = "";
    private string statusMessage = "";

    //private Animation[] animations;
    [SerializeField] private AnimationClip animationClip;

    [SerializeField]
    private List<SystemAction> customActions = new List<SystemAction>();
    private Vector2 actionScroll;

    [SerializeField] private AvailableActionsData selectedActionsData;

    //---------- EDITOR RELATED ----------//
    [MenuItem("Tools/Parser")]
    public static void ShowWindow()
    {
        GetWindow<ParserWindow>("Parser Window");
    }

    void OnGUI()
    {
        GUILayout.Label("Parser", EditorStyles.boldLabel);

        if (GUILayout.Button("Select CSV File"))
        {
            string path = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (!string.IsNullOrEmpty(path))
                csvFilePath = path;
        }

        EditorGUILayout.TextField("CSV File", csvFilePath);

        if (GUILayout.Button("Select Output Folder"))
        {
            string folder = EditorUtility.OpenFolderPanel("Output Folder", "Assets", "");
            if (!string.IsNullOrEmpty(folder))
                outputFolderPath = folder;
        }

        EditorGUILayout.TextField("Output Folder", outputFolderPath);

        if (GUILayout.Button("Convert to Yarn"))
        {
            if (string.IsNullOrEmpty(csvFilePath))
            {
                Debug.LogError("No CSV file selected.");
                return;
            }

            statusMessage = "Executing...";
            Repaint(); // Force the editor to update the window

            EditorApplication.delayCall += () =>
            {
                ConvertToYarn(csvFilePath, outputFolderPath, selectedActionsData);
                statusMessage = "Done!";
                Repaint();
            };
        }

        GUILayout.Space(10);
        GUILayout.Label($"Status: {statusMessage}");

        // ---------- CUSTOM YARN FUNCTIONS ---------- //

        GUILayout.Space(10);
        GUILayout.Label("Action Dictionary", EditorStyles.boldLabel);

        selectedActionsData = (AvailableActionsData)EditorGUILayout.ObjectField(
            "Available Actions Data",
            selectedActionsData,
            typeof(AvailableActionsData),
            false
        );

        /*
        // ---------- [WIP] ANIMATIONS ---------- //

        GUILayout.Space(10);

        GUILayout.Label("Animations", EditorStyles.boldLabel);

        animationClip = (AnimationClip)EditorGUILayout.ObjectField(
            "Animation Clip",
            animationClip,
            typeof(AnimationClip),
            false
        );
        */

        //This is WIP. Functionality to add yarn commands via a editor window, not updating the Script in the Utilities class
        /*
        GUILayout.Space(10);
        GUILayout.Label("Custom Yarn Actions", EditorStyles.boldLabel);

        // Scroll view for long lists
        actionScroll = EditorGUILayout.BeginScrollView(actionScroll, GUILayout.Height(150));

        for (int i = 0; i < customActions.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            customActions[i].key = EditorGUILayout.TextField(customActions[i].key, GUILayout.Width(100));
            customActions[i].yarnCommand = EditorGUILayout.TextField(customActions[i].yarnCommand);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                customActions.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        // Add new action
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+ Add Action"))
        {
            customActions.Add(new SystemAction { key = "", yarnCommand = "" });
        }
        GUILayout.EndHorizontal();
        */
    }



    //----- PROCESS AND OUTPUT HIGH LEVEL FLOW -----//
    /*
        1. take in the csv, read it all
        2. split it up by line break
        3. process each line 
        4. return the line, add it to a List of all the lines
        5. output
   */

    void ConvertToYarn(string csvPath, string outputPath, AvailableActionsData actionsData)
    {
        if (selectedActionsData == null)
        {
            Debug.LogError("No AvailableActionsData selected!");
            return;
        }

        string yarnString = Parser.ConvertToYarn(csvPath, selectedActionsData);
        //the output yarn file will have the same name as the .csv
        string outputName = Path.GetFileNameWithoutExtension(csvPath);
        OutputYarnFile(yarnString, outputPath, outputName);
    }

    //takes in the csv and parses it out into a big string
    private string Parse(string csvFilePath)
    {
        if (File.Exists(csvFilePath))
        {
            try
            {
                string csvContent = File.ReadAllText(csvFilePath);
                return csvContent;
            }
            catch (System.Exception e)
            {
                return string.Empty;
            }
        }
        else
            return string.Empty;
    }

    private void OutputYarnFile(string yarnText, string outputPath, string outputName)
    {
        string outputFileName = outputName + ".yarn";

        string fullOutputPath = Path.Combine(outputFolderPath, outputFileName);

        File.WriteAllText(fullOutputPath, yarnText);
        AssetDatabase.Refresh();
        Debug.Log($"Yarn file written to: {fullOutputPath}");
    }
}