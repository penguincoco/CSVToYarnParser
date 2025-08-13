using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine
{
    private string charName;
    private string charLine;
    private string emotion;

    private string dialogueLine;

    //constructor for lines with data
    public DialogueLine(string charName, string emotion, string charLine)
    {
        this.charName = charName;
        this.emotion = emotion;
        this.charLine = charLine;

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
                if (newCharLine[i] == '"')  //hard-coded for now to check for quotation marks inside of the line, will fix later
                {
                    //BUT then skip over the next character, because it's a guarantee " 
                    i++;
                }
            }

            newCharLine = removedDelimiters;
        }

        return newCharLine;
    }

    /*
    TODO: Implement Translate() based on whatever needs for your project.
    tip: pass in the "cleanest" form of data possible. Meaning all extra characters from the .csv, formatting funkiness
    as part of your spreadsheet implementation, etc. Try and have it be as clean data as possible. 
    For example, if your original .csv, charLine was 
    """sarah said ""hi"" to my mom"
    try and clean it up before sending it to Translate(). Have it be 
    sarah said "hi" to my mom  
    in the case of my example (see Beacon1.csv for input, and Beacon1.yarn for output), we needed a unique function call before 
    every speaker's dialogue that would change the character portrait based off of the speaker, and their emotion 
    */
    private string Translate(string charName, string emotion, string charLine)
    {
        //---------- REPLACE BODY AND FUNCTION PARAMETERS AS NEEDED ----------//
        string dialogueLine = charLine;

        //no character is speaking. It's a system action. 
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

                if (Utilities.availableActions.ContainsKey(value))
                    systemValue = Utilities.availableActions[value];
                else
                    systemValue = "";

                dialogueLine = systemValue;
            }
        }
        //no character speaking, and not a system action. It's just a note in the script about the flow 
        else if (charName.Equals("NOTE"))
        {
            dialogueLine = "";
        }
        //Check for if the first char in the LINE is &, this means it's NARRATION
        else if (charLine.Substring(0, 1).Equals("&"))
        {
            string value = charLine.Substring(1);
            value = charName + ": <i><cspace=0.2em><size=22>" + value + "</i></cspace></size>";

            dialogueLine = value;
        }
        //any other case--basically means it's a character speaking
        else
        {
            dialogueLine = "<<initialize_line " + charName.ToLower() + " " + emotion.ToLower() + " >>" + '\n'.ToString() + charName + ": " + charLine;
        }

        return dialogueLine;
        //---------- END REPLACE ----------//
    }


    //---------- Helper Functions ----------//
    public string GetDialogueLine()
    {
        return dialogueLine;
    }

    //checks for delimiting characters, as defined in Utilities.delimiterCharacters
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

/*
public class Utilities
{
    //TODO: Implement YarnFunctions here
    //as you can see, for my example, we implemented a custom SHAKE function
    public static Dictionary<string, string> availableActions = new Dictionary<string, string>
    {
        { "SHAKE", "<<shakeOverworld vCam_cutsceneCam 3 1>>" },
        { "PAUSE", "<<wait 1.2>>" },
        { "BEACONLIGHT", "<<lightBeacon>>" },
        { "PLAYSOUND", "<<play_sound happy>>"}
    };

    public static List<char> delimiterCharacters = new List<char>
    {
        { '"'},
    };
}  */


public static class Utilities
{
    public static Dictionary<string, string> availableActions = new Dictionary<string, string>();
    public static List<char> delimiterCharacters = new()
    {
        { '"' },
    };

    public static void LoadAvailableActions(AvailableActionsData data)
    {
        availableActions.Clear();

        if (data == null)
        {
            Debug.LogWarning("AvailableActionsData is null!");
            //TODO: default it to Default_AvailableAction
            return;
        }

        foreach (var entry in data.actions)
        {
            if (!availableActions.ContainsKey(entry.key))
                availableActions.Add(entry.key, entry.value);
            else
                Debug.LogWarning($"Duplicate key: {entry.key}");
        }
    }
}