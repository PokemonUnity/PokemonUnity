<p align="center">
  <img alt="Pokémon Unity Logo" src="https://styles.redditmedia.com/t5_39moy/styles/bannerPositionedImage_6is405sk53j01.png" />
  <h1 align="center">Pokémon Unity by IIcolour Spectrum</h1>
  <h2 align="center">Created with Pokémon Framework</h2>
  <h3 align="center">Based on Pokémon Essentials</h3>
  <p align="center">
    <a href="https://opensource.org/licenses/BSD-3-Clause"><img alt="License" src="https://img.shields.io/badge/license-New%20BSD-blue.svg"/></a>
    <a href="https://discord.gg/CCF2YVP"><img alt="Discord Server" src="https://img.shields.io/badge/join%20us%20on-discord-7289DA.svg"/></a>
    <a href="https://www.reddit.com/r/PokemonUnity/"><img alt="Reddit" src="https://img.shields.io/badge/join%20us%20on-reddit-ff5700.svg"/></a>
    <a href="https://herbertmilhomme.github.io/PokemonUnity/"><img alt="GitBook" src="https://img.shields.io/badge/view%20docs%20on-gitbook-blue.svg"/></a>
    <a href="https://herbertmilhomme.visualstudio.com/PokemonUnity/_build/index?definitionId=3"><img src="https://herbertmilhomme.visualstudio.com/_apis/public/build/definitions/90a2f24a-6d43-47cd-9e21-be259c022c96/3/badge"/></a>
  </p>
</p>

**Pokémon Unity** is a versatile game project, written in C# and built upon the Unity platform. Designed for passionate fans, this project aims to provide a platform for creating Pokémon-style games. Originating from the efforts of IIcolour Spectrum (Lucas), the project has transformed over the years, seeing leadership change hands to FlakFlayster in 2017 and currently managed by Gen, with continued contributions from FlakFlayster.

## Overview 

**Pokémon Framework** forms the backbone of this project. Crafted in C#, this robust framework provides a solid foundation for any Pokémon-style remake or emulator. Its abstract design, completely independent of any frontend component, offers versatility, allowing migration between any C# compatible engine or platform (e.g., Unity3D, ASP.NET Websites, or even a text-based Pokémon battle simulator in a command-line console).

This project also includes a C# adaptation of [Pokémon Essentials](https://github.com/griest024/essentials-sample-project), designed to function identically, with only minor variances. To ensure an expansive and updated database, we've substituted the data from **Pokémon Essentials** with [Veekun's Database](https://github.com/veekun/pokedex), which is designed for database queries and is kept up-to-date regularly.

Fundamentally, the project reengineers the **Pokémon Essentials** logic to implement Pokémon mechanics. It leverages Veekun's Pokédex data (extracted straight from Nintendo's Pokémon games) to enrich the **Pokémon Framework**. Crowd-sourced local translations have been set up on a private webserver, which can be downloaded and used as localized scripts. The final render and output are integrated with the [GameFramework for Unity](https://github.com/EllanJiang/GameFramework), overwriting its template data to operate like a Pokémon game.

If you're familiar with Pokémon Essentials, have a preference for Unity or C#, and are willing to put in the effort, this project is a perfect match for you. However, it's important to note that this is not a ready-made game. It's a **FRAMEWORK**, a basis for you to build your own unique game upon. Hence, you will find source codes and DLL files rather than a game executable.

## Adapting Pokémon Essentials 

The project's code is designed to mimic and emulate the [Pokémon Essentials](https://pokemon-essentials.fandom.com/wiki/Pokémon_Essentials_Wiki) package for RPG Maker MV, originally written in Ruby. Given Ruby's similar object-oriented coding structures, the code can be easily adapted to function identically in our C# project.

## Current Project Status

The project builds upon IIcolour Spectrum's original Pokémon Unity, combining it with Herbertmilhomme's framework. While the project is currently playable, it's in an evolutionary phase with some features temporarily disabled and minor issues present. 

For now, we advise against using this project as a foundation for building a game. However, we're making strides towards future improvements and eliminating the need for wrapper functions.

## Demo and Build Instructions 

**To open and play a demo in Unity:**
  - Import the project's folder (`/Pokemon Unity`) using Unity Hub or Editor.
  - Navigate to and open `sampleScene.unity`, then play it.

**To build and run the project on Windows:**
  - You will need to move database to `/Pokemon Unity/Assets/Data`. (Found in `..\ ..\ ..\\veekun-pokedex.sqlite` or the repository's [root folder](https://github.com/PokemonUnity/PokemonUnity/blob/master/veekun-pokedex.sqlite))
    - And uncomment [line 58](https://github.com/PokemonUnity/PokemonUnity/blob/be6672c41bbea75364b1efe342b8662070806dad/Pokemon%20Unity/Assets/Scripts/Scene/GameEvents.cs#L58) from `Assets/Scripts/Scene/GameEvents.cs`
  - Build the project.
  - In the Build folder, copy `SQLite.Interop.dll` from `YourAppName/YourAppName_Data/Plugins` to `YourAppName/YourAppName_Data/Managed`.
  - Finally, run `YourAppName.exe

**To download the project and play it without using unity** 

There are also demos for Windows, Linux, and Mac zipped in [2016 Release](https://github.com/PokemonUnity/PokemonUnity/releases)

Stay tuned for updates and exciting changes as we continue to develop the Pokémon Unity Project!

## Credits

* PKUnity Project Lead: [FlakFlayster](https://github.com/herbertmilhomme/)
* PKUnity Base author: [IIcolour Spectrum](https://www.reddit.com/user/IIcolour_Spectrum)/[superusercode](https://www.reddit.com/user/Lucas_One/)
* PKUnity Maintainer: [MyzTyn](https://github.com/MyzTyn/) and [Gen](https://github.com/gen3vra/)
* PKUnity Logo artist: [Kaihatsu](https://twitter.com/KaihatsuYT)

## Links

* Reddit: https://www.reddit.com/r/PokemonUnity/
* Discord server: https://discord.gg/AzW8Ds7MdE
* Project Board: [Not Frequently Used or Updated](https://github.com/herbertmilhomme/PokemonUnity/projects/1)
* Documentation: [Pokémon Essentials Wiki](https://pokemon-essentials.fandom.com/wiki/Pokemon_Essentials_Wiki) (or [My poorly written Github Wiki 1](https://herbertmilhomme.github.io/PokemonUnity/) and [My poorly written Github Wiki 2](https://github.com/herbertmilhomme/PokemonUnity/tree/gh-pages))
* Documentation-Repo: [Pokémon Framework Library Wiki](https://github.com/PokemonUnity/pklibrary/tree/dev_feature_web-docs) (Web Resources TBD)
* Unity Framework: [GameFramework UnityEngine C# Assets](https://github.com/EllanJiang/UnityGameFramework)
* Unity Framework: [GameFramework Vanilla C# Library](https://github.com/EllanJiang/GameFramework)
* Web-Server: TBD 