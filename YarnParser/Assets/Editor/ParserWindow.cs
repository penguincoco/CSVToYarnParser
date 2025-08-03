using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ParserWindow : EditorWindow
{
    private string csvFilePath = "";
    private string outputFolderPath = "";
    private string statusMessage = "";

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
                ConvertToYarn(csvFilePath, outputFolderPath);
                statusMessage = "Done!";
                Repaint();
            };
        }

        GUILayout.Space(10);
        GUILayout.Label($"Status: {statusMessage}");
    }



    //----- PROCESS AND OUTPUT HIGH LEVEL FLOW -----//
    /*
        1. take in the csv, read it all
        2. split it up by line break
        3. process each line 
        4. return the line, add it to a List of all the lines
        5. output
   */

    void ConvertToYarn(string csvPath, string outputPath)
    {
        string yarnString = Parser.ConvertToYarn(csvPath); 

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