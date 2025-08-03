# Introduction
This Parser is a tool for taking in a .csv and outputting a .yarn file (usable by [YarnSpinner]([url](https://docs.yarnspinner.dev/))). It is most helpful for narrative games that will use a lot of complex Yarn Commands. 
The project has an example case for the game "Starweave". This parser was developed because every line of character needed a function call before it that would swap out which character portrait is shown based on the speaker and their emotion (plus other things, such as play audio, but that has been removed for simplicity's sake for this repo!). 

# How to Use
**Note: YarnSpinner is not in this Unity project, I was too lazy to figure out which version of YarnSpinner I need :D
1. Open Tools > Parser (at the top bar). This will open the Parser. 
<img width="1226" height="146" alt="image" src="https://github.com/user-attachments/assets/97b3d7b5-1a91-48cd-a2d1-0513ae11488a" />

2. Select target .csv

   <img width="946" height="224" alt="image" src="https://github.com/user-attachments/assets/74655cca-d1b2-494a-a4b8-2c2db00e8f9e" />

3. Select the target output folder.

4. Click Convert to Yarn! It should only take a second or two, and once it’s completed, a properly formatted .yarn file will appear in the Output Folder!

<img width="728" height="542" alt="image" src="https://github.com/user-attachments/assets/3ca50768-3f20-4a4c-84c1-769d9a25285f" />

# .csv Setup 
This Unity project has an example that is designed to parse a spreadsheet set up like this: 
<img width="1171" height="191" alt="image" src="https://github.com/user-attachments/assets/45581696-7489-4515-a952-a7e1ac902018" />

The columns are the speaker's name, their emotion, and their actual dialogue line. The parser's default behavior is to treat every individual row as a "data" packages (basically everything that will be needed to execute an individual line of dialogue that are shown via YarnSpinner (what changes upon clicking "continue"). 
You can set up your own .csv however desired, but pay very close attention to syntax and setup. 
Be mindful that a .csv will use COMMAS to separate the columns, so be careful using commas within cells. It could lead to unintended behavior if you ever try and use commas to split the file. 

If you are using [YarnCommands]([url](https://docs.yarnspinner.dev/write-yarn-scripts/scripting-fundamentals/commands )), make sure you use a special, consistent syntax for calling them. In my example, we used `SYSTEM` in column A, and used square brackets ([ ]) on both sides of the `YarnCommand`'s name to let the parser know to search for a `YarnCommand`.

# Implementation 
In `ParserWindow.cs`, look for all `TODO` comments. This will let you know which parts of the script to add your custom parsing functionality to! 

# YarnSpinner setup
Create a YarnProject in the same folder that the Yarn files are outputting to. This will automatically add the .yarn file to the YarnProject you are using. 

# Example Setup
In the scene ParserDemo, there is a sample Project and .yarn file that was generated using the Parser Tool. 
It uses the YarnParserDemo.csv to show the character name, character sprite and dialogue. The sprite is set via SetSpeakerInfo() in YarnCommands.cs

``` C#
[YarnCommand("initialize_line")]
public static void SetSpeakerInfo(string name, string expression)
{
   speaker.sprite = speakerDictionary[name];
}
```
