<p align="center">
  <img alt="Pokemon Unity Logo" src="https://i.imgur.com/2wzGSbW.png" />
  <h1 align="center">Pokemon Unity by IIcolour Spectrum</h3>
  <p align="center">
    <a href="https://opensource.org/licenses/BSD-3-Clause"><img alt="License" src="https://img.shields.io/badge/license-New%20BSD-blue.svg"></a>
    <a href="https://discord.gg/CCF2YVP"><img alt="Discord Server" src="https://img.shields.io/badge/join%20us%20on-discord-7289DA.svg"></a>
    <a href="https://www.reddit.com/r/PokemonUnity/"><img alt="Reddit" src="https://img.shields.io/badge/join%20us%20on-reddit-ff5700.svg"></a>
  </p>
</p>

## This Project with Pokemon Framework

I use IIcolour Spectrm's orginal Pokemon Unity as a base with Herbertmilhomme's framework. So far, this project use wrapper ( Legacy <=> Framework ) and in future, I want to remove wrapper. 

This project is playable, but it has some issues and some features are disabled.

I recommend to not use this project as base to build a game, yet. 

#### My next plan ####
- Fix save file system
- Remove the wrapper
- Refactor Backend scripts
- Rewrite UI scene scripts
- Fix the known bugs

#### How to open the project and play a demo ####

For Unity: (2018.3.7f1)
  - Use Unity Hub or Editor to import this project's folder (/Pokemon Unity)
  - <s>You will need to move database to ..\ ..\ ..\ \veekun-pokedex.sqlite. ( Found in /Pokemon Unity/Assets/Data )</s>
Latest Commit should fix the issue. 
  - Open startup.scene and play it

#### How to build and run the project ####

For Window:
  - You build the project
  - Open Build folder and copy SQLite.Interop.dll from YourAppName/YourAppName_Data/Plugins to YourAppName/YourAppame_Data/Managed
  - Run YourAppName.exe

## Demo and Downloads

There are also demos for Windows, Linux, and Mac zipped in [Releases](https://github.com/PokemonUnity/PokemonUnity/releases)! Please report any bugs if found.

## Contact

* Reddit: https://www.reddit.com/r/PokemonUnity/
* Discord server: https://discord.gg/AzW8Ds7MdE
