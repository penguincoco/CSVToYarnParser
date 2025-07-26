# How to Use
1. Open Tools > Parser (at the top bar). This will open the Parser. 
<img width="1226" height="146" alt="image" src="https://github.com/user-attachments/assets/97b3d7b5-1a91-48cd-a2d1-0513ae11488a" />

2. Select target .csv

   <img width="946" height="224" alt="image" src="https://github.com/user-attachments/assets/74655cca-d1b2-494a-a4b8-2c2db00e8f9e" />

3. Select the target output folder.
<img width="1560" height="382" alt="image" src="https://github.com/user-attachments/assets/354b3f87-9368-455a-8f71-5ef1a4a50415" />

4. Click Convert to Yarn! (the process is slow, give it up to two minutes), but once it’s completed, a properly formatted .yarn file will appear in the Output Folder!

<img width="728" height="542" alt="image" src="https://github.com/user-attachments/assets/3ca50768-3f20-4a4c-84c1-769d9a25285f" />

# .csv Setup 
This Unity project has an example that is designed to parse a spreadsheet set up like this: 
<img width="1171" height="191" alt="image" src="https://github.com/user-attachments/assets/45581696-7489-4515-a952-a7e1ac902018" />

The columns are the speaker's name, their emotion, and their actual dialogue line. The parser's default behavior is to treat every individual row as a "data" packages (basically everything that will be needed to execute an individual line of dialogue that are shown via YarnSpinner (what changes upon clicking "continue"). 
You can set up your own .csv however desired, but pay very close attention to syntax and setup. 
Be mindful that a .csv will use COMMAS to separate the columns, so be careful using commas within cells. It could lead to unintended behavior if you ever try and use commas to split the file. 
