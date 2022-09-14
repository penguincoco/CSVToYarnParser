using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ParserObject : MonoBehaviour
{
    public TextAsset dialogueCSV;
    public TextAsset yarnOutputFile;
    public string[] dialogue;
    //public List<string, string> dialogueList = new List<string, string>();
    //public List<string> speakerContainer = new List<string>();
    //public List<string> dialogueContainer = new List<string>();

    public DialogueLine[] dialogueContainer;
    public int rowCount;

    public string yarnString;
    public TMP_Text UIText;

    public static Dictionary<string, string> systemActionsDict = new Dictionary<string, string>();
    public static Dictionary<char, string> UIActionsDict = new Dictionary<char, string>();

    public static List<char> utilityChars = new List<char>() { '*', '_' };

    //<<shake_cam BlackGuard 3 1>>

    void Awake() 
    {
        //dialogueContainer = new DialogueLine[rowCount];

        systemActionsDict.Add("[PAUSE]", "<<pause>>");
        systemActionsDict.Add("[NODE:", "title");
        systemActionsDict.Add("[SHAKE]", "<<shake>>");
        //systemActionsDict.Add("NOTE","<<function call>>");

        UIActionsDict.Add('*', "<<bold>>");
        UIActionsDict.Add('_', "<<italics>>");

        //systemActionsDict.Add("[ITALICS]", "<<italicize>>");

        
    }

    void Start()
    {
        Parse();
        Translate();
    }

    private void Parse() {
        string text = dialogueCSV.text;

        dialogue = text.Split('\n');

        dialogueContainer = new DialogueLine[dialogue.Length];

        for (int i = 0; i < dialogue.Length; i++) 
        {
            //Debug.Log(dialogue[i]);
            string[] pieces = dialogue[i].Split(new[] { ',' }, 3);

            //Debug.Log("Length " + pieces[0].Length.ToString() + " " + pieces[1].Length.ToString());
            
            //blank line case, call the blank line constructor
            if (pieces[0].Length == 0)
                dialogueContainer[i] = new DialogueLine();
            else 
                dialogueContainer[i] = new DialogueLine(pieces[0], pieces [1], pieces[2]);

            //Debug.Log(dialogueContainer[i].GetDialogueLine());
            yarnString += dialogueContainer[i].GetDialogueLine();

            UIText.text = yarnString;
            
        }


        // foreach(string line in dialogue)
        // {
        //     string[] pieces = line.Split(new[] { ',' }, 2);


        //     // speakerContainer.Add(pieces[0]);
        //     // dialogueContainer.Add(pieces[1]);
            
        //     //string newLine = speakerContainer[dialogueCounter] + " : " + dialogueContainer[dialogueCounter];

        //     //Debug.Log(newLine);
        //     //yarnString += newLine;

        //     dialogueCounter++;
        // }

        //int dialogueCounter = 0;

        // foreach(string line in dialogue) {
        //     //Debug.Log(line);
        //     string[] lineSplit = line.Split(',');

        //     dialogueContainer[dialogueCounter][dialo]
        //     //dialogueList.Add(lineSplit[0], lineSplit[1]);

        //     // foreach (KeyValuePair<string, string> kvp in dialogueDict) 
        //     // {
        //     //     Debug.Log(kvp.Key + " : " + kvp.Value);
        //     // }
        // }
    }

    private void Translate() {
        int dialogueLineCounter = 0;
    }
}

public class DialogueLine
{
    private string charName;
    private string charLine;
    private string emotion;

    private string dialogueLine;

    //this constructor should only ever be called if the line found actually had ANY content at all in either the name column, or line column
    public DialogueLine(string charName, string emotion, string charLine)
    {
        this.charName = charName;
        this.emotion = emotion;
        this.charLine = charLine;

        charLine =  charLine.Substring(0, charLine.Length - 2);

        //delimiter case. the line starts and ends with " because there is ',' or ' " ' somewhere in the line
        if (charLine.Substring(0,1).Equals("\""))
        {
            charLine = RemoveDelimiters(charLine);
        }

        dialogueLine = Translate(charName, emotion, charLine) + '\n';
    }

    // public DialogueLine(string command) 
    // {

    // }

    //constructor for the blank line case
    public DialogueLine() 
    {
        dialogueLine = '\n'.ToString();
    }


    private string RemoveDelimiters(string charLine) 
    {
        return charLine.Substring(1, charLine.Length - 2);
    }

    private string Translate(string charName, string emotion, string charLine) 
    {
        string dialogueLine;

        if (charName.Equals("SYSTEM"))
        {
            if (charLine.Contains("ENDNODE")) 
            {
                dialogueLine = "===";
            }
            else if (charLine.Contains("NODE"))
            {
                //"[NODE: ""Lobby0Meeting.1""]",
                dialogueLine = charLine.Substring(9);
                dialogueLine = dialogueLine.Substring(0, dialogueLine.Length - 3);
                //dialogueLine = charLine.Replace('"', ' ');
                dialogueLine = "title: " + dialogueLine + '\n' + "---";
            }
            else 
            {
                //have to do this step because the value has the NEW LINE character at the end, lmfao 
                string value = charLine;
                string systemValue = ParserObject.systemActionsDict[value];

                dialogueLine = systemValue;
            }
        }
        else if(charName.Equals("NOTE")) 
        {
            // string value = charLine.Substring(0, charLine.Length - 1);
            // ParserObject.systemActionsDict[value];
            dialogueLine = "<<function call>>";
        }

        else if (charLine.Substring(0, 1).Equals("&")) 
        {
            //string value = charLine.Replace("&", "[i]");
            string value = charLine.Substring(1, charLine.Length - 3);
            value = charName + " : [i]" + value + "[/i]";

            dialogueLine = value;
        }
        else
        {
            if (charLine.Contains(ParserObject.utilityChars[0].ToString()) || charLine.Contains(ParserObject.utilityChars[1].ToString()))
            {
                Debug.Log("there was a utility char");
            }

            dialogueLine = "<<PlaySFX " + charName.ToLower() + " " + emotion.ToLower() + " >>" + '\n'.ToString() + charName + ": " + charLine;

        }

        //dialogueLine = dialogueLine.Substring(0, dialogueLine.Length - 1);

        return dialogueLine;
    }

    public string GetDialogueLine() 
    {
        return dialogueLine;
    }

   // Remaining implementation of Person class.
}