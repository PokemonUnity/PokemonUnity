<p align="center">
  <h1 align="center">Pokémon Framework by Herbert Milhomme using</h1>
  <p align="center">
    <a href="https://opensource.org/licenses/BSD-3-Clause"><img alt="License" src="https://img.shields.io/badge/license-New%20BSD-blue.svg"/></a>
    <a href="https://herbertmilhomme.github.io/PokemonUnity/"><img alt="GitBook" src="https://img.shields.io/badge/view%20docs%20on-gitbook-blue.svg"/></a>
    <a href="https://herbertmilhomme.visualstudio.com/PokemonUnity/_build/index?definitionId=3"><img src="https://herbertmilhomme.visualstudio.com/_apis/public/build/definitions/90a2f24a-6d43-47cd-9e21-be259c022c96/3/badge"/></a>
  </p>
</p>

Pokémon Framework is a framework written in csharp and designed to be built on top of, as a foundation and key component in any Pokémon remake or emulator. Because the project is so loosely coupled from any frontend component, it allows for the project to easily migrate between any engine or platform that's C# compatible. (Unity3d, Websites/ASP.Net, or even commandline console, as a text based Pokémon battle simulator).

This contains a C# port of [Pokémon Essentials](https://github.com/griest024/essentials-sample-project) (extension package for RPG Maker MV, which is written in Ruby), that I authored myself. Since Ruby follows similar object-oriented coding structures, it's easy to mirror the code to function the same (with very few and minor differences). 

I have changed so much of the original [Pokémon Unity](https://github.com/PokemonUnity/PokemonUnity) code on the backend that this project runs and functions more like pokemon essentials, but with pokemon unity assets on frontend as a wrapper. I swapped out all of the data from Pokemon Essentials to use [Veekun's Database](https://github.com/veekun/pokedex), which is more expansive (more detailed, regularyly kept up-to-date, and formatted for database queries by default

To summarize, this project rewrites Pokemon Essentials logic to implement Pokemon mechanics, using Veekun's Pokedex which rips data directly from Pokemon game, as foundation for Pokemon Framework. The game's text will be locally translated, through crowdsourcing that i've personally configured and setup on my own private webserver, which will return xml packages that can be downloaded and used as localized scripts by source. Then using [GameFramework for Unity](https://github.com/EllanJiang/GameFramework) as a template, i will integrate the base Pokemon Framework to overwrite the template data to run like a Pokemon game, while feeding the behavior into the Pokemon Unity assets as final render and output (game `.exe` compile).

Please be aware that some features in this branch are incomplete (the features in the [test-project branch](https://github.com/herbertmilhomme/PokemonUnity/tree/TestProject) is being tracked and monitored by the build status). I strongly recommend that you use the master branch as a base rather than the [test-project branch](https://github.com/herbertmilhomme/PokemonUnity/tree/TestProject). This branch is entirely experimental and contains wild erratic ideas, which has possibility of breaking your fangame or other project that is based on this code.

Please report any bugs if found.

This project is **NOT** affiliated with Pokemon Essentials, Project Veekun, GameFramework, nor Pokemon Unity (I _was_ one of the core devs, and branched out into my own unique project).

## Credits
### Links

* Project Board: [Not Frequently Used or Updated](https://github.com/herbertmilhomme/PokemonUnity/projects/1)
* Documentation: [Pokemon Essentials Wiki](http://pokemonessentials.wikia.com/wiki/Pok%C3%A9mon_Essentials_Wiki) (or [My poorly written Github wiki](https://herbertmilhomme.github.io/PokemonUnity/))
* Documentation-Repo: [Github Pages Repo](https://github.com/herbertmilhomme/PokemonUnity/tree/gh-pages)
* Database: [Veekun's Pokedex Github](https://github.com/veekun/pokedex)
* Web-Server: TBD
* PKUnity Author: [IIcolour Spectrum](https://www.reddit.com/user/IIcolour_Spectrum)
* Unity Framework: [GameFramework](https://github.com/EllanJiang/GameFramework)