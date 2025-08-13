using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class Parser
{
    public static string ConvertToYarn(string csvPath, AvailableActionsData actionsData)
    {
        //read from availableActions
        Utilities.LoadAvailableActions(actionsData);

        string rawText = Parse(csvPath); // Read CSV as string
        string[] dialogue = rawText.Split('\n');

        DialogueLine[] dialogueContainer = new DialogueLine[dialogue.Length];
        string yarnString = "";

        //iterate through dialogue array, create a new DialogueObject for every element in the array
        //each individual element will format and parse itself
        for (int i = 0; i < dialogue.Length; i++)
        {
            //TODO: for people using this tool, split the sheet as desired. In my use case (see Beacon1.csv), I had 3 columns. 
            // ---------- START TODO ---------- //
            string[] pieces = dialogue[i].Split(new[] { ',' }, 3);

            // edge case -- length = 0  is a blank line
            if (pieces[0].Length == 0)
                dialogueContainer[i] = new DialogueLine();
            else    //Each line is its own Object -- DialogueLine
                dialogueContainer[i] = new DialogueLine(pieces[0], pieces[1], pieces[2]);

            //only add the current string if there was anything in it. Don't add empty strings.
            if (dialogueContainer[i].GetDialogueLine().Equals("") == false)
            {
                yarnString += dialogueContainer[i].GetDialogueLine();
            }
            // ---------- END TODO ---------- //
        }

        return yarnString;
    }

    private static string Parse(string csvFilePath)
    {
        if (File.Exists(csvFilePath))
        {
            try
            {
                return File.ReadAllText(csvFilePath);
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
        else
            return string.Empty;
    }
}
