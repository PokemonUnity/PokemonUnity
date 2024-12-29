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

**Pokémon Unity** is a flexible and powerful backend tool designed to help developers build their own Pokémon-style games using **C#**. While the project name references Unity, the framework itself is highly **platform-agnostic**. The core logic of this project is written in pure C#, meaning it can be integrated into **any environment** that supports C# and can load DLL files, not just Unity.

## Who is This For?

If you're asking:
- "How do I program a Pokémon game?"
- "Where can I find a framework that handles the core mechanics of a Pokémon game so I don't have to build everything from scratch?"

Then this project is for you!

But if you're wondering:
- "Where can I find art assets to make my game look like this?"
- "Will my game look like this if I download the project?"

This might **not** be what you're looking for.

### What Pokémon Unity Is:
- A **framework** that provides the core game logic for Pokémon-style games, written in **vanilla C#**, making it adaptable to a variety of platforms.
- **Not tied to Unity** alone—you can use this framework with any platform that supports C# and DLLs, including Unity3D, desktop applications, web-based platforms like ASP.NET, or even a text-based simulator.
- A **tool** for developers looking to focus on programming game mechanics rather than front-end design or art. You’ll work with the backend logic of things like battles, moves, and stats—how the game *works*, not how it *looks*.

### What Pokémon Unity Is Not:
- **Not a ready-made game**—this is a developer’s tool, not a finished game you can download and run. There are no built-in art assets or levels.
- **Not limited to Unity**—despite the project name, the framework is versatile and can be used in any C# compatible environment.

## Project Overview

At its core, **Pokémon Unity** revolves around the **[Pokémon Framework](https://github.com/PokemonUnity/pklibrary)**, which distills the essence of Pokémon game mechanics into a pure, platform-agnostic **C# implementation**. This means that while the project is commonly used in Unity, the logic can be easily ported to any platform that supports C#.

For example, you can export this project as a **DLL** and plug it into:
- Unity3D for game development.
- An ASP.NET website to build a web-based Pokémon battle simulator.
- A command-line console for a text-based Pokémon game.

The project includes a C# adaptation of [Pokémon Essentials](https://github.com/griest024/essentials-sample-project), ensuring that the game mechanics behave similarly to that package, but with the flexibility of being in C#. It also integrates data from [Veekun's Database](https://github.com/veekun/pokedex), giving you access to up-to-date Pokémon data that powers the framework’s game logic.

### Key Takeaway:
**Pokémon Unity** is designed to be an adaptable foundation for developers who are comfortable coding in C#. It provides the core mechanics for Pokémon-style games but can be used in **any** C# compatible environment. Whether you want to build a game in Unity or another engine, or even just experiment with Pokémon mechanics in a different application, this project gives you the tools to start coding without having to reinvent the wheel.

## Current Project Status

The project builds upon IIcolour Spectrum's original Pokémon Unity, combining it with HerbertMilhomme's framework. While the project is currently playable, it's in an evolutionary phase with some features temporarily disabled and minor issues present. 

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

There are also demos for Windows, Linux, and Mac zipped in [2016 Release](https://github.com/PokemonUnity/PokemonUnity/releases) (This is the version used in the YouTube video, if you're just looking to play it.)

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
* Documentation-Repository: [Pokémon Framework Library Wiki](https://github.com/PokemonUnity/pklibrary/tree/dev_feature_web-docs) (Web Resources TBD)
* Database: [Veekun's Pokédex Github](https://github.com/veekun/pokedex)
* Unity Framework: [GameFramework UnityEngine C# Assets](https://github.com/EllanJiang/UnityGameFramework)
* Unity Framework: [GameFramework Vanilla C# Library](https://github.com/EllanJiang/GameFramework)
* Web-Server: TBD 