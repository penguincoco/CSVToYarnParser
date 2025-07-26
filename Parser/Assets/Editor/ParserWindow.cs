using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ParserWindow : EditorWindow
{
    private string csvFilePath = "";
    private string outputFolderPath = "Assets";

    //----- EDITOR RELATED -----//
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

            //take in the source CSV and output path
            ConvertToYarn(csvFilePath, outputFolderPath);
        }
    }



    //----- PROCESS AND OUTPUT -----//
    /*
        1. take in the csv, read it all
        2. split it up by line
        3. process each line
        4. return the line, add it to a List of all the lines
        5. output
   */
    void ConvertToYarn(string csvPath, string outputPath)
    {
        string rawTest = Parse(csvPath);
        string[] dialogue = rawTest.Split('\n');

        DialogueLine[] dialogueContainer = new DialogueLine[dialogue.Length];

        string yarnString = "";

        for (int i = 0; i < dialogue.Length; i++)
        {
            string[] pieces = dialogue[i].Split(new[] { ',' }, 3);

            // Handle blank line
            if (pieces[0].Length == 0)
                dialogueContainer[i] = new DialogueLine();
            else
                dialogueContainer[i] = new DialogueLine(pieces[0], pieces[1], pieces[2]);

            //only add the current string if there was anything in it. Don't add empty strings.
            if (dialogueContainer[i].GetDialogueLine().Equals("") == false)
            {
                yarnString += dialogueContainer[i].GetDialogueLine();
            }
        }

        string outputName = Path.GetFileNameWithoutExtension(csvPath);
        OutputYarnFile(yarnString, outputPath, outputName);
    }

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

        Debug.Log($"Yarn file written to: {fullOutputPath}");
    }
}



public class DialogueLine
{
    private string charName;
    private string charLine;
    private string emotion;

    private string dialogueLine;

    public DialogueLine(string charName, string emotion, string charLine)
    {
        this.charName = charName;
        this.emotion = emotion;
        this.charLine = charLine;

        Debug.Log("Character line is: " + this.charLine);

        //delimiter case. the line starts and ends with " because there is ',' or ' " ' somewhere IN the line.
        if (this.charLine[0].ToString().Equals("\""))
        {
            this.charLine = RemoveDelimiters(this.charLine);
        }

        dialogueLine = Translate(this.charName, this.emotion, this.charLine) + '\n';
    }

    //constructor for the blank line case
    public DialogueLine()
    {
        dialogueLine = '\n'.ToString();
    }

    private string RemoveDelimiters(string charLine)
    {
        //the raw line will have a leading and trailing """, so remove the first 3 characters, and last 3 characters
        //remove the delimiters at the front and end 
        string newCharLine = charLine.Substring(3, charLine.Length - 7);

        //check for delimiter INSIDE of the string
        if (HasDelimiter(newCharLine) == true)
        {
            string removedDelimiters = "";
            for (int i = 0; i < newCharLine.Length; i++)
            {
                //if we found a " in the string, then add it to the string
                removedDelimiters += newCharLine[i];
                if (newCharLine[i] == '"')
                {
                    //BUT then skip over the next character, because it's a guarantee duplicate 
                    i++;
                }
            }

            newCharLine = removedDelimiters;
        }

        return newCharLine;
    }

    private string Translate(string charName, string emotion, string charLine)
    {
        Debug.Log("Attempting to translate this raw data: " + charLine);
        string dialogueLine = charLine;

        if (charName.Equals("SYSTEM"))
        {
            if (charLine.Contains("ENDNODE"))
            {
                dialogueLine = "===" + '\n';
            }
            else if (charLine.Contains("NODE"))
            {
                //Example: "[NODE: ""Lobby0Meeting.1""]",
                //dialogueLine = charLine.Replace('"', ' ');
                dialogueLine = charLine.Substring(7, charLine.Length - 9);
                dialogueLine = "title: " + dialogueLine + '\n' + "---";
            }
            else
            {
                //have to do this step because the value has the NEW LINE character at the end, lmfao 
                string value = charLine.Substring(1, charLine.Length - 3);
                string systemValue;

                //Debug.Log("System line is: " + value);

                if (Utilities.availableActions.ContainsKey(value))
                    systemValue = Utilities.availableActions[value];
                else
                    systemValue = "";

                dialogueLine = systemValue;
            }
        }
        else if (charName.Equals("NOTE"))
        {
            // string value = charLine.Substring(0, charLine.Length - 1);
            // ParserObject.systemActionsDict[value];
            //dialogueLine = "";
            dialogueLine = "";
        }
        //Check for if the first char in the LINE is &, this means it's NARRATION
        else if (charLine.Substring(0, 1).Equals("&"))
        {
            //string value = charLine.Replace("&", "[i]");
            string value = charLine.Substring(1);
            value = charName + ": <i><cspace=0.2em><size=22>" + value + "</i></cspace></size>";

            dialogueLine = value;
        }
        else
        {
            dialogueLine = "<<initialize_line " + charName.ToLower() + " " + emotion.ToLower() + " >>" + '\n'.ToString() + charName + ": " + charLine;
        }

        return dialogueLine;
    }

    //---------- Helper Functions ----------//
    public string GetDialogueLine()
    {
        return dialogueLine;
    }

    private bool HasDelimiter(string charLine)
    {
        foreach (char c in Utilities.delimiterCharacters)
        {
            if (charLine.Contains(c.ToString()))
                return true;
        }
        return false;
    }
}

public class Utilities
{
    public static Dictionary<string, string> availableActions = new Dictionary<string, string>
    {
        { "SHAKE", "<<shakeOverworld vCam_cutsceneCam 3 1>>" },
        { "PAUSE", "<<wait 1.2>>" },
        { "BEACONLIGHT", "<<lightBeacon>>" },
    };

    public static List<char> delimiterCharacters = new List<char>
    {
        { '"'},
    };
}