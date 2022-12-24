<p align="center">
  <img alt="Pokémon Unity Logo" src="https://styles.redditmedia.com/t5_39moy/styles/bannerPositionedImage_6is405sk53j01.png" />
  <h1 align="center">Pokémon Unity by IIcolour Spectrum</h1>
  <h2 align="center">Created with Pokémon Framework</h2>
  <p align="center">
    <a href="https://opensource.org/licenses/BSD-3-Clause"><img alt="License" src="https://img.shields.io/badge/license-New%20BSD-blue.svg"/></a>
    <a href="https://discord.gg/CCF2YVP"><img alt="Discord Server" src="https://img.shields.io/badge/join%20us%20on-discord-7289DA.svg"/></a>
    <a href="https://www.reddit.com/r/PokemonUnity/"><img alt="Reddit" src="https://img.shields.io/badge/join%20us%20on-reddit-ff5700.svg"/></a>
    <a href="https://herbertmilhomme.github.io/PokemonUnity/"><img alt="GitBook" src="https://img.shields.io/badge/view%20docs%20on-gitbook-blue.svg"/></a>
    <a href="https://herbertmilhomme.visualstudio.com/PokemonUnity/_build/index?definitionId=3"><img src="https://herbertmilhomme.visualstudio.com/_apis/public/build/definitions/90a2f24a-6d43-47cd-9e21-be259c022c96/3/badge"/></a>
  </p>
</p>

**Pokémon Unity** is a game project made in Unity, that lets people make Pokémon-style games. It was created by IIcolour Spectrum (Lucas), who originally authored it from scratch starting from a Unity project. As of 2017, IIcolour Spectrum handed code development over to FlakFlayster, who then refactored the project into a framework library and based it on **Pokémon Essentials** code source, from RPG Maker. Currently, it is managed by Gen, and still updated by FlakFlayster.

## Summary 

**Pokémon Framework** is a framework written in C-Sharp and designed to be built on top of, as a foundation and key component in any Pokémon remake or emulator. Because the project is abstract, so loosely coupled from any frontend component, it allows for the project to easily migrate between any engine or platform that's C# compatible (i.e. Unity3d, Websites/ASP.Net, or even commandline console, as a text-based Pokémon battle simulator).

This contains a C# port of [Pokémon Essentials](https://github.com/griest024/essentials-sample-project) to mirror the code to function the same (with very few and minor differences). I swapped out all of the data from **Pokémon Essentials** to use [Veekun's Database](https://github.com/veekun/pokedex), which is more expansive (more detailed, regularyly kept up-to-date, and formatted for database queries by default.

For additional context, this project rewrites **Pokémon Essentials** logic to implement Pokémon mechanics, using Veekun's Pokédex which rips data directly from Nintendo's Pokémon game, as foundation for **Pokémon Framework**. The game's text will be locally translated, through crowdsourcing that i've personally configured and setup on my own private webserver, which will return xml packages that can be downloaded and used as localized scripts by source. Then using [GameFramework for Unity](https://github.com/EllanJiang/GameFramework) as a template, i will integrate the base Pokémon Framework to overwrite the template data to run like a Pokémon game, while feeding the behavior into the Pokémon Unity assets as final render and output (game `.exe` compile). If you have any experience with Pokémon Essentials, but with a perference for Unity or C#, then this is the project for you. End users are still required to have an aptitude or interest in **hardwork**. As in, this project isnt making a game __for__ you, or shipping you with a completed game to enjoy and experience; this is a **FRAMEWORK** you still have to put in effort to make __your__ own "game" (and why the source codes and dll are being uploaded and not a link to an `.exe` download).

I'm basing the code mimic and emulate the [Pokémon Essentials](https://pokemon-essentials.fandom.com/wiki/Pokémon_Essentials_Wiki) package for RPG Maker MV, which is written in Ruby. But since Ruby follows similar object-oriented coding structures, it's easy to mirror the code to function the same.

## This Project with Pokemon Framework

I use IIcolour Spectrm's orginal Pokemon Unity as a base with Herbertmilhomme's framework. So far, this project use wrapper (Legacy <=> Framework) and in future, I want to remove wrapper. 

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
  - Use Unity Hub or Editor to import this project's folder (`/Pokemon Unity`)
  - <s>You will need to move database to `..\ ..\ ..\\veekun-pokedex.sqlite`. (Found in `/Pokemon Unity/Assets/Data`)</s>
Latest Commit should fix the issue. 
  - Open startup.scene and play it

#### How to build and run the project ####

For Window:
  - You build the project
  - Open Build folder and copy SQLite.Interop.dll from YourAppName/YourAppName_Data/Plugins to YourAppName/YourAppame_Data/Managed
  - Run YourAppName.exe

## Credits

* PKUnity Project Lead: [FlakFlayster](https://github.com/herbertmilhomme/)
* PKUnity Base author: [IIcolour Spectrum](https://www.reddit.com/user/IIcolour_Spectrum)/[superusercode](https://www.reddit.com/user/Lucas_One/)
* PKUnity Maintainer: [MyzTyn](https://github.com/MyzTyn/) and [Gen](https://github.com/gen3vra/)
* PKUnity Logo artist: [Kaihatsu](https://twitter.com/KaihatsuYT)

## Links

* Reddit: https://www.reddit.com/r/PokemonUnity/
* Discord server: https://discord.gg/AzW8Ds7MdE
* Project Board: [Not Frequently Used or Updated](https://github.com/herbertmilhomme/PokemonUnity/projects/1)
* Documentation: [Pokémon Essentials Wiki](https://pokemon-essentials.fandom.com/wiki/Pokemon_Essentials_Wiki) (or [My poorly written Github wiki](https://herbertmilhomme.github.io/PokemonUnity/))
* Documentation-Repo: https://github.com/herbertmilhomme/PokemonUnity/tree/gh-pages
* Database: [Veekun's Pokédex Github](https://github.com/veekun/pokedex)
* Unity Framework: [GameFramework UnityEngine C# Assets](https://github.com/EllanJiang/UnityGameFramework)
* Unity Framework: [GameFramework Vanilla C# Library](https://github.com/EllanJiang/GameFramework)
* Web-Server: TBD

### Demo and Downloads 

There are also demos for Windows, Linux, and Mac zipped in [2016 Release](https://github.com/PokemonUnity/PokemonUnity/releases)

## FAQ

- Is Pokemon Unity still in development? 
	- Yes
- Is the Unity part of this project being worked on? 
	- No
- Is there a more up to date version and runnable version of this? 
	- Not that we know
- How do I start developing my own Pokemon game? 
	- Thats completely up to you. The maintained Pokemon code in this repository is actually NOT related to Unity at all. This means someone can take the code here and start their own Pokemon game in any way that supports loading C# DLL libraries. Unity is just 1 way :)
- What relevance do the folders other than the `Pokemon Unity` folder have? 
	- The repo contains several projects. Each folder (probably) represents one of those projects. The `Pokemon Unity` folder is just where the Unity related files are found
		- TODO: add breakdown of contents of each parent folder

### Unity FAQ

- Why is my screen pink?
	- Unity metafiles were broken sometime in the past. Because of this, images in (probably) all scenes are corrupted and show up pink
- How do I learn Unity? 
	- There are several great tutorials on YouTube to learn Unity. To name a few
		- [Brackeys](https://www.youtube.com/@Brackeys)
		- [Code Monkey](https://www.youtube.com/@CodeMonkeyUnity)
		- [Jason Weimann](https://www.youtube.com/@Unity3dCollege)
		- [Unity](https://www.youtube.com/@unity)
		- [Sebastian Lague](https://www.youtube.com/@SebastianLague)
		- [Game Maker's Toolkit](https://www.youtube.com/@GMTK)
- Why isnt the Unity side of things being worked on?
	- There is only 1 developer currently active on this project. Their focus is on the non-Unity Pokemon framework
