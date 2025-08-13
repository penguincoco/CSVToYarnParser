# Introduction
This Parser is a tool for taking in a .csv and outputting a .yarn file (usable by [YarnSpinner]([url](https://docs.yarnspinner.dev/))). It is most helpful for narrative games that will use a lot of complex [YarnCommands]([url](https://docs.yarnspinner.dev/write-yarn-scripts/scripting-fundamentals/commands )). 

The project has an example case for the game "Starweave". This parser was developed because every line of character needed a function call before it that would swap out which character portrait is shown based on the speaker and their emotion (plus other things, such as play audio, but that has been removed for simplicity's sake for this repo!). 

# How to Use
## Setup
There are a few steps that need to be taken before actually using the parser. Please see [.csv setup] and [Creating and Populating `AvailableActions`] before following the steps below.

1. Open Tools > Parser (at the top bar). This will open the Parser. 
<img width="1226" height="146" alt="image" src="https://github.com/user-attachments/assets/97b3d7b5-1a91-48cd-a2d1-0513ae11488a" />

2. Select target .csv

   <img width="946" height="224" alt="image" src="https://github.com/user-attachments/assets/74655cca-d1b2-494a-a4b8-2c2db00e8f9e" />

3. Select the target output folder.

4. Set a `AvailableActions` in "Available Actions Data"--this field is necessary for the parser to read custom synax used in your spreadsheet to replace it with `YarnCommand` calls. See [Creating and Populating `AvailableActions`](#Creating-and-Populating-`AvailableActions`) for how to set up your syntax and `YarnCommand` calls.
**Note: it will default to `Default_AvailableActions` if there is no "Available Actions Data" set. 

7. Click Convert to Yarn! It should only take a second or two, and once it’s completed, a properly formatted .yarn file will appear in the Output Folder! The console will also print the file output path. 

<img width="728" height="542" alt="image" src="https://github.com/user-attachments/assets/3ca50768-3f20-4a4c-84c1-769d9a25285f" />

# .csv Setup 
This Unity project has an example that is designed to parse a spreadsheet set up like this: 
<img width="1171" height="191" alt="image" src="https://github.com/user-attachments/assets/45581696-7489-4515-a952-a7e1ac902018" />

The columns are the speaker's name, their emotion, and their actual dialogue line. The parser's default behavior is to treat every individual row as a "data" packages (basically everything that will be needed to execute an individual line of dialogue that are shown via YarnSpinner (what changes upon clicking "continue"). 
You can set up your own .csv however desired, but pay very close attention to syntax and setup. 
Be mindful that a .csv will use COMMAS to separate the columns, so be careful using commas within cells. It could lead to unintended behavior if you ever try and use commas to split the file. 
The .csv setup I used was all dialogue is wrapped in quotation marks. 

If you are using [YarnCommands]([url](https://docs.yarnspinner.dev/write-yarn-scripts/scripting-fundamentals/commands )), make sure you use a special, consistent syntax for calling them. In my example, we used `SYSTEM` in column A, and used square brackets ([ ]) on both sides of the `YarnCommand`'s name to let the parser know to search for a `YarnCommand`.

# Creating and Populating `AvailableActions`
![Actions Dictionary](image.png)

If you are using [YarnCommands]([url](https://docs.yarnspinner.dev/yarn-spinner-for-unity/creating-commands-functions)), make sure you use a special, consistent syntax for calling them. The `Action Editor` allows you to add the syntax on your spreadsheet and associated `YarnCommand` as Key-Value pairs (KVP) and store them to a ScriptableObject. You can also have multiple Dictionaries. This allows you to parse different sheets  and read from different dictionaries if desired! 

Right click: Create > Utilities > Available Actions Database. This will create a `AvailableActionsData` base that must be saved to the `Resources` folder. If it is not, when parsing a `.csv` an error will be thrown.

1. Enter a KVP
Key: the spreadsheet syntax 
Value: the correctly formatted `YarnCommand` call to insert in the `.yarn` file.

2. Click "Add Action". It will populate below under "Existing Actions" 
**Note: direct, manual editing each entry into the Existing Actions dictionary is allowed. 


# Implementation 
## Parser.cs 
`Parser.cs` handles the parsing logic for the .csv. It reads in the .csv as a string, and splits it first by linebreaks. 
Replace `TODO` in `ConvertToYarn()` with your own custom implmentation, depending on how your .csv is set up. 

## ParserUtilities
'DialogueLine` is a class for each row of dialogue in the original .csv/spreadsheet. `Translate()` handles taking in the input and formatting it as desired for the output .yarn file. In my case, I needed a function call before every line of character dialogue, so the function injects a `<< initialize_line >>` before every spoken dialogue line. 

Replace the body of `Translate()` to implement your own custom .yarn output based on your project's needs. 

`Utilities` is a class for adding YarnCommand functions. Replace/add your own custom Yarn Functions to `availableActions` dictionary to inject their calls into the .yarn file. 

# YarnSpinner setup
Create a YarnProject in the same folder that the Yarn files are outputting to. This will automatically add the .yarn file to the YarnProject you are using. 


# Example Setup
In the scene `ParserDemo`, there is a sample Project and `.yarn` file that was generated using the Parser Tool. 

It reads from `LotR.csv` to show the character name, character sprite and text dialogue. 
The sprite is set via SetSpeakerInfo() in `YarnCommands.cs`. 

``` C#
[YarnCommand("initialize_line")]
public static void SetSpeakerInfo(string name, string expression)
{
   speaker.sprite = speakerDictionary[name];
}
```
